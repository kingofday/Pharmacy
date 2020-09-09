using Elk.Core;
using System;
using System.Linq;
using Pharmacy.Domain;
using System.Threading.Tasks;
using Pharmacy.DataAccess.Ef;
using System.Linq.Expressions;
using Pharmacy.Service.Resource;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        readonly AppUnitOfWork _appUow;
        public readonly IGenericRepo<Prescription> _presRepo;
        readonly IAttachmentService _attchSrv;
        readonly INotificationService _notifSrv;
        public PrescriptionService(AppUnitOfWork appUow, IAttachmentService attchSrv, INotificationService notifSrv)
        {
            _appUow = appUow;
            _presRepo = appUow.PrescriptionRepo;
            _attchSrv = attchSrv;
            _notifSrv = notifSrv;
        }

        public virtual async Task<Response<int>> Add(AddPrescriptionModel model)
        {
            var notifications = new List<NotificationDto>();
            var save = await _attchSrv.Save(AttachmentType.PrescriptionImage, model.Files, model.AppDir);
            if (!save.IsSuccessful)
                return new Response<int> { Message = save.Message };
            var prescription = new Prescription
            {
                Status = PrescriptionStatus.Added,
                Attachments = save.Result.Select(x => new PrescriptionAttachment
                {
                    AttachmentType = x.AttachmentType,
                    FileType = x.FileType,
                    Url = x.Url,
                    Extention = x.Extention,
                    FileName = x.FileName,
                    OriginalName = x.OriginalName,
                    PhysicalPath = x.PhysicalPath,
                    Size = x.Size
                }).ToList()
            };
            NotificationDto subNotif = null;
            if (model.UserId == null)
            {
                var mobNum = long.Parse(model.MobileNumber);
                var user = await _appUow.UserRepo.FirstOrDefaultAsync(new BaseFilterModel<User> { Conditions = x => x.MobileNumber == mobNum });
                if (user != null)
                {
                    model.Fullname = user.FullName;
                    prescription.UserId = user.UserId;
                }
                else
                {
                    model.Fullname = $"کاربر-{model.MobileNumber}";
                    subNotif = new NotificationDto
                    {
                        Content = string.Format(NotifierMessage.UserSubscriptionViaPrescription, model.Fullname),
                        FullName = model.Fullname,
                        MobileNumber = long.Parse(model.MobileNumber),
                        Type = EventType.Subscription
                    };
                    prescription.User = new User
                    {
                        FullName = model.Fullname,
                        MobileNumber = long.Parse(model.MobileNumber),
                        Password = HashGenerator.Hash(model.MobileNumber)
                    };
                }

            }

            else prescription.UserId = model.UserId ?? Guid.Empty;
            await _presRepo.AddAsync(prescription);
            var add = await _appUow.ElkSaveChangesAsync();
            if (add.IsSuccessful)
            {
                if (subNotif != null) await _notifSrv.NotifyAsync(subNotif);
                await _notifSrv.NotifyAsync(new NotificationDto
                {
                    Content = string.Format(NotifierMessage.SubmitPrescription, model.Fullname),
                    FullName = model.Fullname,
                    MobileNumber = long.Parse(model.MobileNumber),
                    Type = EventType.Subscription
                });
            }
            return new Response<int>
            {
                IsSuccessful = add.IsSuccessful,
                Message = add.Message,
                Result = prescription.PrescriptionId
            };
        }

        public PagingListDetails<Prescription> Get(PrescriptionSearchFilter filter)
        {
            Expression<Func<Prescription, bool>> conditions = x => true;
            if (filter != null)
            {
                if (filter.PrescriptionId != null)
                    conditions = conditions.And(x => x.PrescriptionId == filter.PrescriptionId);
                if (!string.IsNullOrWhiteSpace(filter.MobileNumber))
                {
                    long mobNum = 0;
                    if (long.TryParse(filter.MobileNumber, out mobNum))
                        conditions = conditions.And(x => x.User.MobileNumber == mobNum);
                }
            }

            return _presRepo.Get(new BasePagedListFilterModel<Prescription>
            {
                Conditions = conditions,
                PagingParameter = filter,
                OrderBy = x => x.OrderByDescending(i => i.PrescriptionId),
                IncludeProperties = new System.Collections.Generic.List<Expression<Func<Prescription, object>>> { x => x.User }
            });
        }

        public async Task<IResponse<Prescription>> FindDetailsAsync(int id)
        {
            var pres = await _presRepo.FirstOrDefaultAsync(new BaseFilterModel<Prescription>
            {
                Conditions = x => x.PrescriptionId == id,
                IncludeProperties = new List<Expression<Func<Prescription, object>>> {
                    x=>x.User,
                    x => x.Attachments }
            });
            if (pres == null) return new Response<Prescription> { Message = ServiceMessage.RecordNotExist };
            pres.Items = _appUow.PrescriptionItemRepo.Get(new BaseListFilterModel<PrescriptionItem>
            {
                Conditions = x => x.PrescriptionId == id,
                IncludeProperties = new System.Collections.Generic.List<Expression<Func<PrescriptionItem, object>>> { x => x.Drug },
                OrderBy = o => o.OrderByDescending(x => x.DrugId)
            });
            return new Response<Prescription> { Result = pres, IsSuccessful = true };
        }

        public async Task<IResponse<Prescription>> UpdateAsync(Prescription model)
        {
            var pres = await _presRepo.FindAsync(model.PrescriptionId);
            if (pres == null) return new Response<Prescription> { Message = ServiceMessage.RecordNotExist };
            if (model.Items != null)
                foreach (var p in model.Items)
                {
                    var drug = await _appUow.DrugRepo.FindAsync(p.DrugId);
                    if (drug == null)
                        return new Response<Prescription> { Message = ServiceMessage.RecordNotExist };
                    p.Price = drug.Price;
                    p.DiscountPrice = drug.DiscountPrice;
                    p.TotalPrice = (drug.Price - drug.DiscountPrice) * p.Count;
                }
            pres.Items = model.Items;
            _presRepo.Update(pres);
            var update = await _appUow.ElkSaveChangesAsync();
            return new Response<Prescription> { IsSuccessful = update.IsSuccessful, Message = update.Message, Result = pres };
        }

        public async Task<IResponse<List<PrescriptionItem>>> DeleteItem(int itemId)
        {
            var item = await _appUow.PrescriptionItemRepo.FindAsync(itemId);
            if (await _appUow.OrderRepo.AnyAsync(new BaseFilterModel<Order> { Conditions = x => x.PrescriptionId == item.PrescriptionId }))
                return new Response<List<PrescriptionItem>> { Message = ServiceMessage.NotAllowedOperation };
            _appUow.PrescriptionItemRepo.Delete(item);
            var delete = await _appUow.ElkSaveChangesAsync();
            return new Response<List<PrescriptionItem>>
            {
                IsSuccessful = delete.IsSuccessful,
                Message = delete.Message,
                Result = delete.IsSuccessful ? _appUow.PrescriptionItemRepo.Get(new BaseListFilterModel<PrescriptionItem>
                {
                    Conditions = x => x.PrescriptionId == item.PrescriptionId,
                    OrderBy = o => o.OrderByDescending(x => x.PrescriptionItemId),
                    IncludeProperties = new List<Expression<Func<PrescriptionItem, object>>> {
                        x=>x.Drug
                    }
                }) : new List<PrescriptionItem>()
            };
        }

        public async Task<IResponse<string>> SendLink(int id, string url)
        {
            var pres = await _presRepo.FirstOrDefaultAsync(new BaseFilterModel<Prescription>
            {
                Conditions = x => x.PrescriptionId == id,
                IncludeProperties = new List<Expression<Func<Prescription, object>>> { x => x.User }
            });
            if (pres == null) return new Response<string> { Message = ServiceMessage.RecordNotExist };
            var send = await _notifSrv.NotifyAsync(new NotificationDto
            {
                Content = string.Format(NotifierMessage.TempBasketUrl, $"{url}/{id}"),
                MobileNumber = pres.User.MobileNumber,
                Type = EventType.Subscription
            });
            return new Response<string>
            {
                IsSuccessful = send.IsSuccessful,
                Message = send.IsSuccessful ? ServiceMessage.SentSuccessfully : send.Message
            };
        }

        public Response<List<DrugDTO>> GetItems(int id, string baseUrl)
        {
            var items = _appUow.PrescriptionItemRepo.Get(new BaseListFilterModel<PrescriptionItem>
            {
                Conditions = x => x.PrescriptionId == id,
                OrderBy = o => o.OrderBy(x => x.PrescriptionItemId),
                IncludeProperties = new List<Expression<Func<PrescriptionItem, object>>> { x => x.Drug, x => x.Drug.Unit, x => x.Drug.DrugAttachments }
            });
            return new Response<List<DrugDTO>>
            {
                IsSuccessful = items.Any(),
                Result = items.Any() ? items.Select(x => new DrugDTO
                {
                    DrugId = x.DrugId,
                    Count = x.Count,
                    UniqueId = x.Drug.UniqueId,
                    Price = x.Price,
                    DiscountPrice = x.DiscountPrice,
                    NameFa = x.Drug.NameFa,
                    NameEn = x.Drug.NameEn,
                    ShortDescription = x.Drug.ShortDescription,
                    UnitName = x.Drug.Unit.Name,
                    ThumbnailImageUrl = x.Drug.DrugAttachments.Where(x => x.AttachmentType == AttachmentType.DrugThumbnailImage).Select(x => baseUrl + x.Url).FirstOrDefault()
                }).ToList() : null,
                Message = items.Any() ? null : ServiceMessage.RecordNotExist
            };

        }
    }
}

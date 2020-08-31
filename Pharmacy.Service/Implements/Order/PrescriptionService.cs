using Elk.Core;
using System;
using System.Linq;
using Pharmacy.Domain;
using System.Threading.Tasks;
using Pharmacy.DataAccess.Ef;
using System.Linq.Expressions;
using Pharmacy.Service.Resource;

namespace Pharmacy.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        readonly AppUnitOfWork _appUow;
        public readonly IGenericRepo<Prescription> _presRepo;
        readonly IAttachmentService _attchSrv;
        public PrescriptionService(AppUnitOfWork appUow, IAttachmentService attchSrv)
        {
            _appUow = appUow;
            _presRepo = appUow.PrescriptionRepo;
            _attchSrv = attchSrv;
        }

        public virtual async Task<Response<int>> Add(AddPrescriptionModel model)
        {
            var save = await _attchSrv.Save(AttachmentType.PrescriptionImage, model.Files, model.AppDir);
            if (!save.IsSuccessful)
                return new Response<int> { Message = save.Message };
            var mobNum = long.Parse(model.MobileNumber);
            var user = await _appUow.UserRepo.FirstOrDefaultAsync(new BaseFilterModel<User> { Conditions = x => x.MobileNumber == mobNum });
            if (user != null)
                model.UserId = user.UserId;
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
            if (user == null)
                prescription.User = new User
                {
                    FullName = $"کاربر-{model.MobileNumber}",
                    MobileNumber = mobNum,
                    Password = HashGenerator.Hash(model.MobileNumber)
                };
            else prescription.UserId = user.UserId;
            await _presRepo.AddAsync(prescription);
            var add = await _appUow.ElkSaveChangesAsync();
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

        public async Task<IResponse<Prescription>> FindAsync(int id)
        {
            var pres = await _presRepo.FirstOrDefaultAsync(new BaseFilterModel<Prescription>
            {
                Conditions = x => x.PrescriptionId == id,
                IncludeProperties = new System.Collections.Generic.List<Expression<Func<Prescription, object>>> {
                    x=>x.User,
                    x=>x.Items,
                    x => x.Attachments }
            });
            if (pres == null) return new Response<Prescription> { Message = ServiceMessage.RecordNotExist };
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
                    if(drug==null)
                        return new Response<Prescription> { Message = ServiceMessage.RecordNotExist };
                    p.Price = drug.Price;
                    p.DiscountPrice = drug.DiscountPrice;
                    p.TotalPrice = p.Price * p.Count;
                }
            pres.Items = model.Items;
            _presRepo.Update(pres);
            var update = await _appUow.ElkSaveChangesAsync();
            return new Response<Prescription> { IsSuccessful = update.IsSuccessful, Message = update.Message, Result = pres };
        }
    }
}

using Elk.Core;
using Pharmacy.DataAccess.Ef;
using Pharmacy.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var user = await _appUow.UserRepo.FirstOrDefaultAsync(conditions: x => x.MobileNumber == mobNum);
            if (user != null)
                model.UserId = user.UserId;
            var prescription = new Prescription
            {
                Status = PrescriptionStatus.Added,
                UserId = model.UserId ?? Guid.Empty,
                User = user ?? new User
                {
                    MobileNumber = mobNum,
                    Password = HashGenerator.Hash(model.MobileNumber)
                },
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
            await _presRepo.AddAsync(prescription);
            var add = await _appUow.ElkSaveChangesAsync();
            return new Response<int>
            {
                IsSuccessful = add.IsSuccessful,
                Message = add.Message,
                Result = prescription.PrescriptionId
            };
        }
    }
}

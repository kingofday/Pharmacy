using Elk.Core;
using Microsoft.AspNetCore.Http;
using Pharmacy.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IAttachmentService
    {
        Task<IResponse<BaseAttachment>> Save(AttachmentType type, IFormFile File, string appDir);
        Task<IResponse<List<BaseAttachment>>> Save(AttachmentType type, List<IFormFile> Files, string appDir);
    }
}
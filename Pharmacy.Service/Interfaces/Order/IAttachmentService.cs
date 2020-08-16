using Elk.Core;
using Microsoft.AspNetCore.Http;
using Pharmacy.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IAttachmentService
    {
        Task<IResponse<List<BaseAttachment>>> Save(AttachmentType type, List<IFormFile> Files, string appDir);
    }
}
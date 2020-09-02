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
        Task<IResponse<List<BaseAttachment>>> Save(AttachmentType type, IList<IFormFile> Files, string appDir);
        IResponse<string> DeleteRange(string appDir, List<string> pathes);
        IResponse<string> DeleteRange(string appDir, List<BaseAttachment> attchs);

        IResponse<string> Delete(string appDir, string path);
    }
}
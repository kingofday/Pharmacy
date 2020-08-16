using Elk.Core;
using Microsoft.AspNetCore.Http;
using Pharmacy.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public class AttachmentService : IAttachmentService
    {
        public async Task<IResponse<List<BaseAttachment>>> Save(AttachmentType type, List<IFormFile> Files, string appDir)
        {
            var baseRelativeRoot = $"/Files/{type}/";
            var result = new List<BaseAttachment>();
            string name, url;
            var pdt = PersianDateTime.Now;
            foreach (var file in Files)
            {
                name = Guid.NewGuid().ToString().Replace("-", "_");
                url = $"{baseRelativeRoot}/{pdt.Year}/{pdt.Month}/{name}{Path.GetExtension(file.Name)}";
                var attch = new BaseAttachment
                {
                    AttachmentType = type,
                    FileType = FileOperation.GetFileType(file.Name),
                    FileName = name,
                    OriginalName = file.Name,
                    Extention = Path.GetExtension(file.Name),
                    Url = url,
                    PhysicalPath = Path.Combine(appDir, url.Replace("/", "\\")),
                    Size = file.Length
                };
                using var fileStream = new FileStream(attch.PhysicalPath, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
            return new Response<List<BaseAttachment>>
            {
                Result = result,
                IsSuccessful = true
            };
        }
    }
}

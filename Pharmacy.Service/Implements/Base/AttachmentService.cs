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
        public async Task<IResponse<BaseAttachment>> Save(AttachmentType type, IFormFile File, string appDir)
        {
            var baseRelativeRoot = $"/Files/{type}";
            string name, url;
            var pdt = PersianDateTime.Now;
            name = Guid.NewGuid().ToString().Replace("-", "_") + Path.GetExtension(File.FileName);
            url = $"{baseRelativeRoot}/{pdt.Year}/{pdt.Month}";
            FileOperation.CreateDirectory($"{appDir}{url}".Replace("/", "\\"));
            var attch = new BaseAttachment
            {
                AttachmentType = type,
                FileType = FileOperation.GetFileType(File.FileName),
                FileName = name,
                OriginalName = File.FileName,
                Extention = Path.GetExtension(File.FileName),
                Url = $"{url}/{name}",
                PhysicalPath = $"{appDir}{url.Replace("/", "\\")}\\{name}",
                Size = File.Length
            };
            using var fileStream = new FileStream(attch.PhysicalPath, FileMode.Create);
            await File.CopyToAsync(fileStream);
            return new Response<BaseAttachment>
            {
                Result = attch,
                IsSuccessful = true
            };
        }
        public async Task<IResponse<List<BaseAttachment>>> Save(AttachmentType type, List<IFormFile> Files, string appDir)
        {
            var baseRelativeRoot = $"/Files/{type}";
            var result = new List<BaseAttachment>();
            string name, url;
            var pdt = PersianDateTime.Now;
            foreach (var file in Files)
            {
                name = Guid.NewGuid().ToString().Replace("-", "_")+ Path.GetExtension(file.FileName);
                url = $"{baseRelativeRoot}/{pdt.Year}/{pdt.Month}";
                FileOperation.CreateDirectory($"{appDir}{url}".Replace("/", "\\"));
                var attch = new BaseAttachment
                {
                    AttachmentType = type,
                    FileType = FileOperation.GetFileType(file.FileName),
                    FileName = name,
                    OriginalName = file.FileName,
                    Extention = Path.GetExtension(file.FileName),
                    Url = $"{url}/{name}",
                    PhysicalPath = $"{appDir}{url.Replace("/", "\\")}\\{name}",
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

using Elk.Core;
using Microsoft.AspNetCore.Http;
using Pharmacy.DataAccess.Ef;
using Pharmacy.Domain;
using Pharmacy.Service.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public class AttachmentService : IAttachmentService
    {
        readonly AppDbContext _app;
        public AttachmentService(AppDbContext app)
        {
            _app = app;
        }
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
        public async Task<IResponse<List<BaseAttachment>>> Save(AttachmentType type, IList<IFormFile> Files, string appDir)
        {
            var baseRelativeRoot = $"/Files/{type}";
            var result = new List<BaseAttachment>();
            string name, url;
            var pdt = PersianDateTime.Now;
            foreach (var file in Files)
            {
                name = Guid.NewGuid().ToString().Replace("-", "_") + Path.GetExtension(file.FileName);
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

        public IResponse<string> DeleteRange(string appDir, List<string> pathes)
        {
            try
            {
                foreach (var path in pathes)
                {
                    if (File.Exists(appDir + path))
                        File.Delete(appDir + path);
                }
                return new Response<string> { IsSuccessful = true };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<string> { Message = ServiceMessage.Error };
            }

        }

        public IResponse<string> DeleteRange(string appDir, List<BaseAttachment> attchs)
        {
            try
            {
                foreach (var path in attchs.Select(x=>x.Url).ToList())
                {
                    if (File.Exists(appDir + path))
                        File.Delete(appDir + path);
                }
                return new Response<string> { IsSuccessful = true };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<string> { Message = ServiceMessage.Error };
            }

        }

        public IResponse<string> Delete(string appDir, string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
                return new Response<string> { IsSuccessful = true };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<string> { Message = ServiceMessage.Error };
            }

        }
    }
}

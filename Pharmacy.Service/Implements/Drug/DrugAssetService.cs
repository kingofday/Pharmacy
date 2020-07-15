using Elk.Core;
using Microsoft.AspNetCore.Http;
using Pharmacy.DataAccess.Ef;
using Pharmacy.Domain;
using Pharmacy.Service.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public class DrugAssetService : IDrugAssetService
    {
        readonly AppUnitOfWork _appUOW;
        readonly IGenericRepo<DrugAsset> _DrugAssetRepo;
        public DrugAssetService(AppUnitOfWork appUOW)
        {
            _DrugAssetRepo = appUOW.DrugAssetRepo;
            _appUOW = appUOW;
        }
        public async Task<IResponse<IList<DrugAsset>>> SaveRange(DrugAddModel model)
        {
            try
            {
                var items = new List<DrugAsset>();
                var pdt = PersianDateTime.Now;
                var dir = $"/Files/{model.DrugId}/{pdt.Year}/{pdt.Month}";
                if (!FileOperation.CreateDirectory(model.Root + dir))
                    return new Response<IList<DrugAsset>> { Message = ServiceMessage.SaveFileFailed };
                foreach (var file in model.Files)
                {
                    var name = Guid.NewGuid().ToString().Replace("-", "_") + Path.GetExtension(file.FileName);
                    var relativePath = $"{dir}/{name}";
                    var physicalPath = (model.Root + relativePath).Replace("/", "\\");
                    items.Add(new DrugAsset
                    {
                        FileName =name,
                        OriginalName = file.FileName,
                        Extention = Path.GetExtension(file.FileName),
                        FileType = FileOperation.GetFileType(file.FileName),
                        Url = model.BaseDomain + relativePath,
                        PhysicalPath = physicalPath
                    });
                    using (var stream = File.Create(physicalPath))
                        await file.CopyToAsync(stream);
                }

                return new Response<IList<DrugAsset>> { IsSuccessful = true, Result = items };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<IList<DrugAsset>>
                {
                    Message = ServiceMessage.SaveFileFailed
                };
            }

        }

        public IResponse<string> DeleteRange(IList<DrugAsset> assets)
        {
            try
            {
                foreach (var asset in assets)
                {
                    if (File.Exists(asset.PhysicalPath))
                        File.Delete(asset.PhysicalPath);
                }
                return new Response<string> { IsSuccessful = true };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<string> { Message = ServiceMessage.Error };
            }

        }

        public IResponse<string> DeleteRange(int drugId)
        {
            try
            {
                foreach (var asset in _DrugAssetRepo.Get(conditions: x => x.DrugId == drugId, null))
                {
                    if (File.Exists(asset.PhysicalPath))
                        File.Delete(asset.PhysicalPath);
                }
                return new Response<string> { IsSuccessful = true };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<string> { Message = ServiceMessage.Error };
            }

        }

        public async Task<IResponse<string>> DeleteAsync(int id)
        {
            try
            {
                var asset = await _DrugAssetRepo.FindAsync(id);
                if (asset == null)
                    return new Response<string> { Message = ServiceMessage.RecordNotExist };
                _DrugAssetRepo.Delete(asset);
                var delete = await _appUOW.ElkSaveChangesAsync();
                if (File.Exists(asset.PhysicalPath))
                    File.Delete(asset.PhysicalPath);
                return new Response<string> { IsSuccessful = delete.IsSuccessful, Message = delete.Message };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<string> { Message = ServiceMessage.Error };
            }

        }

        public IResponse<string> DeleteFiles(string baseDomain, IEnumerable<(string fileUrl, string physicalUrl)> urls)
        {
            try
            {
                if (urls == null) return new Response<string> { IsSuccessful = true };
                foreach (var url in urls)
                    if (File.Exists(url.physicalUrl))
                        File.Delete(url.physicalUrl);
                return new Response<string> { IsSuccessful = true };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<string>
                {
                    Message = ServiceMessage.Error
                };
            }

        }
    }
}

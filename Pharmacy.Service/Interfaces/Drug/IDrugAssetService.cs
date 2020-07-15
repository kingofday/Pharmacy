using Elk.Core;
using Microsoft.AspNetCore.Http;
using Pharmacy.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IDrugAssetService
    {
        Task<IResponse<string>> DeleteAsync(int id);
        IResponse<string> DeleteRange(IList<DrugAsset> assets);
        IResponse<string> DeleteRange(int productId);
        Task<IResponse<IList<DrugAsset>>> SaveRange(DrugAddModel model);
        IResponse<string> DeleteFiles(string baseDomain, IEnumerable<(string fileUrl, string physicalUrl)> urls);
    }
}
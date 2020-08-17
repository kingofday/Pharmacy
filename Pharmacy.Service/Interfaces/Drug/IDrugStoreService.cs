using System;
using Elk.Core;
using Pharmacy.Domain;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public interface IDrugStoreService
    {
        //Task<IResponse<LocationDTO>> GetLocationAsync(int id);
        //Task<IResponse<DrugStoreDTO>> FindAsDtoAsync(int id);
        IResponse<DrugStoreModel> GetNearest(LocationDTO model);
        PagingListDetails<DrugStore> Get(DrugStoreSearchFilter filter);
        Task<IResponse<DrugStore>> FindAsync(int id);
        Task<IResponse<bool>> DeleteAsync(int id);
        Task<IResponse<DrugStore>> SignUp(DrugStoreSignUpModel model);
        IEnumerable<DrugStore> GetAll(Guid userId);
        IDictionary<object, object> Search(string searchParameter, Guid? userId, int take = 10);
        Task<IResponse<DrugStore>> UpdateAsync(DrugStoreUpdateModel model);
        Task<IResponse<DrugStore>> UpdateAsync(DrugStoreAdminUpdateModel model);
        Task<IResponse<bool>> DeleteFile(string baseDomain, string root, int id);
        Task<bool> CheckOwner(int PharmacyId, Guid userId);
        List<DrugStoreDTO> GetAsDTO();
    }
}

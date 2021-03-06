﻿using System;
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
        IResponse<DrugStoreModel> GetNearest(LocationDTO model, List<int> excludedStores = null);
        PagingListDetails<DrugStore> Get(DrugStoreSearchFilter filter);
        Task<IResponse<DrugStore>> FindAsync(int id);
        Task<IResponse<bool>> DeleteAsync(int id, string appDir);
        //Task<IResponse<DrugStore>> SignUp(DrugStoreSignUpModel model);
        IEnumerable<DrugStore> GetAll(Guid userId);
        List<DrugStoreDTO> GetAsDTO(string baseUrl);
        IDictionary<object, object> Search(string searchParameter, Guid? userId, int take = 10);
        Task<IResponse<DrugStore>> AddAsync(DrugStoreAdminModel model);
        Task<IResponse<DrugStore>> UpdateAsync(DrugStoreUpdateModel model);
        Task<IResponse<DrugStore>> UpdateAsync(DrugStoreAdminModel model);
        Task<IResponse<string>> DeleteFile(string appDir, int attchId);
        Task<bool> CheckOwner(int PharmacyId, Guid userId);
        //List<DrugStoreDTO> GetAsDTO();
    }
}

using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Pharmacy.Service.Resource;
using DomainStrings = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Service
{
    public class UnitService : IUnitService
    {
        readonly AppUnitOfWork _appUow;
        readonly IGenericRepo<Unit> _unitRepo;
        public UnitService(AppUnitOfWork appUOW)
        {
            _appUow = appUOW;
            _unitRepo = appUOW.UnitRepo;
        }

        public async Task<IResponse<Unit>> FindAsync(int id)
        {
            var item = await _unitRepo.FindAsync(id);
            return new Response<Unit> { IsSuccessful = item != null, Message = (item == null ? ServiceMessage.RecordNotExist : null), Result = item };
        }

        public async Task<IResponse<Unit>> AddAsync(Unit model)
        {
            await _unitRepo.AddAsync(model);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<Unit> { Result = model, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<Unit>> UpdateAsync(Unit model)
        {
            var findedUnit = await _unitRepo.FindAsync(model.UnitId);
            if (findedUnit == null) return new Response<Unit> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.Unit) };

            findedUnit.Name = model.Name;

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<Unit> { Result = findedUnit, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int UnitId)
        {
            _unitRepo.Delete(new Unit { UnitId = UnitId });
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public PagingListDetails<Unit> Get(UnitSearchFilter filter)
        {
            Expression<Func<Unit, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    conditions = conditions.And(x => x.Name.Contains(filter.Name));
            }

            return _unitRepo.GetPaging(new PagingQueryFilter<Unit> { Conditions = conditions, PagingParameter = filter, OrderBy = x => x.OrderByDescending(i => i.UnitId) });
        }
    }
}

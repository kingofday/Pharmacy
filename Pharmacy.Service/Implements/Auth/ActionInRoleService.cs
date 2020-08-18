using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using Pharmacy.Service.Resource;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public class ActionInRoleService : IActionInRoleService
    {
        private readonly AuthUnitOfWork _authUow;

        public ActionInRoleService(AuthUnitOfWork uow)
        {
            _authUow = uow;
        }


        public async Task<IResponse<ActionInRole>> AddAsync(ActionInRole model)
        {
            if (await _authUow.ActionInRoleRepo.AnyAsync(new BaseFilterModel<ActionInRole> { Conditions = x => x.RoleId == model.RoleId && x.ActionId == model.ActionId }))
                return new Response<ActionInRole> { Message = ServiceMessage.DuplicateRecord, IsSuccessful = false };

            if (model.IsDefault)
            {
                var existActionInRole = await _authUow.ActionInRoleRepo.FirstOrDefaultAsync(new BaseFilterModel<ActionInRole> { Conditions = x => x.RoleId == model.RoleId && x.IsDefault });
                if (existActionInRole != null)
                    existActionInRole.IsDefault = false;
            }

            await _authUow.ActionInRoleRepo.AddAsync(model);
            var saveResult = await _authUow.ElkSaveChangesAsync();
            return new Response<ActionInRole>
            {
                Result = model,
                Message = saveResult.Message,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<bool>> DeleteAsync(int id)
        {
            _authUow.ActionInRoleRepo.Delete(new ActionInRole { ActionInRoleId = id });
            var saveResult = await _authUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public IEnumerable<ActionInRole> GetRoles(int actionId) =>
                _authUow.ActionInRoleRepo.Get(new BaseListFilterModel<ActionInRole>
                {
                    Conditions = x => x.ActionId == actionId,
                    OrderBy = x => x.OrderByDescending(air => air.ActionId),
                    IncludeProperties = new List<Expression<Func<ActionInRole, object>>> { x => x.Role }
                }).ToList();

        public IEnumerable<ActionInRole> GetActions(int roleId) =>
                    _authUow.ActionInRoleRepo.Get(new BaseListFilterModel<ActionInRole>
                    {
                        Conditions = x=> x.RoleId == roleId,
                        OrderBy = x => x.OrderByDescending(air => air.ActionId),
                        IncludeProperties = new List<Expression<Func<ActionInRole, object>>> { x => x.Action }
                    }).ToList();
    }
}

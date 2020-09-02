using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using Pharmacy.Service.Resource;
using System.Linq.Expressions;
using System.Collections.Generic;
using Action = Pharmacy.Domain.Action;
using DomainStrings = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Service
{
    public class ActionService : IActionService
    {
        private readonly AuthUnitOfWork _authUow;

        public ActionService(AuthUnitOfWork uow)
        {
            _authUow = uow;
        }


        public async Task<IResponse<Action>> AddAsync(Action model)
        {
            await _authUow.ActionRepo.AddAsync(model);

            var saveResult = _authUow.ElkSaveChangesAsync();
            return new Response<Action> { Result = model, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<Action>> UpdateAsync(Action model)
        {
            var findedAction = await _authUow.ActionRepo.FindAsync(model.ActionId);
            if (findedAction == null) return new Response<Action> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.Action) };

            findedAction.Name = model.Name;
            findedAction.Icon = model.Icon;
            findedAction.ParentId = model.ParentId;
            findedAction.ShowInMenu = model.ShowInMenu;
            findedAction.ControllerName = model.ControllerName;
            findedAction.ActionName = model.ActionName;
            findedAction.OrderPriority = model.OrderPriority;

            var saveResult = _authUow.ElkSaveChangesAsync();
            return new Response<Action> { Result = findedAction, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int actionId)
        {
            _authUow.ActionRepo.Delete(new Action { ActionId = actionId });
            var saveResult = await _authUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<Action>> FindAsync(int actionId)
        {
            var findedAction = await _authUow.ActionRepo.FirstOrDefaultAsync(new BaseFilterModel<Domain.Action>
            {
                Conditions = x => x.ActionId == actionId,
                IncludeProperties = new List<Expression<Func<Domain.Action, object>>> { i => i.Parent }
            });
            if (findedAction == null) return new Response<Action> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.Action) };
            return new Response<Action> { Result = findedAction, IsSuccessful = true };
        }

        public IDictionary<object, object> Get(bool justParents = false)
            => _authUow.ActionRepo.Get(new BaseListFilterModel<Domain.Action>
            {
                Conditions = x => !justParents || (x.ControllerName == null && x.ActionName == null),
                OrderBy = x => x.OrderByDescending(a => a.ActionId)
            })
             .ToDictionary(k => (object)k.ActionId, v => (object)$"{v.Name}({v.ControllerName}/{v.ActionName})");

        public PagingListDetails<Action> Get(ActionSearchFilter filter)
        {
            Expression<Func<Action, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.NameF))
                    conditions = conditions.And(x => x.Name.Contains(filter.NameF));
                if (!string.IsNullOrWhiteSpace(filter.ActionNameF))
                    conditions = conditions.And(x => x.ActionName.Contains(filter.ActionNameF.ToLower()));
                if (!string.IsNullOrWhiteSpace(filter.ControllerNameF))
                    conditions = conditions.And(x => x.ControllerName.Contains(filter.ControllerNameF.ToLower()));
            }

            return _authUow.ActionRepo.Get(new BasePagedListFilterModel<Domain.Action>
            {
                Conditions = conditions,
                PagingParameter = filter,
                OrderBy = x => x.OrderByDescending(u => u.ActionId),
                IncludeProperties = new List<Expression<Func<Domain.Action, object>>> { i => i.Parent }
            });
        }

        public IDictionary<object, object> Search(string searchParameter, int take = 10)
            => _authUow.ActionRepo.Get(new BaseListFilterModel<Domain.Action>
            {
                Conditions = x => x.Name.Contains(searchParameter) || x.ControllerName.Contains(searchParameter) || x.ActionName.Contains(searchParameter),
                OrderBy = o => o.OrderByDescending(x => x.ActionId)
            }
            )
            .Take(take)
            .ToDictionary(k => (object)k.ActionId, v => (object)$"{v.Name}({(string.IsNullOrWhiteSpace(v.ControllerName) ? "" : v.ControllerName + "/" + v.ActionName)})");
    }
}


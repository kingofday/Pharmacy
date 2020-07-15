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
    public class UserInRoleService : IUserInRoleService
    {
        private readonly AuthUnitOfWork _authUow;
        IGenericRepo<UserInRole> _userInRoleRepo;
        public UserInRoleService(AuthUnitOfWork uow,IGenericRepo<UserInRole> userInRoleRepo)
        {
            _authUow = uow;
            _userInRoleRepo = userInRoleRepo;
        }


        public async Task<IResponse<UserInRole>> Add(UserInRole model)
        {
            
            if (await _userInRoleRepo.AnyAsync(x => x.UserId == model.UserId && x.RoleId == model.RoleId))
                return new Response<UserInRole> { Message = ServiceMessage.DuplicateRecord, IsSuccessful = false };

            await _userInRoleRepo.AddAsync(model);
            var saveResult = await _authUow.ElkSaveChangesAsync();
            return new Response<UserInRole>
            {
                Result = model,
                Message = saveResult.Message,
                IsSuccessful = saveResult.IsSuccessful
            };
        }

        public async Task<IResponse<bool>> Delete(int userInRoleId)
        {
            _userInRoleRepo.Delete(new UserInRole { UserInRoleId = userInRoleId });
            var saveResult = await _authUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public IEnumerable<UserInRole> Get(Guid userId)
            => _userInRoleRepo.Get(x => x.UserId == userId,
            x => x.OrderByDescending(uir => uir.UserId),
            new List<Expression<Func<UserInRole, object>>> { x => x.Role }).ToList();
    }
}

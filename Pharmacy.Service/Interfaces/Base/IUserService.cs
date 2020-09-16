using System;
using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public interface IUserService : IScopedInjection
    {
        Task<IResponse<User>> AddAsync(User model);
        Task<IResponse<User>> UpdateAsync(User model);
        Task<IResponse<User>> UpdateProfile(User model);
        Task<IResponse<bool>> DeleteAsync(Guid userId);
        Task<IResponse<User>> FindAsync(Guid userId);
        Task<IResponse<User>> FindByMobileNumber(long mobileNumber);
        Task<MenuModel> GetAvailableActions(Guid userId, List<MenuSPModel> spResult = null, string urlPrefix = "");
        Task<IResponse<User>> Authenticate(long mobileNumber, string password);
        void SignOut(Guid userId);
        PagingListDetails<User> Get(UserSearchFilter filter);
        IDictionary<object, object> Search(string query, int take = 10);
        Task<IResponse<string>> RecoverPassword(long mobileNumber, string from, EmailMessage model);
        //=======================================================================
        //-- api
        //=======================================================================
        Task<IResponse<User>> SignUp(SignUpModel model, int defaultRoleId);
        Task<IResponse<AuthResponse>> SignIn(long username, string password);
        Task<Response<AuthResponse>> Confirm(long mobileNumber, int code);
        Task<Response<bool>> Resend(long mobileNumber);
        Task<Response<User>> UpdateProfile(Guid id, UpdateProfileModel model);
    }
}
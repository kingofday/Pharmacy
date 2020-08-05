using System;
using Elk.Core;
using Elk.Cache;
using System.Text;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Pharmacy.InfraStructure;
using Pharmacy.Service.Resource;
using Pharmacy.DataAccess.Dapper;
using System.Collections.Generic;
using DomainStrings = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Service
{
    public class UserService : IUserService, IUserActionProvider
    {
        private readonly AppUnitOfWork _appUow;
        private readonly IEmailService _emailService;
        private readonly IMemoryCacheProvider _cache;
        private readonly DashboardMenuSp _dashboardMenuSp;
        readonly IUserRepo _userRepo;
        readonly AuthUnitOfWork _authUow;
        public UserService(AppUnitOfWork appUow, AuthUnitOfWork authUow, IMemoryCacheProvider cache,
            IEmailService emailService, DashboardMenuSp dashboardMenuSp)
        {
            _appUow = appUow;
            _cache = cache;
            _emailService = emailService;
            _dashboardMenuSp = dashboardMenuSp;
            _userRepo = appUow.UserRepo;
            authUow = _authUow;
        }


        #region CRUD

        public async Task<IResponse<User>> AddAsync(User model)
        {
            var user = await _userRepo.FirstOrDefaultAsync(conditions: x => x.MobileNumber == model.MobileNumber);
            if (user != null) return new Response<User>
            {
                IsSuccessful = true,
                Result = user
            };
            model.UserId = Guid.NewGuid();
            model.Password = HashGenerator.Hash(model.NewPassword);
            await _userRepo.AddAsync(model);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<User> { Result = model, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<User>> UpdateProfile(User model)
        {
            var user = await _userRepo.FindAsync(model.UserId);
            if (user == null) return new Response<User> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.User) };
            if (await _userRepo.AnyAsync(x => x.MobileNumber == model.MobileNumber && x.UserId != model.UserId))
                return new Response<User> { Message = ServiceMessage.DuplicateRecord };
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
                user.Password = HashGenerator.Hash(model.NewPassword);
            user.FullName = model.FullName;
            user.Email = model.Email;
            if (!string.IsNullOrWhiteSpace(user.NewPassword)) user.Password = HashGenerator.Hash(model.NewPassword);
            user.NewPassword = null;
            _userRepo.Update(user);
            var saveResult = _appUow.ElkSaveChangesAsync();
            return new Response<User> { Result = user, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<User>> UpdateAsync(User model)
        {
            var findedUser = await _userRepo.FindAsync(model.UserId);
            if (findedUser == null) return new Response<User> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.User) };

            if (model.MustChangePassword)
                findedUser.Password = HashGenerator.Hash(model.Password);

            findedUser.FullName = model.FullName;
            findedUser.IsActive = model.IsActive;

            var saveResult = _appUow.ElkSaveChangesAsync();
            return new Response<User> { Result = findedUser, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(Guid userId)
        {
            _userRepo.Delete(new User { UserId = userId });
            var Pharmacys = _appUow.DrugStoreRepo.Get(conditions: x => x.UserId == userId, orderBy: null);
            if (Pharmacys != null)
                foreach (var Pharmacy in Pharmacys) _appUow.DrugStoreRepo.Delete(Pharmacy);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<User>> FindAsync(Guid userId)
        {
            var findedUser = await _userRepo.FindAsync(userId);
            if (findedUser == null) return new Response<User> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.User) };

            return new Response<User> { Result = findedUser, IsSuccessful = true };
        }

        #endregion

        private string MenuModelCacheKey(Guid userId) => $"MenuModel_{userId.ToString().Replace("-", "_")}";

        public IEnumerable<UserAction> GetUserActions(string userId, string urlPrefix = "")
            => GetAvailableActions(Guid.Parse(userId), null, urlPrefix).ActionList;

        public Task<IEnumerable<UserAction>> GetUserActionsAsync(string userId, string urlPrefix = "")
            => (Task.Run(() => GetAvailableActions(Guid.Parse(userId), null, urlPrefix).ActionList));

        public async Task<IResponse<User>> FindByMobileNumber(long mobileNumber)
        {
            var user = await _userRepo.FirstOrDefaultAsync(conditions: x => x.MobileNumber == mobileNumber, includeProperties: null);
            return new Response<User>
            {
                IsSuccessful = user != null,
                Result = user,
                Message = user == null ? ServiceMessage.RecordNotExist : string.Empty
            };
        }

        public async Task<IResponse<User>> Authenticate(long mobileNumber, string password)
        {
            var user = await _userRepo.FirstOrDefaultAsync(conditions: x => x.MobileNumber == mobileNumber, includeProperties: null);
            if (user == null) return new Response<User> { Message = ServiceMessage.InvalidUsernameOrPassword };

            if (!user.IsActive) return new Response<User> { Message = ServiceMessage.AccountIsBlocked };

            var hashedPassword = HashGenerator.Hash(password);
            if (user.Password != hashedPassword)
            {
                FileLoger.Message($"UserService/Authenticate-> Invalid Password Login ! Username:{mobileNumber} Password:{password}");
                return new Response<User> { Message = ServiceMessage.InvalidUsernameOrPassword };
            }
            //if (user.NewPassword == hashedPassword)
            //{
            //    user.Password = user.NewPassword;
            //    user.NewPassword = null;
            //}
            user.LastLoginDateMi = DateTime.Now;
            user.LastLoginDateSh = PersianDateTime.Now.ToString(PersianDateTimeFormat.Date);
            _userRepo.Update(user);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<User> { IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message, Result = user };
        }

        private string GetAvailableMenu(List<MenuSPModel> spResult, string urlPrefix = "")
        {
            var sb = new StringBuilder(string.Empty);
            foreach (var item in spResult.Where(x => x.ShowInMenu && (x.IsAction || (!x.IsAction && x.ActionsList.Any(v => v.ShowInMenu)))).OrderBy(x => x.OrderPriority))
            {
                if (!item.IsAction && !item.HasChild) continue;
                #region Add Menu
                sb.AppendFormat("<li {0}><a href='{1}'><i class='{2} default-i'></i><span class='nav-label'>{3}</span> {4}</a>",
                            item.IsAction ? "" : "class='link-parent'",
                            item.IsAction ? $"{urlPrefix}/{item.ControllerName.ToLower()}/{item.ActionName.ToLower()}" : "#",
                            item.Icon,
                            item.Name,
                            item.IsAction ? "" : "<span class='fa arrow'></span>");
                #endregion

                if (!item.IsAction && item.HasChild)
                {
                    #region Add Sub Menu
                    sb.Append("<ul class='nav nav-second-level collapse'>");
                    foreach (var v in item.ActionsList.Where(x => x.ShowInMenu).OrderBy(x => x.OrderPriority))
                    {
                        sb.AppendFormat("<li><a href='{0}' >{1}</a><li>",
                        $"{urlPrefix}/{v.ControllerName.ToLower()}/{v.ActionName.ToLower()}",
                        v.Name);
                    }
                    sb.Append("</ul>");
                    #endregion
                }
                sb.Append("</li>");
            }
            return sb.ToString();
        }

        public MenuModel GetAvailableActions(Guid userId, List<MenuSPModel> spResult = null, string urlPrefix = "")
        {
            var userMenu = (MenuModel)_cache.Get(GlobalVariables.CacheSettings.MenuModelCacheKey(userId));
            if (userMenu != null) return userMenu;

            userMenu = new MenuModel();
            if (spResult == null) spResult = _dashboardMenuSp.GetUserMenu(userId).ToList();

            //#region Find Default View
            //foreach (var menuItem in spResult)
            //{
            //    if (menuItem.IsAction && menuItem.IsDefault)
            //    {
            //        userMenu.DefaultUserAction = new UserAction
            //        {
            //            Action = menuItem.ActionName,
            //            Controller = menuItem.ControllerName
            //        };
            //        break;
            //    }
            //    var actions = menuItem.ActionsList;
            //    if (actions.Any(x => x.IsDefault))
            //    {
            //        userMenu.DefaultUserAction = new UserAction
            //        {
            //            Action = actions.FirstOrDefault(x => x.IsDefault).ActionName,
            //            Controller = actions.FirstOrDefault(x => x.IsDefault).ControllerName
            //        };
            //        break;
            //    }
            //}
            //if (userMenu.DefaultUserAction == null || userMenu.DefaultUserAction.Controller == null) return null;
            //#endregion
            var userActions = new List<UserAction>();
            void AddAction(MenuSPModel item)
            {
                if (item.IsDefault) userMenu.DefaultUserAction = new UserAction
                {
                    Action = item.ActionName,
                    Controller = item.ControllerName
                };
                if (item.IsAction) userActions.Add(new UserAction
                {
                    Controller = item.ControllerName.ToLower(),
                    Action = item.ActionName.ToLower(),
                    RoleId = item.RoleId,
                    RoleNameFa = item.RoleNameFa
                });
                if (item.ActionsList != null)
                    foreach (var child in item.ActionsList) AddAction(child);
            }
            foreach (var item in spResult) AddAction(item);
            //foreach (var item in spResult)
            //{
            //    if (item.IsAction)
            //        userActions.Add(new UserAction
            //        {
            //            Controller = item.ControllerName.ToLower(),
            //            Action = item.ActionName.ToLower(),
            //            RoleId = item.RoleId,
            //            RoleNameFa = item.RoleNameFa
            //        });
            //    if (item.ActionsList != null)
            //        foreach (var child in item.ActionsList)
            //            userActions.Add(new UserAction
            //            {
            //                Controller = child.ControllerName.ToLower(),
            //                Action = child.ActionName.ToLower(),
            //                RoleId = child.RoleId,
            //                RoleNameFa = child.RoleNameFa
            //            });
            //}
            userActions = userActions.Distinct().ToList();
            userMenu.Menu = GetAvailableMenu(spResult, urlPrefix);
            userMenu.ActionList = userActions;

            _cache.Add(GlobalVariables.CacheSettings.MenuModelCacheKey(userId), userMenu, DateTime.Now.AddMinutes(30));
            return userMenu;
        }

        public void SignOut(Guid userId)
        {
            _cache.Remove(MenuModelCacheKey(userId));
        }

        public PagingListDetails<User> Get(UserSearchFilter filter)
        {
            Expression<Func<User, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.FullNameF))
                    conditions = conditions.And(x => x.FullName.Contains(filter.FullNameF));
                if (!string.IsNullOrWhiteSpace(filter.EmailF))
                    conditions = x => x.Email.Contains(filter.EmailF);
                if (!string.IsNullOrWhiteSpace(filter.MobileNumberF))
                    conditions = x => x.MobileNumber.ToString().Contains(filter.MobileNumberF);
            }

            var items = _userRepo.Get(conditions, filter, x => x.OrderByDescending(u => u.InsertDateMi));
            return items;
        }

        public IDictionary<object, object> Search(string searchParameter, int take = 10)
           => _userRepo.Get(conditions: x => x.FullName.Contains(searchParameter) || x.Email.Contains(searchParameter), pagingParameter: new PagingParameter
           {
               PageNumber = 1,
               PageSize = 10
           },
               selector: x => new
               {
                   x.UserId,
                   x.MobileNumber,
                   x.FullName
               },
               orderBy: o => o.OrderByDescending(x => x.InsertDateMi))
               .Items.ToDictionary(k => (object)k.UserId, v => (object)$"{v.FullName}({v.MobileNumber})");

        public async Task<IResponse<string>> RecoverPassword(long mobileNumber, string from, EmailMessage model)
        {
            var user = await _userRepo.FirstOrDefaultAsync(conditions: x => x.MobileNumber == mobileNumber, includeProperties: null);
            if (user == null) return new Response<string> { Message = ServiceMessage.RecordNotExist.Fill(DomainStrings.User) };

            user.MustChangePassword = true;
            var newPassword = Randomizer.GetUniqueKey(6);
            user.Password = HashGenerator.Hash(newPassword);
            _userRepo.Update(user);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            if (!saveResult.IsSuccessful) return new Response<string> { IsSuccessful = false, Message = saveResult.Message };

            model.Subject = ServiceMessage.RecoverPassword;
            model.Body = model.Body.Fill(newPassword);
            _emailService.Send(user.Email, new List<string> { from }, model);
            return new Response<string> { IsSuccessful = true, Message = saveResult.Message };
        }
        //=======================================================================
        //-- api
        //=======================================================================
        public async Task<IResponse<User>> SignUp(SignUpModel model, int defaultRoleId)
        {
            using var db = _appUow.Database.BeginTransaction();
            var mobNum = long.Parse(model.MobileNumber);
            var user = await _userRepo.FirstOrDefaultAsync(conditions: x => x.MobileNumber == mobNum);
            if (user != null)
                return new Response<User> { Message = ServiceMessage.AlreadySignedUp };
            user = new User
            {
                UserId = Guid.NewGuid(),
                MobileNumber = long.Parse(model.MobileNumber),
                Email = model.Email,
                Password = HashGenerator.Hash(model.NewPassword),
                FullName = model.Fullname,
                MobileConfirmCode = Randomizer.GetRandomInteger(4),
                LastLoginDateMi = DateTime.Now,
                LastLoginDateSh = PersianDateTime.Now.ToString(PersianDateTimeFormat.Date),
                UserStatus = UserStatus.Added
            };
            await _userRepo.AddAsync(user);
            var saveUser = await _appUow.ElkSaveChangesAsync();
            if (!saveUser.IsSuccessful)
                return new Response<User> { Message = saveUser.Message };
            await _authUow.UserInRoleRepo.AddAsync(new UserInRole
            {
                RoleId = defaultRoleId,
                UserId = user.UserId
            });
            var saveUIR = await _authUow.ElkSaveChangesAsync();
            return new Response<User> { Result = user, IsSuccessful = saveUIR.IsSuccessful, Message = saveUIR.Message };
        }
        public async Task<Response<AuthResponse>> Confirm(long mobileNumber, int code)
        {
            var user = await _userRepo.FirstOrDefaultAsync(conditions: x => x.MobileNumber == mobileNumber && x.MobileConfirmCode == code);
            if (user == null) return new Response<AuthResponse> { Message = ServiceMessage.WrongConfirmCode };
            user.IsConfirmed = true;
            user.MobileConfirmCode = null;
            _userRepo.Update(user);
            var update = await _appUow.ElkSaveChangesAsync();
            if (!update.IsSuccessful)
                return new Response<AuthResponse> { Message = update.Message };
            return new Response<AuthResponse>
            {
                Result = new AuthResponse
                {
                    Email = user.Email,
                    Fullname = user.Email,
                    MobileNumber = user.MobileNumber.ToString(),
                    IsConfirmed = false
                }
            };
        }
        public async Task<IResponse<AuthResponse>> LogIn(long username, string password)
        {
            var pw = HashGenerator.Hash(password);
            var user = await _userRepo.FirstOrDefaultAsync(conditions: x => x.MobileNumber == username && x.Password == pw);
            if (user == null) return new Response<AuthResponse> { Message = ServiceMessage.WrongUsernameOrPassword };
            if (user.IsConfirmed)
            {
                user.LastLoginDateSh = PersianDateTime.Now.ToString(PersianDateTimeFormat.Date);
                user.LastLoginDateMi = DateTime.Now;
                _userRepo.Update(user);
                await _appUow.ElkSaveChangesAsync();
            }
            return new Response<AuthResponse>
            {
                IsSuccessful = true,
                Result = new AuthResponse
                {
                    Email = user.Email,
                    Fullname = user.Email,
                    MobileNumber = user.MobileNumber.ToString(),
                    IsConfirmed = true
                }
            };
        }
    }
}

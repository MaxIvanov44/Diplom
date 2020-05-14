using Microsoft.AspNetCore.Identity;
using StudentAccounting.Data.EntityModels;
using StudentAccounting.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Logic.Services
{
    public interface IUserService
    {
        Task<UsersEntityModel> GetUserAsync(string name);
        Task<IdentityResult> CreateUserAsync(RegisterViewModel request);
        Task<SignInResult> LogInAsync(LogInViewModel request);
        void LogOutAsync();
        Task<IdentityResult> CreateRole(string name);
    }
}

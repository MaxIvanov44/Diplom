using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StudentAccounting.Common.Enums;
using StudentAccounting.Common.Models;
using StudentAccounting.Data.EntityModels;
using StudentAccounting.Data.Repositories;
using StudentAccounting.Logic.Models;
using StudentAccounting.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Logic.Services.Implementations
{
    public class UsersService : IUserService
    {
        private readonly UserManager<UsersEntityModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<UsersEntityModel> _signInManager;
        public UsersService(UserManager<UsersEntityModel> userManager, RoleManager<IdentityRole> roleManager, SignInManager<UsersEntityModel> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<UsersEntityModel> GetUserAsync(string name)
        {
            var currentUser = await _userManager.FindByNameAsync(name);
            return currentUser;
        }
        public async Task<IdentityResult> CreateUserAsync(RegisterViewModel request)
        {
            var newUser = new UsersEntityModel
            {
                UserName = request.Email,
                FirstName = request.FirstName,
                SecondName = request.SecondName,
                Patronymic = request.Patronymic,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (result.Succeeded)
                return await _userManager.AddToRoleAsync(newUser, request.RoleName);
            else return null;
        }
        public async Task<SignInResult> LogInAsync(LogInViewModel request)
        {
            return await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, false);
        }

        public async void LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<IdentityResult> CreateRole(string name)
        {
            return await _roleManager.CreateAsync(new IdentityRole(name));
        }


    }
}

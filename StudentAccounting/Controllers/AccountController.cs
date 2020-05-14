using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using StudentAccounting.Common.Models;
using StudentAccounting.Logic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using StudentAccounting.Common.Enums;
using StudentAccounting.Logic.ViewModels;
using StudentAccounting.Logic.Services.Implementations;
using Microsoft.Extensions.Logging;

namespace StudentAccounting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _UsersService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IUserService UsersService, ILogger<AccountController> logger)
        {
            _UsersService = UsersService;
            _logger = logger;
        }

        /// <summary>
        /// Авторизации пользователя
        /// </summary>
        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromForm]LogInViewModel model)
        {
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    var result = await _UsersService.LogInAsync(model);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"Пользователь: {model.Email}, успешно авторизован");
                        return Ok("Успешная авторизация");
                    }
                }

                return BadRequest("Ошибка авторизации");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при авторизации");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromForm]RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid && model != null)
                {
                    var result = await _UsersService.CreateUserAsync(model);
                    if (result.Succeeded)
                        return Ok("Пользователь успешно зарегестрирован");
                }
                return BadRequest("Входе регистрации произошла ошибка");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Выход из аккаунта текущего пользователя
        /// </summary>
        [HttpPost]
        [Route("LogOut")]
        public IActionResult LogOut()
        {
            try
            {
                _UsersService.LogOutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выходе из аккаунта");
                return BadRequest(ex);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAccounting.Logic.Models;
using StudentAccounting.Logic.Services;

namespace StudentAccounting.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionController : ControllerBase
    {
        private readonly ILogger<InstitutionController> _logger;
        private readonly IInstitutionService _institutionService;
        public InstitutionController(IInstitutionService institutionService, ILogger<InstitutionController> logger)
        {
            _institutionService = institutionService;
            _logger = logger;
        }

        /// <summary>
        /// Добавление нового учебного заведения в базу
        /// </summary>
        /// <param name="request">Принимает модель учебного заведения</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager")]
        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateInstitution(string name)
        {
            try
            {
                var result = _institutionService.CreateInstitution(name);
                if (result == null)
                {
                    return BadRequest("Неудалось добавить учебное заведение");
                }
                _logger.LogInformation($"Учебное заведение: {name}, успешно добавлено");
                return Ok($"Учебное заведение {name} успешно добавлено");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка входе добавление учебного заведения");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Удаление учебного заведения
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager")]
        [HttpDelete]
        [Route("[action]")]
        public IActionResult DeleteInstitution(Guid id)
        {
            try
            {
                var result = _institutionService.DeleteInstitution(id);
                if (result == null)
                {
                    return BadRequest("Ошибка при удалении");
                }
                _logger.LogInformation($"Учебное заведение: {id}, успешно удаленно");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка входе удаления учебного заведения");
                return BadRequest(ex);

            }
        }

        /// <summary>
        /// Обновление информации о учебном заведении
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager")]
        [HttpPut]
        [Route("[action]")]
        public IActionResult UpdateInstitution([FromForm]InstitutionModel model)
        {
            try
            {
                var result = _institutionService.UpdateInstitution(model);
                if (result == null)
                {
                    return BadRequest("Ошибка при сохранении данных");
                }
                _logger.LogInformation($"Данные о учебном заведении: {model.Name}, успешно обновлены");
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка входе обновления данных о учебном заведении");
                return BadRequest(ex);
            }
        }


    }
}
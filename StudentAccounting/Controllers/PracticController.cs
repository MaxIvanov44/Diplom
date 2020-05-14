

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
using StudentAccounting.Logic.Services.Implementations;

namespace StudentAccounting.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PracticController : ControllerBase
    {
        private readonly ILogger<PracticController> _logger;
        private readonly IPracticService _practicService;

        public PracticController(IPracticService practicService, ILogger<PracticController> logger)
        {
            _practicService = practicService;
            _logger = logger;
        }

        /// <summary>
        /// Обновление отчета по практике студента
        /// </summary>
        /// <param name="practic">Модель отчета по практике</param>
        /// <returns></returns>
        [Authorize(Roles = "Mentor")]
        [HttpPut("[action]")]
        public IActionResult UpdatePractic([FromForm]PracticModel practic)
        {
            try
            {
                var result = _practicService.UpdatePractic(practic);
                if (result == null)
                {
                    return BadRequest("При обновление данных произошла ошибка");
                }

                _logger.LogInformation($"Данные о прохождении практики успешно обновленны: {practic.Id}");
                return Ok("Данные успешно обновлены");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении данных о прохождении практики");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Удаление отчета по практике студента
        /// </summary>
        /// <param name="id">Guid отчета по практике</param>
        /// <returns></returns>
        [Authorize(Roles = "Mentor")]
        [HttpDelete("[action]")]
        public IActionResult DeletePractic(Guid id)
        {
            try
            {
                var result = _practicService.DeletePractic(id);
                if (result == null)
                {
                    return BadRequest("Ошибка при удалении практики");
                }
                _logger.LogInformation($"Данные о прохождении практики успешно удаленны: {result}");
                return Ok("Данные практики успешно удаленны");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении данных о прохождении практики");
                return BadRequest(ex);
            }
        }

        [Authorize(Roles = "HR-Manager,Mentor")]
        [HttpPost("[action]")]
        public IActionResult ExportToExcel(Guid guid)
        {
            try
            {
                _logger.LogInformation($"Экспорт данных о практике: {guid}, в Excel");
                return RedirectToAction("PracticToExcel", "Reports", new { id = guid });
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при экспорте данных о практике в Excel");
                return BadRequest(ex);
            }
        }
    }
}
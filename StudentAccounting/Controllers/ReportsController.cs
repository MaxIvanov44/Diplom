using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentAccounting.Common.Models;
using StudentAccounting.Logic.Models;
using StudentAccounting.Logic.Services;
using StudentAccounting.Logic.Services.ReportGeneration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;


namespace StudentAccounting.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : Controller
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly IReportService _reportService;
        //protected static DataTable currentReport;
        public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        /// <summary>
        /// Экспорт информации о практике студента в Excel
        /// </summary>
        /// <param name="id">Принимает Guid практики студента</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager,Director")]
        [HttpPost]
        [Route("[action]")]
        public IActionResult PracticToExcel(Guid id)
        {
            try
            {
                _reportService.PracticReport(id);
                _logger.LogInformation($"Экспорт информации о практике {id} в Excel");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при экспорте данных о практике в Excel");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Экспорт информации о студенте в Excel
        /// </summary>
        /// <param name="id">Принимает Guid студента</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager")]
        [HttpPost]
        [Route("StudentCard")]
        public IActionResult StudentCard(Guid id)
        {
            try
            {
                _reportService.StudentCard(id);
                _logger.LogInformation($"Экспорт информации о студенте {id} в Excel");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при экспорте данных о студенте в Excel");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Создание отчёта по прохождению практики студентами
        /// </summary>
        /// <param name="model">Модель поиска</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager,Director")]
        [HttpPost]
        [Route("[action]")]
        public IActionResult StudentsReport(ReportSearchModel model)
        {
            try
            {
                var result = _reportService.StudentsReport(model);
                if (result == null)
                {
                    return BadRequest("Неудалось сформировать отчёт");
                }
                _logger.LogInformation("Успешенное создание отчёта по прохождению студентами практики");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании отчёта по прохождению студентами практики");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Экспорт отчёта по прохождению практики студентами в Excel
        /// </summary>
        /// <param name="currentReport">Отчёт по прохождению студентами практики</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager,Director")]
        [HttpGet]
        [Route("[action]")]
        public IActionResult ReportIntoExcel(DataTable currentReport)
        {
            try
            {
                var result = _reportService.CreateReport(currentReport);
                result = Path.GetFullPath(result);
                string fileType = "application/vnd.ms-excel";
                string fileName = "testXlsx.xlsx";
                _logger.LogInformation("Экспорт отчёта по прохождению практики студентами в Excel");
                return PhysicalFile(result, fileType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при экспорте отчёта по прохождению практики студентами в Excel");
                return BadRequest(ex);
            }
        }
    }
}



using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccounting.Common.Enums;
using StudentAccounting.Common.Models;
using StudentAccounting.Logic.Models;
using StudentAccounting.Logic.Services;
using StudentAccounting.Logic.ViewModels;
using StudentAccounting.Logic;
using StudentAccounting.Logic.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using MassTransit;
using StudentAccounting.Logic.EventBus;
using Microsoft.Extensions.Logging;

namespace StudentAccounting.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudService _studentService;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        private readonly IRequestClient<IEmailSend> _requestClient;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudService studentService, IUserService userService, IFileService fileService, IRequestClient<IEmailSend> requestClient, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _userService = userService;
            _fileService = fileService;
            _requestClient = requestClient;
            _logger = logger;
        }

        /// <summary>
        /// Просмотр карточки студента
        /// </summary>
        /// <param name="id">Guid студента</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager,Mentor,Director")]
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetStudentCard(Guid id)
        {
            try
            {
                var result = _studentService.GetStudentByGuid(id);
                if (result == null)
                {
                    return BadRequest();
                }
                if (User.IsInRole("Mentor"))
                {
                    var user = _userService.GetUserAsync(User.Identity.Name).Result;
                    if (user.Id != result.MentorId)
                    {
                        return BadRequest("Вы не курируете этого студента");
                    }
                    _logger.LogInformation($"Получение карточки студента: {result}");
                    return Ok(result);
                }
                _logger.LogInformation($"Получение карточки студента: {result}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении карточки студента с ID:{id}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Создание новой заявки на практику
        /// </summary>
        /// <param name="request">Модель студента</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateNewStudent([FromForm]NewStudentModel request, IFormFile photo)
        {
            try
            {
                var imageBytes = _fileService.ImageToBytes(photo);
                var result = _studentService.CreateNewStudent(request, imageBytes);
                if (result == null)
                {
                    return BadRequest("Такой студент уже существует");
                }
                _logger.LogInformation($"Успешное добавление студента в базу: {result}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при добавлении студента {request}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Добавление в базу нового студента вручную HR-менеджером
        /// </summary>
        /// <param name="model">Модель студента</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager")]
        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateStudent([FromForm] StudentViewModel model, IFormFile photo)
        {
            try
            {
                var imageBytes = _fileService.ImageToBytes(photo);
                var result = _studentService.CreateStudent(model, imageBytes);
                if (result == null)
                {
                    return BadRequest("Ошибка при сохранении данных");
                }
                _logger.LogInformation($"Успешное добавление студента в базу: {result}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при добавлении студента {model}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Обновление информации в карточке студента
        /// </summary>
        /// <param name="model">Модель студента с обновленной информацией</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager,Mentor")]
        [HttpPut("[action]")]
        public IActionResult UpdateInfo([FromBody]StudentsModel model)
        {
            try
            {
                var result = _studentService.UpdateStudInfo(model);
                if (result == null)
                {
                    return BadRequest("Студент находится в архиве");
                }
                _logger.LogInformation($"Успешное обновление данных о студенте {model}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении данных о студенте {model}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Изменение статуса студента
        /// </summary>
        /// <param name="student"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager")]
        [HttpPost("[action]")]
        public IActionResult ChangeStudentStatus([FromForm]StudentsModel student, string subject, string message)
        {
            try
            {
                var result = _studentService.ChangeStudentStatus(student, subject, message);
                if (result == null)
                {
                    return BadRequest();
                }
                _logger.LogInformation($"Успешное изменение статуса студенту {student}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при изменении статуса студенту {student}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Удаление карточки студента
        /// </summary>
        /// <param name="id">Guid студента подлежащего удалению</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager")]
        [HttpDelete("[action]")]
        public IActionResult DeleteStudent([FromForm]Guid id)
        {
            try
            {
                var result = _studentService.DeleteStud(id);
                if (result == null)
                {
                    return BadRequest("Произошла ошибка при удаление студента");
                }
                _logger.LogInformation($"Студент {result} успешно удалён");
                return Ok("Студент успешно удален");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении студента {id}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Поиск студентов по заданым критериям
        /// </summary>
        /// <param name="request">Поисковую модель</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager,Director")]
        [HttpPost]
        [Route("[action]")]
        public IActionResult FindStudents([FromForm]StudSearchModel request)
        {
            try
            {

                var result = _studentService.FindStudents(request);
                if (result == null)
                {
                    return BadRequest("Извините, по Вашему запросу ничего не найдено. Попробуйте изменить параметры поиска.");
                }
                _logger.LogInformation($"Успешный поиск студентов с фильтром: {request}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при поиске студентов с фильтром: {request}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Поиск студентов по статусу
        /// </summary>
        /// <param name="status">Значение статуса</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager,Director")]
        [HttpGet("[action]")]
        public IActionResult GetStudentsByStatus(Status status)
        {
            try
            {
                var result = _studentService.GetStudentsByStatus(status);
                if (result == null)
                {
                    return BadRequest("Студентов с таким статусом не найдено");
                }
                _logger.LogInformation($"Успешный поиск студентов по статусу: {status}");
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при поиске студентов по статусу: {status}");
                return BadRequest(ex);

            }
        }

        /// <summary>
        /// Создание отчета по прохождению практики студентом
        /// </summary>
        /// <param name="id">Принимает Guid студента</param>
        /// <returns></returns>
        [Authorize(Roles = "Mentor")]
        [HttpPut("[action]")]
        public IActionResult CreateStudentPractic(Guid id)
        {
            try
            {
                var result = _studentService.CreateReport(id);
                if (result == null)
                {
                    return BadRequest("Неудалось добавить студенту практику");
                }
                _logger.LogInformation($"Успешный создание отчёта по прохождению практики студентом: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при создании отчёта по прохождению практики студентом: {id}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Назначение наставника студенту
        /// </summary>
        /// <param name="studentId">Принимает Guid студента</param>
        /// <param name="mentorId">Принимает Guid наставника</param>
        /// <returns></returns>
        [Authorize(Roles = "HR-Manager")]
        [HttpPut("[action]")]
        public IActionResult AddStudentMentor(Guid studentId, string mentorId)
        {
            try
            {
                var result = _studentService.AddStudentMentor(studentId, mentorId);

                if (result == null)
                {
                    return BadRequest("Неудалось добавить студенту наставника");
                }
                _logger.LogInformation($"Успешный назначение наставника: {mentorId}, студенту: {studentId}");
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при назначении наставника: {mentorId}, студенту: {studentId}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Добавляет комментарий о прохождении практики для студента
        /// </summary>
        /// <param name="id">Guid студента</param>
        /// <param name="comment">Комментарий</param>
        /// <returns></returns>
        [Authorize(Roles = "Mentor")]
        [HttpPut("[action]")]
        public IActionResult AddStudentPracicComment(Guid id, string comment)
        {
            try
            {
                var result = _studentService.AddStudentPracticComment(id, comment);
                _logger.LogInformation($"Успешный добавление комметария по практике студенту: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Ошибка при добавлении комметария по практике студенту: {id}");
                return BadRequest(ex);

            }
        }

        /// <summary>
        /// Выгрузка информации из карточки студента в Excel
        /// </summary>
        [Authorize(Roles = "HR-Manager")]
        [HttpPost("[action]")]
        public IActionResult StudentCardReport(Guid studentId)
        {
            try
            {
                _logger.LogInformation($"Успешный экспорт карточки студента: {studentId}");
                return RedirectToAction("StudentCard", "Reports", new { id = studentId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при экспорте карточки студента: {studentId}");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Вывод всех студентов находящихся в архиве
        /// </summary>
        [Authorize(Roles = "HR-Manager,Mentor,Director")]
        [HttpPost("[action]")]
        public IActionResult StudentsArchive([FromBody]StudentsSortModel sortModel)
        {
            try
            {
                var result = _studentService.StudentsArchive();
                if (result == null)
                {
                    return BadRequest();
                }
                if (User.IsInRole("Mentor"))
                {
                    var mentor = _userService.GetUserAsync(User.Identity.Name);
                    var mentorResult = _studentService.GetMentorStudents(result, mentor.Result.Id);
                    if (mentorResult.Count() == 0)
                    {
                        return BadRequest("У вас нет студентов находящихся в архиве которых вы курировали");
                    }
                    var mentorSortedResult = _studentService.SortStudents(mentorResult, sortModel);
                    _logger.LogInformation($"Успешное получение студентов из архива {mentorSortedResult}");
                    return Ok(mentorSortedResult);
                }
                var sortedResult = _studentService.SortStudents(result, sortModel);
                _logger.LogInformation($"Успешное получение студентов из архива {sortedResult}");
                return Ok(sortedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении студентов из архива по фильтру {sortModel}");
                return BadRequest(ex);
            }
        }

        [Authorize(Roles = "HR-Manager")]
        [HttpPut("[action]")]
        public IActionResult ChangePhoto(Guid studentId, IFormFile file)
        {
            try
            {
                var imageBytes = _fileService.ImageToBytes(file);
                var result = _studentService.ChangePhoto(studentId, imageBytes);
                if (result == null)
                {
                    return BadRequest("Входе изменения изображения произошла ошибка");
                }
                _logger.LogInformation($"Успешное изменение фотографии студента: {studentId}");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при изменении фотографии студента: {studentId}");
                return BadRequest(ex);
            }
        }

        [HttpGet("[action]")]
        public IActionResult GetStudentPhoto(Guid studentId)
        {
            try
            {
                var student = _studentService.GetStudentByGuid(studentId);
                var image = _fileService.ImageFromBytes(student.Photo);
                if (image == null)
                {
                    return BadRequest();
                }
                _logger.LogInformation($"Успешное получение фотографии студента: {studentId}");
                return Ok(image);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении фотографии студента: {studentId}");
                return BadRequest(ex);
            }
        }

        [HttpPost("[action]")]
        public IActionResult EmailTest([FromForm] SendModel model)
        {
            try
            {
                var request = _requestClient.Create(new { To = model.MailTo, model.Body, model.Subject });

                var responce = request.GetResponse<IEmailSent>();

                return Content($"Email succefully sent! ID:{responce.Result.Message.EventId}  Time:{responce.Result.Message.SentAtUtc}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
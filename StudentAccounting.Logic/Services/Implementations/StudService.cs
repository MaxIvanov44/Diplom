using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAccounting.Common.Enums;
using StudentAccounting.Common.Models;
using StudentAccounting.Data.EntityModels;
using StudentAccounting.Data.Repositories;
using StudentAccounting.Logic.EventBus;
using StudentAccounting.Logic.Models;
using StudentAccounting.Logic.Properties;
using StudentAccounting.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Logic.Services.Implementations
{
    public class StudService : IStudService
    {
        private readonly IStudRepository _studRepository;
        private readonly IRequestClient<IEmailSend> _requestClient;
        private readonly ILogger<StudService> _logger;
        public StudService(IStudRepository studRepository, IRequestClient<IEmailSend> requestClient, ILogger<StudService> logger)
        {
            _studRepository = studRepository;
            _requestClient = requestClient;
            _logger = logger;
        }

        public async Task<StudentsModel> CreateNewStudent(NewStudentModel request, byte[] imageBytes)
        {
            var student = new StudentsEntityModel
            {
                FirstName = request.FirstName,
                SecondName = request.SecondName,
                Patronymic = request.Patronymic,
                InstitutionId = request.InstitutionId,
                Status = Status.New,
                FilingDate = DateTime.Now.Date,
                Email = request.Email,
                Phone = request.Phone,
                PracticArea = request.PracticArea,
                Speciality = request.Speciality,
                PractiesBegining = request.PractiesBegining,
                PractiesEnding = request.PractiesEnding
            };

            var result = _studRepository.CreateStudent(student, imageBytes);

            if (result == null)
            {
                return result;
            }

            var mailSubject = "Заявка на практику";
            var mailBody = "Ваша заявка была успешно оформлена. Ожидайте ответа на указанный вами номер телефона, либо почту.";

            var mailRequest = _requestClient.Create(new { To = request.Email, Body = mailBody, Subject = mailSubject });
            var responce = await mailRequest.GetResponse<IEmailSent>();
            _logger.LogInformation($"Email succefully sent! ID:{responce.Message.EventId}  Time:{responce.Message.SentAtUtc}");

            return result;
        }
        public List<StudentsModel> FindStudents(StudSearchModel request)
        {
            var result = _studRepository.FindStudents(request);
            var dtos = new List<StudentsModel>();
            foreach (var student in result)
            {
                dtos.Add(student);
            }
            return dtos;
        }
        public StudentsModel UpdateStudInfo(StudentsModel request)
        {
            var result = _studRepository.UpdateStudInfo(request);
            return result;
        }

        public async Task<StudentsModel> ChangeStudentStatus(StudentsModel student, string subject, string body)
        {
            var result = _studRepository.UpdateStudInfo(student);

            var request = _requestClient.Create(new { To = student.Email, body, subject });
            var responce = await request.GetResponse<IEmailSent>();
            _logger.LogInformation($"Email succefully sent! ID:{responce.Message.EventId}  Time:{responce.Message.SentAtUtc}");
            return result;
        }

        public StudentsModel DeleteStud(Guid id)
        {
            return _studRepository.DeleteStud(id);
        }

        public void StudInfoExport(StudentsModel request)
        {
            _studRepository.StudInfoExport(request);
        }

        public PracticModel CreateReport(Guid id)
        {
            var result = _studRepository.CreateReport(id);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public List<StudentsModel> GetStudentsByStatus(Status status)
        {
            var result = _studRepository.GetStudentsByStatus(status);
            List<StudentsModel> dtos = new List<StudentsModel>();
            foreach (var student in result)
            {
                dtos.Add(student);
            }
            return dtos;
        }

        public async Task<StudentsModel> AddStudentMentor(Guid studentId, string mentorId)
        {
            var result = _studRepository.AddStudentMentor(studentId, mentorId);
            if (result == null)
            {
                return null;
            }
            var student = _studRepository.GetStudentByGuid(studentId);

            var mailBody = $"Вы были назначены наставником для студента {student.SecondName} {student.FirstName} {student.Patronymic}.";
            var request = _requestClient.Create(new { To = student.Mentor.Email, Body = mailBody, Subject = Resources.Email_Mentor_Subject });
            var responce = await request.GetResponse<IEmailSent>();
            _logger.LogInformation($"Email succefully sent! ID:{responce.Message.EventId}  Time:{responce.Message.SentAtUtc}");

            return student;
        }

        public string AddStudentPracticComment(Guid id, string comment)
        {
            var result = _studRepository.AddStudentPracticComment(id, comment);
            return result;
        }

        public StudentsEntityModel GetStudentByGuid(Guid id)
        {
            var result = _studRepository.GetStudentByGuid(id);
            return result;
        }

        public List<StudentsModel> StudentsArchive()
        {
            var result = _studRepository.StudentsArchive();
            List<StudentsModel> students = new List<StudentsModel>();
            foreach (var student in result)
            {
                students.Add(student);
            }
            return students;
        }

        public StudentsModel CreateStudent(StudentViewModel model, byte[] imageBytes)
        {
            StudentsEntityModel entity = new StudentsEntityModel
            {
                FilingDate = DateTime.Now.Date,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                Patronymic = model.Patronymic,
                Description = model.Description,
                Email = model.Email,
                InstitutionId = model.InstitutionId,
                MentorId = model.MentorId,
                PracticId = model.PracticId,
                IsDeleted = false,
                Phone = model.Phone,
                PracticArea = model.PracticArea,
                PractiesBegining = model.PractiesBegining,
                PractiesEnding = model.PractiesEnding,
                Speciality = model.Speciality,
                Status = model.Status
            };
            var result = _studRepository.CreateStudent(entity, imageBytes);
            return result;
        }
        public StudentsModel GetStudentByPractic(Guid id)
        {
            return _studRepository.GetStudentByPractic(id);
        }

        public List<StudentsModel> GetMentorStudents(List<StudentsModel> students, string mentorId)
        {
            var mentorResult = new List<StudentsModel>();
            foreach (var student in students)
            {
                if (student.MentorId == mentorId)
                {
                    mentorResult.Add(student);
                }
            }
            return mentorResult;
        }
        public IEnumerable<StudentsModel> SortStudents(List<StudentsModel> students, StudentsSortModel sortModel)
        {
            IEnumerable<StudentsModel> query = null;
            if (sortModel.SortByDate == true)
            {
                query = students.OrderBy(st => st.PractiesBegining)
                    .ThenBy(st => st.PractiesEnding);
            }
            if (sortModel.SortByName == true)
            {
                if (query == null)
                {
                    query = students.OrderBy(st => st.SecondName)
                        .ThenBy(st => st.FirstName)
                        .ThenBy(st => st.Patronymic);
                }
                else query.OrderBy(st => st.SecondName)
                        .ThenBy(st => st.FirstName)
                        .ThenBy(st => st.Patronymic);
            }
            if (sortModel.SortByMentor == true)
            {
                if (query == null)
                {
                    query = students.OrderBy(st => st.MentorId);
                }
                else query.OrderBy(st => st.MentorId);
            }
            return query;
        }

        public StudentsModel ChangePhoto(Guid id, byte[] arr)
        {
            return _studRepository.ChangePhoto(id, arr);
        }
    }
}

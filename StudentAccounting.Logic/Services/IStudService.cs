using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccounting.Common.Enums;
using StudentAccounting.Common.Models;
using StudentAccounting.Data.EntityModels;
using StudentAccounting.Logic.Models;
using StudentAccounting.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Logic.Services
{
    public interface IStudService
    {
        StudentsModel CreateStudent(StudentViewModel model, byte[] imageBytes);
        Task<StudentsModel> CreateNewStudent(NewStudentModel request, byte[] imageBytes);
        List<StudentsModel> FindStudents(StudSearchModel request);
        StudentsModel UpdateStudInfo(StudentsModel request);
        StudentsModel DeleteStud(Guid id);
        void StudInfoExport(StudentsModel request);
        PracticModel CreateReport(Guid id);
        List<StudentsModel> GetStudentsByStatus(Status status);
        string AddStudentPracticComment(Guid id, string comment);
        Task<StudentsModel> AddStudentMentor(Guid studentId, string mentorId);
        StudentsEntityModel GetStudentByGuid(Guid id);
        List<StudentsModel> StudentsArchive();
        StudentsModel GetStudentByPractic(Guid id);
        List<StudentsModel> GetMentorStudents(List<StudentsModel> students, string mentorId);
        IEnumerable<StudentsModel> SortStudents(List<StudentsModel> students, StudentsSortModel sortModel);
        Task<StudentsModel> ChangeStudentStatus(StudentsModel student, string subject, string body);
        StudentsModel ChangePhoto(Guid id, byte[] arr);
    }
}

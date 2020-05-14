using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAccounting.Common.Enums;
using StudentAccounting.Common.Models;
using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Data.Repositories
{
    public interface IStudRepository : IBaseRepository<StudentsEntityModel>
    {

        StudentsEntityModel CreateStudent(StudentsEntityModel request, byte[] imageBytes);
        List<StudentsEntityModel> FindStudents(StudSearchModel request);
        StudentsEntityModel UpdateStudInfo(StudentsEntityModel request);
        StudentsEntityModel GetStudentByGuid(Guid id);
        StudentsEntityModel DeleteStud(Guid id);
        void StudInfoExport(StudentsEntityModel request);
        List<StudentsEntityModel> GetStudentsByStatus(Status status);
        PracticEntityModel CreateReport(Guid id);
        StudentsEntityModel AddStudentMentor(Guid studentId, string mentorId);
        string AddStudentPracticComment(Guid id, string comment);
        List<StudentsEntityModel> StudentsArchive();
        StudentsEntityModel GetStudentByPractic(Guid id);
        StudentsEntityModel ChangePhoto(Guid id, byte[] arr);
    }
}

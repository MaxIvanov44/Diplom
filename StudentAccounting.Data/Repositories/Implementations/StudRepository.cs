using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAccounting.Common.Enums;
using StudentAccounting.Common.Models;
using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccounting.Data.Repositories.Implementations
{
    public class StudRepository : BaseRepository<StudentsEntityModel>, IStudRepository
    {
        public StudRepository(StudentAccountingContext context) : base(context) { }
        // Новый студент-практикант
        public StudentsEntityModel CreateStudent(StudentsEntityModel request, byte[] imageBytes)
        {
            if (GetQuery().Contains(request))
            {
                return null;
            }
            else if (!GetQuery().Any(s =>
             s.FirstName == request.FirstName &&
             s.SecondName == request.SecondName &&
             s.Patronymic == request.Patronymic &&
             s.InstitutionId == request.InstitutionId &&
             (s.Status == Status.Arhive || s.Status == Status.New)))
            {
                request.IsDeleted = false;
                request.Photo = imageBytes;
                Insert(request);
                return request;
            }
            else
            {
                StudentsEntityModel existingStudent = GetQuery().Where(s =>
             s.FirstName == request.FirstName &&
             s.SecondName == request.SecondName &&
             s.Patronymic == request.Patronymic &&
             s.InstitutionId == request.InstitutionId).FirstOrDefault();
                existingStudent.Status = Status.New;
                existingStudent.Photo = imageBytes;
                Update(existingStudent);
                return existingStudent;
            }
        }
        // Поиск студентов по заданным критериям
        public List<StudentsEntityModel> FindStudents(StudSearchModel request)
        {
            var result = GetQuery().Where(st =>
    (st.FirstName == request.FirstName && st.SecondName == request.SecondName && st.Patronymic == request.Patronymic) ||
    st.Institution.Name == request.Institution ||
    st.PracticArea == request.Direction ||
    (st.PractiesBegining == request.PractiesBegining && st.PractiesEnding == request.PractiesEnding) && st.IsDeleted == false)
         .Distinct().ToList();
            if (result == null)
            {
                return null;
            }
            return result;
        }

        // Удаление студента практиканта
        public StudentsEntityModel DeleteStud(Guid id)
        {
            var entity = GetQuery().FirstOrDefault(s => s.Id == id);
            entity.IsDeleted = true;
            Update(entity);
            return entity;
        }

        // Изменение данных студента практиканта
        public StudentsEntityModel UpdateStudInfo(StudentsEntityModel request)
        {
            var oldstudent = GetQuery().FirstOrDefault(st => st.Id == request.Id);
            if (oldstudent.Status == Status.Arhive)
            {
                return null;
            }
            oldstudent.ChangeInfo(request);
            Update(oldstudent);
            return request;
        }
        // Экспорт информации о студенте
        public void StudInfoExport(StudentsEntityModel request)
        {
            throw new ArgumentNullException();
        }

        // Создание отчета
        public PracticEntityModel CreateReport(Guid id)
        {
            var currentStudent = GetQuery().FirstOrDefault(s => s.Id == id);
            if (currentStudent.Status == Status.OnPracties)
            {
                currentStudent.Practic = new PracticEntityModel() { Id = Guid.NewGuid(), FillingDate = DateTime.Now };
                Update(currentStudent);
                return currentStudent.Practic;
            }
            return null;
        }

        // выбор всех студентов с выбранным статусом
        public List<StudentsEntityModel> GetStudentsByStatus(Status status)
        {
            var result = GetQuery().Where(s => s.Status == status && s.IsDeleted == false).ToList();
            return result;
        }

        // выбор ментора для студента
        public StudentsEntityModel AddStudentMentor(Guid studentId, string mentorId)
        {
            var currentStudent = GetQuery().FirstOrDefault(s => s.Id == studentId);
            if (currentStudent.Status == Status.Accepted || currentStudent.Status == Status.OnPracties)
            {
                currentStudent.MentorId = mentorId;
                Update(currentStudent);
                return currentStudent;
            }
            return null;
        }

        // добавление комментария по практике
        public string AddStudentPracticComment(Guid id, string comment)
        {
            var currentStudent = GetQuery().FirstOrDefault(s => s.Id == id);
            if (currentStudent.Status == Status.OnPracties)
            {
                currentStudent.Description = comment;
                Update(currentStudent);
                return comment;
            }
            return null;
        }

        // получение студента по Guid
        public StudentsEntityModel GetStudentByGuid(Guid id)
        {
            return GetQuery().Where(st => st.Id == id).Include(st => st.Institution).Include(st => st.Mentor).FirstOrDefault();
        }

        // получение всех студентов из архива
        public List<StudentsEntityModel> StudentsArchive()
        {
            return GetQuery().Where(st => st.Status == Status.Arhive && st.IsDeleted == false).ToList();
        }

        public StudentsEntityModel GetStudentByPractic(Guid id)
        {
            return GetQuery().Where(st => st.PracticId == id).FirstOrDefault();
        }

        public StudentsEntityModel ChangePhoto(Guid id, byte[] arr)
        {
            var student = GetQuery().FirstOrDefault(st => st.Id == id);
            student.Photo = arr;
            Update(student);
            return student;
        }
    }
}

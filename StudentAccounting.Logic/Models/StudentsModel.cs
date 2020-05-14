using StudentAccounting.Common.Enums;
using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudentAccounting.Logic.Models
{
    public class StudentsModel
    {
        public Guid Id { get; set; }                        // Id практиканта
        [Required]
        public DateTime FilingDate { get; set; }            // Дата подачи заявки
        [MaxLength(50)]
        [Required]
        public string SecondName { get; set; }              // Фамилия
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }               // Имя
        [MaxLength(50)]
        [Required]
        public string Patronymic { get; set; }              // Отчество
        [Required]
        [MaxLength(50)]
        public string Speciality { get; set; }              // Специальность
        [Required]
        public DateTime PractiesBegining { get; set; }      // Дата выхода на практику
        [Required]
        public DateTime PractiesEnding { get; set; }        // Дата окончания практики
        [Phone]
        [Required]
        [MaxLength(11)]
        public string Phone { get; set; }                   // Телефон
        [EmailAddress]
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }                   // Email
        [Required]
        public Status Status { get; set; }                  // Статус практиканта
        public byte[] Photo { get; set; }                   // Фото практиканта
        [MaxLength(150)]
        public string Description { get; set; }             // Дополнительная информация (комментарий)
        //public DateTime DeletedDate { get; set; }           // Дата удаления (флаг удаления)

        [Required]
        public Guid InstitutionId { get; set; }               // Id учебного заведения

        public Guid? PracticId { get; set; }                 // Практика студента

        public string MentorId { get; set; }                     // Наставник

        [Required]
        public string PracticArea { get; set; }         // Желаемые направления

        //Неявное преобразование из данной модели в модель БД и наоборот
        public static implicit operator StudentsModel(StudentsEntityModel students)
        {
            if (students == null)
                return null;
            else return new StudentsModel
            {
                Id = students.Id,
                SecondName = students.SecondName,
                Speciality = students.Speciality,
                Status = students.Status,
                Description = students.Description,
                Email = students.Email,
                FilingDate = students.FilingDate,
                FirstName = students.FirstName,
                InstitutionId = students.InstitutionId,
                MentorId = students.MentorId,
                Patronymic = students.Patronymic,
                Phone = students.Phone,
                Photo = students.Photo,
                PracticArea = students.PracticArea,
                PracticId = students.PracticId,
                PractiesBegining = students.PractiesBegining,
                PractiesEnding = students.PractiesEnding
            };
        }
        public static implicit operator StudentsEntityModel(StudentsModel students)
        {
            if (students == null)
                return null;
            else return new StudentsEntityModel
            {
                Id = students.Id,
                SecondName = students.SecondName,
                Speciality = students.Speciality,
                Status = students.Status,
                Description = students.Description,
                Email = students.Email,
                FilingDate = students.FilingDate,
                FirstName = students.FirstName,
                InstitutionId = students.InstitutionId,
                MentorId = students.MentorId,
                Patronymic = students.Patronymic,
                Phone = students.Phone,
                Photo = students.Photo,
                PracticArea = students.PracticArea,
                PracticId = students.PracticId,
                PractiesBegining = students.PractiesBegining,
                PractiesEnding = students.PractiesEnding
            };
        }
    }
}

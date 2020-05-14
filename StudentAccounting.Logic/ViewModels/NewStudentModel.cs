using StudentAccounting.Common.Enums;
using StudentAccounting.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentAccounting.Logic.ViewModels
{
    //Модель заполняемая студентом желающим проходить практику
    public class NewStudentModel
    {
        public string SecondName { get; set; }              // Фамилия
        public string FirstName { get; set; }               // Имя
        public string Patronymic { get; set; }              // Отчество
        public Guid InstitutionId { get; set; }             // Учебное заведение
        public string Speciality { get; set; }              // Специальность
        public string PracticArea { get; set; }             // Желаемое направление
        public DateTime PractiesBegining { get; set; }      // Дата выхода на практику
        public DateTime PractiesEnding { get; set; }        // Дата окончания практики
        public string Phone { get; set; }                   // Телефон
        public string Email { get; set; }                   // Email
    }
}

using StudentAccounting.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudentAccounting.Logic.ViewModels
{
    //Модель для добавления студента в учёт вручную HR-менеджером
    public class StudentViewModel
    {
        [Required]
        public string SecondName { get; set; }              // Фамилия
        [Required]
        public string FirstName { get; set; }               // Имя
        [Required]
        public string Patronymic { get; set; }              // Отчество        
        [Required]
        public string PracticArea { get; set; }              // Желаемое направление
        [Required]
        public string Speciality { get; set; }              // Специальность/Факультет
        [Required]
        public DateTime PractiesBegining { get; set; }      // Дата выхода на практику
        [Required]
        public DateTime PractiesEnding { get; set; }        // Дата окончания практики
        [Required]
        public string Phone { get; set; }                   // Телефон
        [Required]
        public string Email { get; set; }                   // Email
        [Required]
        public Status Status { get; set; }                  // Статус практиканта (новая заявка/одобрена/отклонена/в архиве )
        //public byte[] Photo { get; set; }                   // Фото практиканта
        public string Description { get; set; }             // Дополнительная информация (комментарий)


        [Required]
        public Guid InstitutionId { get; set; }               // Id учебного заведения


        public Guid? PracticId { get; set; }                 // Практика студента


        public string MentorId { get; set; }                     // Наставник
    }
}

using StudentAccounting.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StudentAccounting.Data.EntityModels
{
    [Table("Students")]
    public class StudentsEntityModel
    {
        public StudentsEntityModel() { }
        [Required]
        [Key]
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
        [MaxLength(50)]
        [Required]
        public string PracticArea { get; set; }              // Желаемое направление
        [Required]
        [MaxLength(50)]
        public string Speciality { get; set; }              // Специальность/Факультет
        [Required]
        public DateTime PractiesBegining { get; set; }      // Дата выхода на практику
        [Required]
        public DateTime PractiesEnding { get; set; }        // Дата окончания практики
        [Required]
        [MaxLength(11)]
        public string Phone { get; set; }                   // Телефон
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }                   // Email
        [Required]
        public Status Status { get; set; }                  // Статус практиканта (новая заявка/одобрена/отклонена/в архиве )
        public byte[] Photo { get; set; }                   // Фото практиканта
        [MaxLength(150)]
        public string Description { get; set; }             // Дополнительная информация (комментарий)
        public bool IsDeleted { get; set; }                 // Проверка удаления (флаг удаления)


        [Required]
        public Guid InstitutionId { get; set; }               // Id учебного заведения
        public InstitutionEntityModel Institution { get; set; }


        public Guid? PracticId { get; set; }                 // Практика студента
        public PracticEntityModel Practic { get; set; }


        public string MentorId { get; set; }                     // Наставник
        public UsersEntityModel Mentor { get; set; }

        public void ChangeInfo(StudentsEntityModel updatedStudent)
        {
            this.FirstName = updatedStudent.FirstName;
            this.SecondName = updatedStudent.SecondName;
            this.Patronymic = updatedStudent.Patronymic;
            this.MentorId = updatedStudent.MentorId;
            this.Phone = updatedStudent.Phone;
            this.Photo = updatedStudent.Photo;
            this.PracticId = updatedStudent.PracticId;
            this.PractiesBegining = updatedStudent.PractiesBegining;
            this.PractiesEnding = updatedStudent.PractiesEnding;
            this.Speciality = updatedStudent.Speciality;
            this.Status = updatedStudent.Status;
            this.IsDeleted = updatedStudent.IsDeleted;
            this.InstitutionId = updatedStudent.InstitutionId;
            this.PracticArea = updatedStudent.PracticArea;
        }
    }
}

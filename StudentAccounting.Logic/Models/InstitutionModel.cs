using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudentAccounting.Logic.Models
{
    public class InstitutionModel
    {

        [Key]
        [Required]
        public Guid Id { get; set; }
        [MaxLength(150)]
        [Required]
        public string Name { get; set; }

        // Преобразование из данной модели в модель БД и наоборот

        public static implicit operator InstitutionModel(InstitutionEntityModel institution)
        {
            if (institution == null)
                return null;
            else return new InstitutionModel
            {
                Id = institution.Id,
                Name = institution.Name
            };
        }
        public static implicit operator InstitutionEntityModel(InstitutionModel institution)
        {
            if (institution == null)
                return null;
            else return new InstitutionEntityModel
            {
                Id = institution.Id,
                Name = institution.Name
            };
        }

    }
}

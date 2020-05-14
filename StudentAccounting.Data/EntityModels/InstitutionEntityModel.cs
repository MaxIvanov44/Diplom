using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StudentAccounting.Data.EntityModels
{
    [Table("Institution")]
    public class InstitutionEntityModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [MaxLength(150)]
        [Required]
        public string Name { get; set; }
        public List<StudentsEntityModel> Students { get; set; }
    }
}

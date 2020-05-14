using Microsoft.AspNetCore.Identity;
using StudentAccounting.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StudentAccounting.Data.EntityModels
{
    public class UsersEntityModel : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string SecondName { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Patronymic { get; set; }

    }
}

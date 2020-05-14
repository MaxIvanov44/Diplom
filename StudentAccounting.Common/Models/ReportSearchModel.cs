using System;
using System.Collections.Generic;
using System.Text;

namespace StudentAccounting.Common.Models
{
    //Модель фильтров для формирования отчёта
    public class ReportSearchModel
    {
        public Guid StudentId { get; set; }
        public Guid InstitutionId { get; set; }
        public string Speciality { get; set; }
        public string PracticArea { get; set; }
        public string MentorId { get; set; }
    }
}

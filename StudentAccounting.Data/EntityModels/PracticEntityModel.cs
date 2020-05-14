using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StudentAccounting.Data.EntityModels
{
    [Table("Practic")]
    public class PracticEntityModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime FillingDate { get; set; }

        //public Guid? StudentId { get; set; }

        public int ObuchCriteriaMark { get; set; }
        public string ObuchCriteriaComment { get; set; }
        public int KachCriteriaMark { get; set; }
        public string KachCriteriaComment { get; set; }
        public int OtvestvCriteriaMark { get; set; }
        public string OtvestvCriteriaComment { get; set; }
        public int InicCriteriaMark { get; set; }
        public string InicCriteriaComment { get; set; }
        public int ToxicCriteriaMark { get; set; }
        public string ToxicCriteriaComment { get; set; }
        public int VsaimOkrCriteriaMark { get; set; }
        public string VsaimOkrCriteriaComment { get; set; }
        public int InteresCriteriaMark { get; set; }
        public string InteresCriteriaComment { get; set; }
        public int ItogCriteriaMark { get; set; }
        public string ItogCriteriaComment { get; set; }

    }
}

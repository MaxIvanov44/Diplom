using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudentAccounting.Logic.Models
{
    public class PracticModel
    {
        public Guid Id { get; set; }
        public DateTime FillingDate { get; set; }
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

        // Преобразование из данной модели в модель БД и наоборот
        public static implicit operator PracticEntityModel(PracticModel model)
        {
            return new PracticEntityModel
            {
                Id = model.Id,
                InicCriteriaMark = model.InicCriteriaMark,
                InicCriteriaComment = model.InicCriteriaComment,
                InteresCriteriaMark = model.InteresCriteriaMark,
                InteresCriteriaComment = model.InteresCriteriaComment,
                ObuchCriteriaMark = model.ObuchCriteriaMark,
                ObuchCriteriaComment = model.ObuchCriteriaComment,
                KachCriteriaMark = model.KachCriteriaMark,
                KachCriteriaComment = model.KachCriteriaComment,
                OtvestvCriteriaMark = model.OtvestvCriteriaMark,
                OtvestvCriteriaComment = model.OtvestvCriteriaComment,
                ToxicCriteriaMark = model.ToxicCriteriaMark,
                ToxicCriteriaComment = model.ToxicCriteriaComment,
                VsaimOkrCriteriaMark = model.VsaimOkrCriteriaMark,
                VsaimOkrCriteriaComment = model.VsaimOkrCriteriaComment,
                ItogCriteriaMark = model.ItogCriteriaMark,
                ItogCriteriaComment = model.ItogCriteriaComment,
                FillingDate = model.FillingDate
            };
        }
        public static implicit operator PracticModel(PracticEntityModel entity)
        {
            return new PracticModel
            {
                Id = entity.Id,
                InicCriteriaMark = entity.InicCriteriaMark,
                InicCriteriaComment = entity.InicCriteriaComment,
                InteresCriteriaMark = entity.InteresCriteriaMark,
                InteresCriteriaComment = entity.InteresCriteriaComment,
                ObuchCriteriaMark = entity.ObuchCriteriaMark,
                ObuchCriteriaComment = entity.ObuchCriteriaComment,
                KachCriteriaMark = entity.KachCriteriaMark,
                KachCriteriaComment = entity.KachCriteriaComment,
                OtvestvCriteriaMark = entity.OtvestvCriteriaMark,
                OtvestvCriteriaComment = entity.OtvestvCriteriaComment,
                ToxicCriteriaMark = entity.ToxicCriteriaMark,
                ToxicCriteriaComment = entity.ToxicCriteriaComment,
                VsaimOkrCriteriaMark = entity.VsaimOkrCriteriaMark,
                VsaimOkrCriteriaComment = entity.VsaimOkrCriteriaComment,
                ItogCriteriaMark = entity.ItogCriteriaMark,
                ItogCriteriaComment = entity.ItogCriteriaComment,
                FillingDate = entity.FillingDate
            };
        }
        public static int GetItogMark(PracticModel model)
        {
            return (model.InicCriteriaMark + model.InteresCriteriaMark + model.KachCriteriaMark + model.ObuchCriteriaMark + model.OtvestvCriteriaMark + model.ToxicCriteriaMark + model.VsaimOkrCriteriaMark) / 7;
        }
    }
}

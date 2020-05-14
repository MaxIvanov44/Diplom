using Microsoft.EntityFrameworkCore;
using StudentAccounting.Common.Models;
using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Data.Repositories.Implementations
{
    public class ReportGenRepository : BaseRepository<StudentsEntityModel>, IReportGen
    {
        public ReportGenRepository(StudentAccountingContext context) : base(context)
        {
        }

        public List<StudentsEntityModel> FindStudents(ReportSearchModel model)
        {
            if (model == null || (model.InstitutionId == null && model.MentorId == null && model.PracticArea == "" && model.Speciality == "" && model.StudentId == null))
            {
                return GetQuery().Distinct().ToList();
            }
            //TODO: Если я правильно понял логику запроса, то должно быть что-то вроде этого: ((...||...||...) && IsDeleted == false)
            return GetQuery().Where(st =>
            st.Id == model.StudentId ||
            st.InstitutionId == model.InstitutionId ||
            st.MentorId == model.MentorId ||
            st.PracticArea == model.PracticArea ||
            st.Speciality == model.Speciality &&
            st.IsDeleted == false).Include(st => st.Institution).Include(st => st.Mentor).Distinct().ToList();
        }
    }
}

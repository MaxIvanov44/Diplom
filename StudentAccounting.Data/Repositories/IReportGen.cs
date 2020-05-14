using StudentAccounting.Common.Models;
using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Data.Repositories
{
    public interface IReportGen : IBaseRepository<StudentsEntityModel>
    {
        List<StudentsEntityModel> FindStudents(ReportSearchModel model);
    }
}

using GemBox.Spreadsheet;
using StudentAccounting.Common.Models;
using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Logic.Services
{
    public interface IReportService
    {
        string CreateReport(DataTable table);
        List<StudentsEntityModel> FindStudents(ReportSearchModel model);
        void PracticReport(Guid id);
        void StudentCard(Guid id);
        DataTable StudentsReport(ReportSearchModel model);
    }
}

using GemBox.Spreadsheet;
using StudentAccounting.Common.Models;
using StudentAccounting.Data.EntityModels;
using StudentAccounting.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace StudentAccounting.Logic.Services.ReportGeneration
{
    public class ReportGeneration : IReportService
    {
        private readonly IReportGen _reportGen;
        private readonly IPracticService _practicService;
        private readonly IStudService _studService;
        public ReportGeneration(IReportGen reportGen, IPracticService practicService, IStudService studService)
        {
            _reportGen = reportGen;
            _practicService = practicService;
            _studService = studService;
        }
        public string CreateReport(DataTable table)
        {

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("DataTable to Sheet");
            worksheet.Cells[0, 0].Value = "DataTable insert example:";
            worksheet.InsertDataTable(table,
                new InsertDataTableOptions()
                {
                    ColumnHeaders = true,
                    StartRow = 1
                });
            //на выбор
            //workbook.Save($"../StudentAccounting/wwwroot/reports/studreport.xlsx");
            var path = $"../StudentAccounting/wwwroot/reports/test_{DateTime.Now.ToString("dd.MM.yyyy.hh.mm.ss")}.xlsx";
            workbook.Save(path);
            return path;
        }
        public List<StudentsEntityModel> FindStudents(ReportSearchModel model)
        {
            return _reportGen.FindStudents(model);
        }
        public void PracticReport(Guid id)
        {
            var practic = _practicService.GetPractic(id);
            var stud = _studService.GetStudentByPractic(practic.Id);
            var datatable = new DataTable();
            datatable.Columns.Add("ФИО", typeof(string));
            datatable.Columns.Add("Дата", typeof(string));
            datatable.Columns.Add("Обучаемость", typeof(string));
            datatable.Columns.Add("Качество", typeof(string));
            datatable.Columns.Add("Ответственность", typeof(string));
            datatable.Columns.Add("Инициативность", typeof(string));
            datatable.Columns.Add("Конфликтность", typeof(string));
            datatable.Columns.Add("Взаимоотношение с окружающими", typeof(string));
            datatable.Columns.Add("Интерес к работе", typeof(string));
            datatable.Columns.Add("Итоговая оценка", typeof(string));
            datatable.Rows.Add((stud.SecondName + " " + stud.FirstName + " " + stud.Patronymic), practic.FillingDate.ToShortDateString(),
                (practic.ObuchCriteriaMark + " " + practic.ObuchCriteriaComment),
                (practic.KachCriteriaMark + " " + practic.KachCriteriaComment),
                (practic.OtvestvCriteriaMark + " " + practic.OtvestvCriteriaComment),
                (practic.InicCriteriaMark + " " + practic.InicCriteriaComment),
                (practic.ToxicCriteriaMark + " " + practic.ToxicCriteriaComment),
                (practic.VsaimOkrCriteriaMark + " " + practic.VsaimOkrCriteriaComment),
                (practic.InteresCriteriaMark + " " + practic.InteresCriteriaComment),
                (practic.ItogCriteriaMark + " " + practic.ItogCriteriaComment));
            CreateReport(datatable);
        }

        public void StudentCard(Guid id)
        {
            var student = _studService.GetStudentByGuid(id);
            var dataTable = new DataTable();
            dataTable.Columns.Add("ФИО", typeof(string));
            dataTable.Columns.Add("Адрес электронной почты", typeof(string));
            dataTable.Columns.Add("Номер контактного телефона", typeof(string));
            dataTable.Columns.Add("Учебное заведение", typeof(string));
            dataTable.Columns.Add("Факультет,специальность", typeof(string));
            dataTable.Columns.Add("Сроки практики", typeof(string));
            dataTable.Columns.Add("Направление деятельности", typeof(string));
            dataTable.Rows.Add((student.FirstName + " " + student.SecondName + " " + student.Patronymic), student.Email, student.Phone, student.Institution.Name, student.Speciality, (student.PractiesBegining.ToShortDateString() + " - " + student.PractiesEnding.ToShortDateString()), student.PracticArea);
            CreateReport(dataTable);
        }

        public DataTable StudentsReport(ReportSearchModel model)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(string));
            dataTable.Columns.Add("ФИО", typeof(string));
            dataTable.Columns.Add("Учебное заведение", typeof(string));
            dataTable.Columns.Add("Специальность", typeof(string));
            dataTable.Columns.Add("Направление практики", typeof(string));
            dataTable.Columns.Add("Куратор", typeof(string));
            var result = FindStudents(model);
            foreach (var student in result)
            {
                dataTable.Rows.Add(
                    student.Id,
                    student.FirstName + " " + student.SecondName + " " + student.Patronymic,
                    student.Institution.Name,
                    student.Speciality,
                    student.PracticArea,
                    student.Mentor?.FirstName + student.Mentor?.SecondName + student.Mentor?.Patronymic);
            }
            return dataTable;
            //CreateReport(dataTable);
        }
    }
}
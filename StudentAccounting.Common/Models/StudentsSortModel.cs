using System;
using System.Collections.Generic;
using System.Text;

namespace StudentAccounting.Common.Models
{
    //Модель сортировки студентов
    public class StudentsSortModel
    {
        public bool SortByDate { get; set; }
        public bool SortByName { get; set; }
        public bool SortByMentor { get; set; }
    }
}

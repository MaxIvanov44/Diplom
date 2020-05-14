using System;
using System.Collections.Generic;
using System.Text;

namespace StudentAccounting.Common.Models
{
    //Модель поиска студентов
    public class StudSearchModel
    {

        public string Institution { get; set; }
        public DateTime? PractiesBegining { get; set; }
        public DateTime? PractiesEnding { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }
        public string Direction { get; set; }

    }
}

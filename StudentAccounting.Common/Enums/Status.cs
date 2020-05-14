using System;
using System.Collections.Generic;
using System.Text;

namespace StudentAccounting.Common.Enums
{
    public  enum Status
    {
        //Новая запись
        New = 0,

        //Одобренно
        Accepted = 1,

        //Отклонено
        Refused = 2,

        //На практике
        OnPracties = 3,

        //Архив
        Arhive = 4
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace StudentAccounting.Common.Models
{
    //Модель отправки Email
    public class SendModel
    {

        public string MailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }


    }
}

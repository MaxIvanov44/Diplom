using StudentAccounting.Common.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using StudentAccounting.Logic.Properties;
using MailKit.Net.Smtp;
using System.Net.Mail;
using MimeKit;

namespace StudentAccounting.Logic.Services.EmailSender
{
    public class EmailSender : IEmailService
    {
        public void SendEmailAsync(SendModel sendModel)
        {
            var senderEmail = "apxtest@yandex.ru";
            var senderPassword = "testmail";
            var senderSMTP = "smtp.yandex.ru";

            var email = new MimeMessage
            {
                Body = new TextPart { Text = sendModel.Body },
                Subject = sendModel.Subject,
            };

            email.From.Add(new MailboxAddress(senderEmail));
            email.To.Add(new MailboxAddress(sendModel.MailTo));

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(senderEmail) //адрес отправителя
            }; //создание экземпляра MailMessage
            mail.To.Add(new MailAddress(sendModel.MailTo)); //адрес получателя
            mail.Subject = sendModel.Subject; //заголовок письма
            mail.Body = sendModel.Body; //сам текст письма

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(senderSMTP, 465, true);
                client.Authenticate(senderEmail, senderPassword);
                client.Send(email);

                client.Disconnect(true);
            }
        }
    }
}

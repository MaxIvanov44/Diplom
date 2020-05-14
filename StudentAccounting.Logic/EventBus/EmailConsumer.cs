using MassTransit;
using StudentAccounting.Logic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Logic.EventBus
{
    public class EmailConsumer : IConsumer<IEmailSend>
    {
        private readonly IEmailService _emailService;
        public EmailConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task Consume(ConsumeContext<IEmailSend> context)
        {
            _emailService.SendEmailAsync(new Common.Models.SendModel { MailTo = context.Message.To, Body = context.Message.Body, Subject = context.Message.Subject });

            await context.RespondAsync<IEmailSent>(new { EventId = Guid.NewGuid(), SentAtUtc = DateTime.UtcNow });
        }
    }
}

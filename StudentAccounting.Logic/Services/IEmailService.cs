using StudentAccounting.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentAccounting.Logic.Services
{
    public interface IEmailService
    {
        void SendEmailAsync(SendModel model);
    }
}

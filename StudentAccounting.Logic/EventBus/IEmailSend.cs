using System;
using System.Collections.Generic;
using System.Text;

namespace StudentAccounting.Logic.EventBus
{
    public interface IEmailSend
    {
        string To { get; }
        string Subject { get; }
        string Body { get; }
    }
}

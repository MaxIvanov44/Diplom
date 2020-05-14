using System;
using System.Collections.Generic;
using System.Text;

namespace StudentAccounting.Logic.EventBus
{
    public interface IEmailSent
    {
        Guid EventId { get; }
        DateTime SentAtUtc { get; }
    }
}

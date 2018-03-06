using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace Common.Domain.Interfaces
{
    public interface IEmail : IDisposable
    {
        string SmtpPassword { get; set; }
        string SmtpUser { get; set; }
        string SmtpHost { get; set; }
        int SmtpPort { get; set; }
        string SmtpEmail { get; set; }
        string SmtpName { get; set; }
        bool SmtpEnableSSL { get; set; }
        string Subject { get; set; }
        string Message { get; set; }
        string ReplayTo { get; set; }

        bool Send();
        void EmailRecipientsAdd(string EmailRecipient);
        void EmailRecipientsClear();
        void AttachmentPathsAdd(string attachment);
        void AttachmentBystesAdd(AttachmentConfig attachment);
    }
}

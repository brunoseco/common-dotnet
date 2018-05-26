using System;
using System.Net.Mail;
using System.Configuration;
using Common.Domain.Interfaces;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using Common.Models;

namespace Common.Email
{
    public class Email : IEmail
    {
        private MailMessage mailMessage;
        public Email()
        {
            this.mailMessage = new MailMessage();

            this.SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            this.SmtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            this.SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            this.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
            this.SmtpEmail = ConfigurationManager.AppSettings["EmailSender"];
            this.SmtpName = ConfigurationManager.AppSettings["NameSender"];
            this.SmtpEnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["enableSSL"]);
            this.FooterMessage = ConfigurationManager.AppSettings["FooterMessage"];

            this._attachmentsBytes = new List<AttachmentConfig>();
            this._attachmentsPaths = new List<string>();

            if (!this.SmtpPort.IsSent()) this.SmtpPort = 25;
            //if (!this.FooterMessage.IsSent()) this.FooterMessage = "<span style='display: none;'>{unsubscribe} {accountaddress}</span>";
        }

        public string SmtpPassword { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpEmail { get; set; }
        public string SmtpName { get; set; }
        public bool SmtpEnableSSL { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string FooterMessage { get; set; }
        public string ReplayTo { get; set; }
        private List<string> _attachmentsPaths { get; set; }
        private List<AttachmentConfig> _attachmentsBytes { get; set; }

        public void EmailRecipientsAdd(string emailRecipient)
        {
            this.mailMessage.Bcc.Add(emailRecipient);
        }

        public void AttachmentPathsAdd(string attachment)
        {
            this._attachmentsPaths.Add(attachment);
        }

        public void AttachmentBystesAdd(AttachmentConfig attachment)
        {
            this._attachmentsBytes.Add(attachment);
        }

        public void EmailRecipientsClear()
        {
            this.mailMessage.Bcc.Clear();
        }

        public bool Send()
        {
            if (this.ReplayTo.IsNotNull())
                this.mailMessage.ReplyToList.Add(new MailAddress(this.ReplayTo));

            this.mailMessage.From = new System.Net.Mail.MailAddress(this.SmtpName + "<" + this.SmtpEmail + ">");
            this.mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            this.mailMessage.IsBodyHtml = true;
            this.mailMessage.Subject = this.Subject;
            this.mailMessage.Body = this.Message;
            this.mailMessage.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            this.mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

            if (this.FooterMessage.IsSent())
                this.mailMessage.Body += string.Format("<br />{0}", this.FooterMessage);

            var smtp = new System.Net.Mail.SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(this.SmtpUser, this.SmtpPassword),
                Host = this.SmtpHost,
                Port = this.SmtpPort,
                EnableSsl = this.SmtpEnableSSL
            };

            this.AttachmentsPaths();
            this.AttachmentsByte();

            try
            {
                smtp.Send(this.mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            smtp.Dispose();
            return true;

        }

        private void AttachmentsPaths()
        {
            if (this._attachmentsPaths.IsAny())
            {
                foreach (var attachmentPath in this._attachmentsPaths)
                {
                    var attachment = new Attachment(attachmentPath);
                    this.mailMessage.Attachments.Add(attachment);
                }
            }
        }

        private void AttachmentsByte()
        {
            if (this._attachmentsBytes.IsAny())
            {
                foreach (var attachmentByte in this._attachmentsBytes)
                {
                    var ms = new MemoryStream(attachmentByte.Content);
                    var attachment = new Attachment(ms, attachmentByte.FileName, attachmentByte.ContentType);
                    this.mailMessage.Attachments.Add(attachment);
                }
            }
        }

        public void Dispose()
        {
            this.mailMessage.Dispose();
        }
    }
}

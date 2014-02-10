using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace JasonSoft.Services.SentMail.Domain
{
    public class JMailMessage
    {
        public JMailAddress From { get; set; }
        public JMailAddress[] To { get; set; }
        public JMailMessage[] CC { get; set; }
        public JMailAddress[] Bcc { get; set; }

        public String Subject { get; set; }
        public Boolean IsBodyHtml { get; set; }
        public String Body { get; set; }

        public JMailAddress ReplyTo { get; set; }
        public MailPriority Priority { get; set; }
    }
}

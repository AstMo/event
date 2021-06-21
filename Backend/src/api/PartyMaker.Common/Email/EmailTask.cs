using System;
using System.Collections.Generic;
using System.Text;

namespace PartyMaker.Common.Email
{
    public class EmailTask
    {
        public EmailTask()
        {
            Substitutions = new List<EmailSubstitution>();
        }

        public string Receiver { get; set; }

        public string Subject { get; set; }

        public string Template { get; set; }

        public ICollection<string> Attachments { get; set; }

        public virtual ICollection<EmailSubstitution> Substitutions { get; set; }
    }
}

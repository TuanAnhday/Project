using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Settings.Sections
{
    public class Email
    {
        public SMTP SMTP { get; set; }
        public Credentials Credentials { get; set; }
    }

    public class Credentials
    {
        public string Password { get; set; }
        public string SenderAddress { get; set; }
        public string Name { get; set; }
        public string ReplyToAddress { get; set; }
    }

    public class SMTP
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}

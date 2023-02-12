using Project.Domain.Settings.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Settings
{
    public class AppSettings
    {
        public Jwt Jwt { get; set; }
        public Email Email { get; set; }
        public Web Web { get; set; }
        public string Secret { get; set; }
        public string CloudStorageBucket { get; set; }
        public string CloudStorageUrl { get; set; }
        public string Cookies { get; set; }
        public int Expires { get; set; }
        public string StaticDataFolder { get; set; }
    }
}

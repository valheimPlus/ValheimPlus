using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValheimPlusManager.Models
{
    public class ValheimPlusUpdate
    {
        public bool NewVersion { get; set; }
        public string Version { get; set; }
        public string WindowsGameClientDownloadURL { get; set; }
        public string WindowsServerClientDownloadURL { get; set; }
    }
}

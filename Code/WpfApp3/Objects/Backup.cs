using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Objects
{
    public class Backup
    {
        public int id { get; set; }
        public string name { get; set; }

        public string type { get; set; }
        public string sourcePath { get; set; }

        public string destinationPath { get; set; }
        
        public DateTime date { get; set; }
    }
}

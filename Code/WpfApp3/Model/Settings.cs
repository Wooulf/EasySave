using System.Collections.Generic;

namespace WpfApp3.Model
{
    public class Settings
    {
        public string cryptoSoftPath { get; set; }
        public List<string> cryptoSoftExtensions { get; set; }
        public string businessSoftware { get; set; }
        public string language { get; set; }
        public string fileSize { get; set; }
        
        public void Update(string _cryptoSoftPath, List<string> _cryptoExtensions, string _businessSoftware, string _language, string _fileSize)
        {
            this.cryptoSoftPath = _cryptoSoftPath;
            this.cryptoSoftExtensions = _cryptoExtensions;
            this.businessSoftware = _businessSoftware;
            this.language = _language;
            this.fileSize = _fileSize;
        }
    }
}
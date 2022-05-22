namespace WpfApp3.Model
{
    public class BackupWork
    {
        #region Variable BackupWork
        public string name { get; set; }
        public EnumBackupType enumBackupType { get; set; }
        public string sourcePath { get; set; }
        public string destinationPath { get; set; }
        public State state { get; set; }
        public Log log { get; set; }

        #endregion Variable BackupWork 
        #region BackupWork Method 

        public BackupWork() {}

        public BackupWork(string _name, string _sourcePath, string _destinationPath, EnumBackupType _enumBackupType)
        {
            this.name = _name;
            this.sourcePath = _sourcePath;
            this.destinationPath = _destinationPath;
            this.enumBackupType = _enumBackupType;
            this.state = null;
            this.log = null;
        }
        #endregion BackupWork Method 

    }
}
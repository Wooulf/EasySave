namespace WpfApp3.Model
{
    public abstract class File
    {
        #region Variable  File 

        public string name { get; set; }
        public string sourcePath { get; set; }
        public string destinationPath { get; set; }
        public long fileSize { get; set; }

        #endregion Variable  File

    }
}
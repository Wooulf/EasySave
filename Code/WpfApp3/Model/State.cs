using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WpfApp3.Model
{
    public class State : File
    {
        #region Variable State
        public int totalFileLeft { get; set; }
        public long totalSizeLeft { get; set; }
        public int totalFile { get; set; }
        public int progress { get; set; }
        #endregion Variable State

        #region Method State

        public State()
        {
            
        }

        public State(string _name, string _sourcePath, string _destinationPath, int _totalFile, long _totalSize, int _totalFileLeft, int _progress )
        {
            this.totalFile = _totalFile;
            this.fileSize = _totalSize;
            this.sourcePath = _sourcePath;
            this.destinationPath = _destinationPath;
            this.progress = 0;
            this.name = _name;
            this.totalFileLeft = _totalFileLeft;
            this.progress = _progress;


        }
        public void UpdateState(int _fileLeft, long _sizeLeft, int _progress, string _sourcePath, string _destinationPath ,JsonSerializerOptions jsonOptions)
        {
            this.totalFileLeft = _fileLeft;
            this.totalSizeLeft = _sizeLeft;
            this.progress = _progress;
            this.sourcePath = _sourcePath;
            this.destinationPath = _destinationPath;

            this.progress = this.totalFileLeft / this.totalFile * 100;
            
            //Create the directory unless it already exist
            var path = Directory.CreateDirectory("./State").FullName;
            
            var states = new List<State>();

            if (System.IO.File.Exists(path + @"/State.json"))
            {
                states = JsonSerializer.Deserialize<List<State>>(
                    System.IO.File.ReadAllText(path + @"/State.json"));
            
                int index = states.FindIndex(s => s.name == name);

                if (index != -1)
                {
                    states[index] = new State(this.name, this.sourcePath, this.destinationPath, this.totalFile,
                        this.fileSize, this.totalFileLeft, this.progress);
                }
                else
                {
                    states.Add(new State(this.name, this.sourcePath, this.destinationPath, this.totalFile, this.fileSize, this.totalFileLeft, this.progress));
                } 
            }
            else
            {
                states.Add(new State(this.name, this.sourcePath, this.destinationPath, this.totalFile, this.fileSize, this.totalFileLeft, this.progress));
            }
            
            
            
            System.IO.File.WriteAllText(path + @"/State.json", JsonSerializer.Serialize(states,jsonOptions));

        }
        #endregion Method State
    }
}
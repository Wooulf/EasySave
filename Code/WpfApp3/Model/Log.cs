using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace WpfApp3.Model
{
    public class Log: File
    {
        #region Variable Log
        public DateTime startTime { get; set; }
        public string timeSpent { get; set; }
        
        public int cryptedTime { get; set; }

        #endregion Variable Log 

        #region Method Log

        public Log()
        {
            
        }

        private Log(string _name, string _sourcePath, string _destinationPath, int _fileSize, DateTime _startTime, string _timeSpent, int _cryptedTime)
        {
            this.name = _name;
            this.sourcePath = _sourcePath;
            this.destinationPath = _destinationPath;
            this.fileSize = _fileSize;
            this.startTime = _startTime;
            this.timeSpent = _timeSpent;
            this.cryptedTime = _cryptedTime;
        }

        public static void SaveLog(string _name, string _sourcePath, string _destinationPath, int _fileSize, DateTime _startTime, string _timeSpent, JsonSerializerOptions jsonOptions, bool error, int _cryptedTime)
        {
            //Create the directory unless it already exist
            Directory.CreateDirectory("./Logs");

            var logs = new List<Log>();
            
            //if the today's log file already exist, we copy it
            if (System.IO.File.Exists($"./Logs/{_startTime:yyyy-MM-dd}.json"))
            {
                logs = JsonSerializer.Deserialize<List<Log>>(
                    System.IO.File.ReadAllText($"./Logs/{_startTime:yyyy-MM-dd}.json"));
            }

            if (error)
            {
                _timeSpent = "-404";
            }
            
            //we add the new log to the logs
            logs.Add(new  Log(_name, _sourcePath, _destinationPath, _fileSize, _startTime, _timeSpent, _cryptedTime));
            
            //we write the brand new logs to the logs file
            System.IO.File.WriteAllText($"./Logs/{_startTime:yyyy-MM-dd}.json", JsonSerializer.Serialize(logs, jsonOptions));
        }

        #endregion Method Log
    }
}
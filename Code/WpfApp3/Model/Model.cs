using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace WpfApp3.Model
{
    public class Model
    {
        #region Variable Model
        public List<BackupWork> backupWorks { get; private set; }
        private const string backupWorkpath = "./ConfigBackup.json";
        private const string settingsFilePath = "./Settings.json";
        public Settings settings;

        // Prepare options for the Json Files
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        #endregion Variable Model

        #region Method Model

        //Constructor method
        public Model()
        {
            //Initialise backupWorks with a list of BackupWork
            this.backupWorks = new List<BackupWork>();
            LoadSettings();
        }

        //Add BackupWork
        public void AddBackupWork(string _name, string _sourcePath, string _destinationPath, EnumBackupType _enumBackupType)
        {
            //Add a BackupWork to the list of BackupWorks
            this.backupWorks.Add(new BackupWork(_name, _sourcePath, _destinationPath, _enumBackupType));
            SaveBackupWork();
        }

        //Use for 
        public void LoadBackupWork()
        {
            //test if the file ConfigBackup.json exist
            if (System.IO.File.Exists(backupWorkpath))
            {
                //Read the Json file and fill backupWorks
                this.backupWorks = JsonSerializer.Deserialize<List<BackupWork>>(System.IO.File.ReadAllText(backupWorkpath));

            }
        }

        //Save BackupWork
        public void SaveBackupWork()
        {
            //Write into the Json file ConfigBackup.json in order to save
            System.IO.File.WriteAllText(backupWorkpath, JsonSerializer.Serialize(this.backupWorks, this.jsonOptions));
        }

        public void DeleteBackupWork(int _number)
        {
            //Delete the BackupWork thanks to the index
            this.backupWorks.RemoveAt(_number);
            SaveBackupWork();

        }
        public bool FileToCopy(BackupWork _backupWork, long _sizeLeft, int _totalFile, int _progress, int _index, string _destinationPath, FileInfo _currentFile)
        {
            //startTime is use in the log
            DateTime startTime = DateTime.UtcNow;
            //Get the directory full path
            string currentFilePath = _currentFile.DirectoryName;

            //Check if the directory exist, if it exist, _destiantionPath get the relative path
            if (Path.GetRelativePath(_backupWork.sourcePath, currentFilePath).Length > 1)
            {
                _destinationPath += @"\" + Path.GetRelativePath(_backupWork.sourcePath, currentFilePath);

                //If the directory _destinationPath doesn't exist it create it, if it exist it does nothing 
                Directory.CreateDirectory(_destinationPath);
            }

            //add the file name to the path
            _destinationPath += @"\" + _currentFile.Name;


            //Try or catch if there is an error
            try
            {
                //Update the state File
                _backupWork.state.UpdateState((_totalFile - _index), _sizeLeft, _progress, _currentFile.FullName, _destinationPath, jsonOptions);
                SaveBackupWork();
                int elapsedTime = -1;
                int cryptedTime = 0;
                var test = settings.cryptoSoftExtensions.Contains(
                    _currentFile.Name.Substring(_currentFile.Name.LastIndexOf(".")));
                var test2 = _currentFile.Name.Contains(".");
                //Copy the current file to the distinationFile
                if (!(settings.cryptoSoftPath.Length != 0 && settings.cryptoSoftExtensions.Count != 0 && _currentFile.Name.Contains(".") && settings.cryptoSoftExtensions.Contains(_currentFile.Name.Substring(_currentFile.Name.LastIndexOf(".")))))
                {
                    _currentFile.CopyTo(_destinationPath, true);
                    elapsedTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
                }
                // Crypt the current file
                else
                {
                    try
                    {
                        ProcessStartInfo process = new ProcessStartInfo(settings.cryptoSoftPath)
                        {
                            ArgumentList =
                            {
                                _currentFile.FullName,
                                _destinationPath
                            },
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };
                        var proc = Process.Start(process);
                        /*while (!proc.HasExited)
                        {
                            if (Process.GetProcessesByName(settings.businessSoftware).Length > 0)
                            {
                                proc.Kill();
                            }

                        }
                        proc.WaitForExit();
                        cryptedTime = proc.ExitCode;*/
                        proc?.WaitForExit();
                        if (proc != null)
                        {
                            cryptedTime = proc.ExitCode;
                        }
                    }
                    catch
                    {
                        // Return Error Code
                        cryptedTime = -1;
                    }
                }

                //Update the state File when finish
                _backupWork.state.UpdateState((_totalFile - _index), _sizeLeft, _progress, _currentFile.DirectoryName, _destinationPath, jsonOptions);

                //Mark in the log
                _backupWork.log = new Log();
                if (_backupWork.log == null) return true;
                Log.SaveLog(_backupWork.name, _currentFile.FullName, _destinationPath, _totalFile, startTime, (DateTime.Now - startTime).ToString(), jsonOptions, false, cryptedTime);
                _backupWork.log = null;

                return true;
            }
            catch
            {
                Log.SaveLog(_backupWork.name, _currentFile.FullName, _destinationPath, _totalFile, startTime, (DateTime.Now - startTime).ToString(), jsonOptions, true, -1);
                return false;
            }

        }

        private void LoadSettings()
        {
            // Check if backupWorkSave.json File exists
            if (System.IO.File.Exists(settingsFilePath))
            {
                try
                {
                    // Read Works from JSON File (from ./BackupWorkSave.json) (use Work() constructor)
                    this.settings = JsonSerializer.Deserialize<Settings>(System.IO.File.ReadAllText(settingsFilePath));
                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                // Create Settings File
                SaveSettings();
            }
        }

        public void SaveSettings()
        {
            // Write Work list into JSON file (at ./BackupWorkSave.json)
            System.IO.File.WriteAllText(settingsFilePath, JsonSerializer.Serialize(this.settings, this.jsonOptions));
        }
        #endregion Method Model

    }
}
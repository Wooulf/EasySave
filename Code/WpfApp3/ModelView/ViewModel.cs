using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using WpfApp3.Model;
using WpfApp3.Objects;
using File = System.IO.File;

namespace WpfApp3.ModelView
{
    public class ViewModel
    {
        #region Variable ViewModel
        private WpfApp3.Model.Model model;
        #endregion Variable ViewModel

        #region Method ViewModel
        public ViewModel()
        {
            model = new WpfApp3.Model.Model();
            model.LoadBackupWork();
        }

        public int AddBackUpWorks(string _name, string _sourcePath, string _destinationPath, string backupType)
        {


            EnumBackupType enumBackupType;
            switch (backupType)
            {

                case "Full":
                    enumBackupType = EnumBackupType.Full;
                    break;

                case "Differential":
                    enumBackupType = EnumBackupType.Differential;
                    break;

                default:
                    enumBackupType = EnumBackupType.Full;
                    break;

            }

            model.AddBackupWork(_name, _sourcePath, _destinationPath, enumBackupType);

            return 0;

        }

        public List<Backup> LoadBackUpWork()
        {
            List<Backup> backups = new List<Backup>();
            var i = 0;
            foreach (var backupWork in model.backupWorks)
            {
                backups.Add(new Backup() { id = i, name = backupWork.name, type = Enum.GetName(typeof(EnumBackupType), backupWork.enumBackupType), sourcePath = backupWork.sourcePath, destinationPath = backupWork.destinationPath, date = Directory.GetLastWriteTime(backupWork.destinationPath) });
                i++;
            }

            return backups;
        }

        public void ExecuteBackUpWork(int _choice)
        {
            if (model.backupWorks.Count > 0)
            {

                switch (_choice)
                {
                    case -1:
                        foreach (BackupWork backupWork in model.backupWorks)
                        {
                            ExecuteBackUpWorkType(backupWork);
                        }
                        break;

                    default:
                        ExecuteBackUpWorkType(model.backupWorks[_choice]);
                        break;
                }

            }
        }

        private void ExecuteBackUpWorkType(BackupWork _backupWork)
        {
            DirectoryInfo directory = new DirectoryInfo(_backupWork.sourcePath);

            if (!Directory.Exists(_backupWork.destinationPath) && !directory.Exists)
            {
                return;
            }

            switch (_backupWork.enumBackupType)
            {
                case EnumBackupType.Differential:
                    if (FullBackUpWorkDirectory(_backupWork) != null)
                    {
                        DifferentialBackUpWork(_backupWork, directory);
                        break;
                    }

                    FullBackUpWork(_backupWork, directory);
                    break;

                case EnumBackupType.Full:
                    FullBackUpWork(_backupWork, directory);
                    break;
            }
        }

        private int SaveBackUpWork(BackupWork _backUpWork, FileInfo[] _files, long _totalSize)
        {

            DateTime startTime = DateTime.Now;
            _backUpWork.state = new State(_backUpWork.name, _backUpWork.sourcePath, _backUpWork.destinationPath,
                _files.Length, _totalSize, _files.Length, 0);
            string destinationPath = _backUpWork.destinationPath + @"\" + _backUpWork.name + "_" +
                                     startTime.ToString("yyyy-MM-dd_HH-mm-ss");
            Directory.CreateDirectory(destinationPath);

            List<string> failedFiles = FilesToCopy(_backUpWork, _totalSize, destinationPath, _files);

            _backUpWork.state = null;
            model.SaveBackupWork();

            if (failedFiles.Count == 0)
            {
                return 0;
            }

            return 1;

        }

        public void DeleteBackUpWork(int _number)
        {
            this.model.DeleteBackupWork(_number);
        }

        private List<String> FilesToCopy(BackupWork _backUpWork, long _totalSize, string _destinationPath, FileInfo[] _files)
        {
            int totalFile = _files.Length;
            long sizeLeft = _totalSize;
            List<string> filesEchec = new List<string>();

            for (int i = 0; i < _files.Length; i++)
            {
                int progress = (i * 100) / totalFile;
                sizeLeft -= _files[i].Length;

                if (model.FileToCopy(_backUpWork, sizeLeft, totalFile, progress, i, _destinationPath, _files[i]))
                {
                    //Implement display
                }
                else
                {
                    filesEchec.Add(_files[i].Name);
                }
            }

            return filesEchec;
        }

        private static string FullBackUpWorkDirectory(BackupWork _backupWork, string _path = null)
        {
            DirectoryInfo[] directories = new DirectoryInfo(_backupWork.destinationPath).GetDirectories();

            foreach (DirectoryInfo directory in directories)
            {
                if (_path != null)
                {
                    string pathfile = directory + @"\" + _path;
                    if (_backupWork.name == directory.Name.Substring(0, directory.Name.IndexOf("_")) && directory.Name.IndexOf("_") > 0 && File.Exists(pathfile))
                    {
                        return directory.FullName;
                    }
                }
                else
                {
                    if (_backupWork.name == directory.Name.Substring(0, directory.Name.IndexOf("_")) && directory.Name.IndexOf("_") > 0)
                    {
                        return directory.FullName;
                    }
                }

            }

            return null;
        }

        private void FullBackUpWork(BackupWork _backUpWork, DirectoryInfo _directoryInfo)
        {
            FileInfo[] files = _directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            long totalSize = 0;

            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }
            SaveBackUpWork(_backUpWork, files, totalSize);
        }

        private void DifferentialBackUpWork(BackupWork _backUpWork, DirectoryInfo _directoryInfo)
        {
            FileInfo[] sourceFiles = _directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            List<FileInfo> filesToCopy = new List<FileInfo>();
            long totalSize = 0;

            //Check for modification
            foreach (var sourceFile in sourceFiles)
            {
                string path = FullBackUpWorkDirectory(_backUpWork, Path.GetRelativePath(_backUpWork.sourcePath, sourceFile.FullName)) + "\\" +
                              Path.GetRelativePath(_backUpWork.sourcePath, sourceFile.FullName);
                if (!File.Exists(path))
                {
                    path = Path.GetRelativePath(_backUpWork.sourcePath, sourceFile.FullName);
                }
                FileInfo pathFile = new FileInfo(path);

                if (!CheckFiles(pathFile, sourceFile) || !File.Exists(path))
                {
                    //Size of  every files
                    totalSize += sourceFile.Length;

                    filesToCopy.Add(sourceFile);
                }

            }

            if (filesToCopy.Count == 0)
            {
                model.SaveBackupWork();
                return;
            }

            SaveBackUpWork(_backUpWork, filesToCopy.ToArray(), totalSize);
        }

        //Check if the file
        private bool CheckFiles(FileInfo firstFile, FileInfo secondFile)
        {

            if (!firstFile.Exists)
            {
                return false;
            }

            //Check if the length is the same
            if (firstFile.Length != secondFile.Length)
                return false;

            //Check if the full path of both file are the same
            if (string.Equals(firstFile.FullName, secondFile.FullName, StringComparison.OrdinalIgnoreCase))
                return true;

            BitArray bitFile1 = new BitArray(File.ReadAllBytes(firstFile.FullName));
            BitArray bitFile2 = new BitArray(File.ReadAllBytes(secondFile.FullName));
            var test = model.settings.cryptoSoftExtensions.Contains(
                firstFile.Name.Substring(firstFile.Name.LastIndexOf(".")));
            if (model.settings.cryptoSoftExtensions.Contains(firstFile.Name.Substring(firstFile.Name.LastIndexOf("."))))
            {
                byte[] byteKey = { 12, 255, 102, 147, 8, 52, 157, 235 };
                BitArray bitKey = new BitArray(byteKey);

                for (var i = 0; i < bitFile1.Length; i++)
                {
                    bitFile1[i] ^= bitKey[i % byteKey.Length];
                }
            }

            for (var i = 0; i < bitFile1.Length; i++)
            {
                if (bitFile1[i] != bitFile2[i])
                {
                    return false;
                }
            }
            return true;

        }

        public void ChangeSettings(string _businessSoftware = "", string _fileSize = "0", string _language = "English",
            string _cryptoExtensions = null)
        {
            List<string> extensions = new List<string>(_cryptoExtensions.Split(' '));
            this.model.settings = new Settings()
            {
                businessSoftware = _businessSoftware,
                fileSize = _fileSize,
                cryptoSoftExtensions = extensions,
                language = _language,
                cryptoSoftPath = @".\CryptoSoft\CryptoSoft.exe"
            };
            model.SaveSettings();
        }

        public Settings LoadSettings()
        {
            return model.settings;
        }

        #endregion Method ViewModel

    }

}
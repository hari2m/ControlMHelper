using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Promoter
{
    public class Process
    {
        #region privateVariables
        private readonly string _path;
        private readonly string _sourceEnv;
        private readonly string _targetEnv;
        #endregion

        #region Constructor
        public Process(string path, string sourceEnv, string targetEnv)
        {
            _path = path;
            _sourceEnv = sourceEnv.ToLower();
            _targetEnv = targetEnv.ToLower();
        }
        #endregion

        private string TargetDirectory { get { return @$"{_path}\{_targetEnv}"; } }
        private string Targetfile(string fileName) => @$"{TargetDirectory}\{fileName}";

        public void StartProcess()
        {
            try
            {
                Console.WriteLine("Getting list of files");
                var files = Directory.GetFiles(_path);
                foreach (var file in files)
                {
                    if(Path.GetExtension(file).ToLower() != ".xml") continue;
                    Console.WriteLine($"Processing file {file}");
                    string content;
                    using (var sr = new StreamReader(file))
                    {
                        content = sr.ReadToEnd();
                        foreach (var record in DataToChange.Profiles)
                        {
                            content = UpdateContent(content, record);
                        }
                    }
                    SaveTargetFile(content.Trim(), file.Split('\\')[file.Split('\\').Length - 1].Replace(GetFolderPrefix().Pair[_sourceEnv], GetFolderPrefix().Pair[_targetEnv]));
                }
            }
            catch (DirectoryNotFoundException exception)
            {
                Console.WriteLine("Please check if the first argument provided is a correct directory");
                Console.WriteLine(exception.Message);
                throw;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private string UpdateContent(string content, Profiles record)
        {
            Console.WriteLine($"Found {Regex.Split(content, record.Profile[_sourceEnv], RegexOptions.IgnoreCase).Length - 1} instance of {record.Profile[_sourceEnv]}");
            return content.Replace(record.Profile[_sourceEnv], record.Profile[_targetEnv], true, CultureInfo.CurrentCulture);
        }

        private void SaveTargetFile(string fileContent, string fileName)
        {
            try
            {
                if (!Directory.Exists(TargetDirectory))
                {
                    Directory.CreateDirectory(TargetDirectory);
                }
                using (var sw = new StreamWriter(Targetfile(fileName)))
                {
                    Console.WriteLine($"Writing to file {Targetfile(fileName)}");
                    sw.Write(fileContent);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public ObjectsTochange DataToChange
        {
            get
            {
                return ConfigLoader.LoadConfig(_path);
            }
        }

        private Profile GetFolderPrefix() => new Profile
        {
            Pair = new Dictionary<string, string>
                {
                    {"dev", "DEV1"},
                    {"qa", "SYS1"},
                    {"stage", "PSUP"},
                    {"prod", "PROD" }
                }
        };
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Packages.DataClassGenerator.Editor
{
    
    public static class SaveSettigFiles
    {
        private const string SettingsPath = "Assets/CsvToData/Settings/";
        private const string JsonFileName = "CsvToDataSettings.json";
        private const string RspFileName = "csc.rsp";
        public static void SaveSettingsToJson(CsvToDataSettings settings)
        {
            if (!Directory.Exists(SettingsPath))
            {
                Directory.CreateDirectory(SettingsPath);
            }

            string json = JsonUtility.ToJson(settings, true);
            File.WriteAllText(Path.Combine(SettingsPath, JsonFileName), json);
        }
        
        public static void RemoveFromRspFile(IEnumerable<string> rootPaths)
        {
            string rspPath = Path.Combine("Assets", RspFileName);
            List<string> lines = new List<string>();

            if (File.Exists(rspPath))
            {
                lines = File.ReadAllLines(rspPath).ToList();
            }
            
            foreach (var rootPath in rootPaths)
            {
                foreach (var line in lines)
                {
                    if (line.Contains(rootPath))
                    {
                        lines.Remove(line);
                    }
                }
            }

            File.WriteAllLines(rspPath, lines);
        }

        public static void AppendToRspFile(CsvToDataSettings settings)
        {
            string rspPath = Path.Combine("Assets", RspFileName);
            List<string> lines = new List<string>();

            if (File.Exists(rspPath))
            {
                lines = File.ReadAllLines(rspPath).ToList();
            }

            string additionalFileLine = $"/additionalfile:{Path.Combine(SettingsPath, JsonFileName)}";
            if (!lines.Contains(additionalFileLine))
            {
                lines.Add(additionalFileLine);
            }

            foreach (var setting in settings.settings)
            {
                string filePath = setting.FilePath;

                string fileLine = $"/additionalfile:{filePath.Replace('\\', '/')}";
                if (!lines.Contains(fileLine))
                {
                    lines.Add(fileLine);
                }
                
            }

            File.WriteAllLines(rspPath, lines);
        }

    }
}
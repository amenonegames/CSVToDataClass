using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Packages.DataClassGenerator.Editor
{
    [CustomEditor(typeof(CsvToDataSettings))]
    [CanEditMultipleObjects]
    public class CsvToDataSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            CsvToDataSettings settings = (CsvToDataSettings)target;

            // Add a button to add a new item with default values
            if (GUILayout.Button("Add New Setting"))
            {
                if (settings.settings == null)
                {
                    settings.settings = new List<CsvToDataSetting>();
                }
                settings.settings.Add(new CsvToDataSetting
                {
                    FilePath = "Assetsからの相対位置でファイルパスを指定",
                    CSVSeparator = ",",
                    NameSpace = "Data",
                    Serializable = true,
                    InterfaceEnable = false,
                    InterfaceName = "",
                    Usings = new string[] {}  // Initialize as empty or with default values
                });

                EditorUtility.SetDirty(settings);  // Mark the settings object as "dirty" to ensure changes are saved
            }

            // Optionally, add a button to save changes (depends on your setup and needs)
            if (GUILayout.Button("Save Settings"))
            {
                RemoveCheck(settings);
                SaveSettigFiles.SaveSettingsToJson(settings);
                SaveSettigFiles.AppendToRspFile(settings);
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                settings.prevRootPaths = settings.settings.Select(s => s.FilePath).ToArray();
            }
            
        }

        private void RemoveCheck(CsvToDataSettings settings)
        {
            if (settings.prevRootPaths is null) return;
            
            List<string> removedRootPath = new List<string>();
            foreach (var prevRootPath in settings.prevRootPaths)
            {
                if( settings.settings.Select(x => x.FilePath).Any( x => x != prevRootPath) )
                    removedRootPath.Add(prevRootPath);
            }
                
            if(removedRootPath.Count > 0)
                SaveSettigFiles.RemoveFromRspFile(removedRootPath);
        }
        

    }
}
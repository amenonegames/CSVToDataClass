using UnityEditor;
using UnityEngine;

namespace DataClassGenerator
{

    public class DataClassDirectorySettings : ScriptableObject
    {
        const string SettingsPath = "Project/DataClassDirectorySettings";

        [SerializeField] private string directoryPath;
        [SerializeField] private string csvDirStructureCopyLimit;

        public static string DirectoryPath
        {
            get => Instance.directoryPath;
            set => Instance.directoryPath = value;
        }

        public static string CsvDirStructureCopyLimit
        {
            get => Instance.csvDirStructureCopyLimit;
            set => Instance.csvDirStructureCopyLimit = value;
        }
        
        private static DataClassDirectorySettings Instance
        {
            get
            {
                var settings = AssetDatabase.LoadAssetAtPath<DataClassDirectorySettings>(SettingsPath);
                if (settings == null)
                {
                    settings = ScriptableObject.CreateInstance<DataClassDirectorySettings>();
                    AssetDatabase.CreateAsset(settings, SettingsPath);
                    AssetDatabase.SaveAssets();
                }
                return settings;
            }
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider("Project/DataClassDirectorySettings", SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    var settings = DataClassDirectorySettings.Instance;

                    EditorGUILayout.LabelField("Directory Settings", EditorStyles.boldLabel);
                    settings.directoryPath = EditorGUILayout.TextField("Directory Path", settings.directoryPath);

                    EditorGUILayout.LabelField("CSV Dir Structure Copy Limit", EditorStyles.boldLabel);
                    settings.directoryPath = EditorGUILayout.TextField("CSV Dir Structure Copy Limit", settings.csvDirStructureCopyLimit);
                    
                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(settings);
                    }
                }
            };
        }
    }

}
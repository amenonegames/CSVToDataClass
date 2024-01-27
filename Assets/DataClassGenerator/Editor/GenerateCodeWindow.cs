using UnityEditor;
using UnityEngine;

namespace DataClassGenerator
{
    public class GenerateCodeWindow : EditorWindow
    {
        private TextAsset textAsset;
        private string folderPath = ""; // フォルダパスを格納する変数
        
        [MenuItem("Tools/GenerateCode/DataClassFromCsv")]
        private static void OpenWindow()
        {
            GetWindow<GenerateCodeWindow>("Generate Code");
        }

        private void OnGUI()
        {
            GUILayout.Label("Attach CSV", EditorStyles.boldLabel);
            textAsset =
                (TextAsset)EditorGUILayout.ObjectField("CSV", textAsset, typeof(TextAsset), false);
            
            // フォルダパスの選択
            GUILayout.Label("Select Output Folder", EditorStyles.boldLabel);
            if (GUILayout.Button("Select Folder"))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    folderPath = selectedPath;
                }
            }

            // 選択されたフォルダパスを表示
            EditorGUILayout.TextField("Folder Path", folderPath);

            if (GUILayout.Button("Generate DataClass") && textAsset != null && !string.IsNullOrEmpty(folderPath))
            {
                DataClassGenerator.Generate(textAsset, folderPath);
            }

        }
    }
}
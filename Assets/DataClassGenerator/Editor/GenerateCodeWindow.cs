using UnityEditor;
using UnityEngine;

namespace DataClassGenerator
{
    public class GenerateCodeWindow : EditorWindow
    {
        private TextAsset textAsset;
        private string folderFullPath = ""; // フォルダパスを格納する変数
        
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
                    folderFullPath = selectedPath;
                }
            }

            // 選択されたフォルダパスを表示
            EditorGUILayout.TextField("Folder Path", folderFullPath);

            if (GUILayout.Button("Generate DataClass") && textAsset != null && !string.IsNullOrEmpty(folderFullPath))
            {
                DataClassGenerator.Generate(textAsset, folderFullPath);
            }

        }
    }
}
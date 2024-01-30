using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Amenonegames.DataClassGenerator.Editor
{
    public class GenerateCodeWindow : EditorWindow
    {
        private TextAsset textAsset;
        private string folderPath = ""; // フォルダパスを格納する変数

        private List<MonoScript> scripts = new List<MonoScript>();
        
        [MenuItem("Tools/GenerateCode/DataClassFromCsv")]
        private static void OpenWindow()
        {
            GetWindow<GenerateCodeWindow>("Generate Code");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Attach CSV", EditorStyles.boldLabel);
            textAsset =
                (TextAsset)EditorGUILayout.ObjectField("SourceCSV", textAsset, typeof(TextAsset), false);
            
            GUILayout.Space(10);
            // フォルダパスの選択
            GUILayout.Label("Select Output Folder", EditorStyles.boldLabel);
            if (GUILayout.Button("Select Folder"))
            {
                // 'Assets'フォルダのフルパスを取得
                string assetsPath = Application.dataPath;
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", assetsPath, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    // pathの区切り文字を置換
                    var separatorCharReplacedPath = selectedPath.Replace('/', Path.DirectorySeparatorChar);
                    folderPath = GetSubPathFromFolder(separatorCharReplacedPath, "Assets");
                }
            }
            // 選択されたフォルダパスを表示
            GUILayout.Label($"Folder Path :{folderPath}",EditorStyles.boldLabel);

            GUILayout.Space(10);
            // クラスまたはインターフェースの選択
            GUILayout.Label("Select Inheritance/Interface", EditorStyles.boldLabel);
            if (GUILayout.Button("Add New Type"))
            {
                scripts.Add(null);
            }

            for (int i = 0; i < scripts.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                scripts[i] = (MonoScript)EditorGUILayout.ObjectField("Inherit", scripts[i], typeof(MonoScript), false);
                if (GUILayout.Button("Remove"))
                {
                    scripts.RemoveAt(i);
                    i--;
                }
                EditorGUILayout.EndHorizontal();
            }
            
            // 選択されたタイプを表示
            //EditorGUILayout.PropertyField(typeProperty);
            GUILayout.Space(20);
            if (GUILayout.Button("Generate DataClass", GUILayout.Height(40)))
            {
                //textAssetがnullの場合は例外を投げる
                if (textAsset == null)
                {
                    throw new InvalidOperationException("TextAsset is null.");
                }
                if (!IsCsv(textAsset))
                {
                    throw new InvalidOperationException("TextAsset is not csv.");
                }
                
                // folderPathが空の場合は例外を投げる
                if (string.IsNullOrEmpty(folderPath))
                {
                    throw new InvalidOperationException("Folder path is empty.");
                }

                
                DataClassGenerator.Generate(textAsset, folderPath,scripts);
            }

        }
        
        private static string GetSubPathFromFolder(string fullPath, string targetFolder)
        {
            // パスをディレクトリ名の配列に分割
            var directories = fullPath.Split(Path.DirectorySeparatorChar);

            // 指定されたフォルダ名を含むインデックスを見つける
            int folderIndex = Array.IndexOf(directories, targetFolder);

            if (folderIndex <= 0)
            {
                // 指定されたフォルダ名が見つからなかった場合
                throw new InvalidOperationException("Target folder not found in the path.");
            }

            // 指定されたフォルダ名より上の部分を削除
            var subPathDirectories = directories.Skip(folderIndex);

            // 残りの部分を結合して返す
            return Path.Combine(subPathDirectories.ToArray());
        }
        
        private static bool IsCsv(TextAsset textAsset)
        {
            if (textAsset == null)
            {
                return false;
            }

            // TextAssetのパスを取得
            string path = AssetDatabase.GetAssetPath(textAsset);

            // パスの拡張子が.csvかどうかをチェック
            return path.EndsWith(".csv");
        }
    
    }
}
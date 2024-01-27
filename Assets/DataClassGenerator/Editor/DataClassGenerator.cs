using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DataClassGenerator
{
    public static class DataClassGenerator
    {

        public static void Generate(TextAsset asset , string folderFullPath)
        {
            var data = GetData(asset);
            var source = BuildClassStr(data.fileName, data.propertyNameClumns, data.typeStr);
            
            var folderPath = GetSubPathFromFolder(folderFullPath, "Assets");
            
            // filenameとfolderPathからファイルパスを生成
            string filePath = Path.Combine(folderPath, $"{data.fileName}.cs");
            
            File.WriteAllText(filePath, source);
            AssetDatabase.ImportAsset(filePath);
        }
        
        public static string GetSubPathFromFolder(string fullPath, string targetFolder)
        {
            // パスをディレクトリ名の配列に分割
            var directories = fullPath.Split("/");

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

        private static (string fileName, string[] propertyNameClumns , string[] typeStr) GetData( TextAsset asset )
        {
            var fileName = asset.name;
                            
            StringReader reader = new StringReader(asset.text);

            var firstLine = reader.ReadLine();
            var propertyNameClumns = firstLine.Split(',');
            string[] typeStr = new string[] { };
            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();
                if (line != null)
                {
                    var columns = line.Split(',');
                                
                    if (IsNativeType(columns[0].Trim()))
                    {
                        
                        typeStr = columns;
                        break;
                    }
                }
            }
            
            return (fileName, propertyNameClumns, typeStr);
        }

        private static void GenerateDirIfNotExists(string csvFilePath)
        {
            // Create Directory if not exists
            string directoryPath = "";
            directoryPath = Path.GetDirectoryName(csvFilePath);
            if (!Directory.Exists(directoryPath))
                if (directoryPath != null)
                    Directory.CreateDirectory(directoryPath);
        }

        private static string BuildClassStr(string fileName, string[] propertyNameClumns , string[] typeStr)
        {
            var builder = new StringBuilder();
            var namespaceName = ConvertToNamespace(fileName);
            
            builder.Append($@"
using UnityEngine;

namespace {namespaceName}
{{
    [System.Serializable]
    public class {fileName}
    {{");

            for (int i = 0; i < propertyNameClumns.Length; i++)
            {
                var propertyName = propertyNameClumns[i];
                var typeName = typeStr[i];
                var variableName = propertyName.ToLower();
                
                builder.Append($@"

        [SerializeField]
        private {typeName} _{variableName};
        public {typeName} {propertyName} 
        {{ 
            get => _{variableName}; 
            set => _{variableName} = value;
        }}");
                        
            }
                    
            builder.Append(@"
    }
}");
            return builder.ToString();
            
        }
    
        
        private static bool IsNativeType(string field)
        {
            string[] nativeTypes = new [] { "int", "uint" ,"float", "double", "bool", "string" ,"Vector2","Vector3" }; // 例
            return nativeTypes.Contains(field);
        }
        
        private static string ConvertToNamespace(string folderPath)
        {
            // "Assets/Scripts/"を取り除く
            string namespacePath = folderPath.Replace("Assets/Scripts/", "");

            // パスのスラッシュをドットに置換して名前空間形式にする
            string namespaceName = namespacePath.Replace("/", ".");

            return namespaceName;
        }
        
    }
}
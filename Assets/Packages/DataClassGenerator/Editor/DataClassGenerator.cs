﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Amenonegames.DataClassGenerator.Editor
{
    public static class DataClassGenerator
    {

        public static void Generate(TextAsset asset , string folderPath)
        {
            var data = GetData(asset);
            
            // filenameとfolderPathからファイルパスを生成
            string filePath = Path.Combine(folderPath, $"{data.fileName}.cs");
            var source = BuildClassStr(data.fileName , filePath , data.propertyNameClumns, data.typeStr);
            
            File.WriteAllText(filePath, source);
            AssetDatabase.ImportAsset(filePath);
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

        private static string BuildClassStr(string fileName, string filePath , string[] propertyNameClumns , string[] typeStr)
        {
            var builder = new StringBuilder();
            var namespaceName = ConvertToNamespace(filePath);

            builder.Append($@"
using UnityEngine;
");
            if (!string.IsNullOrEmpty(namespaceName))
            {
                builder.Append($@"
namespace {namespaceName}
{{
");
            }
            builder.Append($@"
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
    }");
            if (!string.IsNullOrEmpty(namespaceName))
            {
                builder.Append(@"
}");
            }

            return builder.ToString();
            
        }
    
        
        private static bool IsNativeType(string field)
        {
            string[] nativeTypes = new [] { "int", "uint" ,"float", "double", "bool", "string" ,"Vector2","Vector3" }; // 例
            return nativeTypes.Contains(field);
        }
        
        private static string ConvertToNamespace(string folderPath)
        {
            string removePath = Path.Combine("Assets", "Scripts");
            
            // "Assets/Scripts/"を取り除く
            string namespacePath = folderPath.Replace(removePath, "");
            
            // 末尾のファイル名部分を取り除く
            int lastSlashIndex = namespacePath.LastIndexOf(Path.DirectorySeparatorChar);
            if (lastSlashIndex >= 0)
            {
                namespacePath = namespacePath.Substring(0, lastSlashIndex);
            }

            // パスのスラッシュをドットに置換して名前空間形式にする
            string namespaceName = namespacePath.Replace(Path.DirectorySeparatorChar.ToString(), ".");

            return namespaceName;
        }
        
    }
}
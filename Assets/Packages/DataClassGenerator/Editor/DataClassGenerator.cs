using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Amenonegames.DataClassGenerator.Settings;
using NUnit.Framework;

namespace Amenonegames.DataClassGenerator.Editor
{
    public static class DataClassGenerator
    {

        public static void Generate(TextAsset asset , string folderPath , List<MonoScript> inherits)
        {
            var data = GetData(asset);
            
            // filenameとfolderPathからファイルパスを生成
            string filePath = Path.Combine(folderPath, $"{data.fileName}.cs");
            var source = BuildClassStr(data.fileName , filePath , data.propertyNameClumns, data.typeStr, inherits);
            
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
                                
                    if (ValidTypeJudge.IsValidTypeStr(columns[0].Trim()))
                    {
                        
                        typeStr = columns;
                        break;
                    }
                }
            }
            
            return (fileName, propertyNameClumns, typeStr);
        }

        private static string BuildClassStr(string fileName, string filePath , string[] propertyNameClumns , string[] typeStr , List<MonoScript> inherits)
        {
            var builder = new StringBuilder();
            var namespaceName = ConvertToNamespace(filePath);
            var inheritTypes = inherits.Select(x => x.GetClass());
            var enumerable = inheritTypes as Type[] ?? inheritTypes.ToArray();
            var usings = enumerable.Select(x => x.Namespace).Distinct().ToList();
            
            builder.Append($@"
using UnityEngine;");
            
            foreach (var usingName in usings)
            {
                builder.Append($@"
using {usingName};");
            }
            
            if (!string.IsNullOrEmpty(namespaceName))
            {
                builder.Append($@"

namespace {namespaceName}
{{
");
            }

            builder.Append($@"
    [System.Serializable]
    public class {fileName}");
            
            if(enumerable.Any())
            {
                builder.Append(" : ");
                for (int i = 0; i < enumerable.Count(); i++)
                {
                    var inheritType = enumerable.ElementAt(i);
                    builder.Append($"{inheritType.Name}");
                    if (i != enumerable.Count() - 1)
                    {
                        builder.Append(", ");
                    }
                }
            }
            
            builder.Append(@"
    {");

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
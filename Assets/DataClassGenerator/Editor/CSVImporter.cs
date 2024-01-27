using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace DataClassGenerator
{

    public class CsvAssetPostprocessor : AssetPostprocessor 
    {
        
        void OnPostprocessAllAssets (
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths) 
        {
            foreach (string asset in importedAssets) 
            {
                if (asset.EndsWith(".csv"))
                {
                    OnImportAsset(context);
                }
            }

            foreach (var asset in deletedAssets)
            {
                
            }
        }
        private void OnImportAsset(AssetImportContext ctx)
        {
            var text = File.ReadAllText(ctx.assetPath);
            TextAsset textAsset = new TextAsset(text);

            var csvPath = ctx.assetPath;
            var path = GetSubPathFromFolder(csvPath);
            
            DataClassGenerator.Generate(textAsset, path);
        }
        
        private void OnDeleteAsset(AssetImportContext ctx)
        {
            var csvPath = ctx.assetPath;
            var path = GetSubPathFromFolder(csvPath);
        }
        
        private string GetSubPathFromFolder(string fullPath)
        {
            string targetRootPath = DataClassDirectorySettings.DirectoryPath;
            string csvDirStructureCopyLimit = DataClassDirectorySettings.CsvDirStructureCopyLimit;
            
            if(string.IsNullOrEmpty(csvDirStructureCopyLimit))
                csvDirStructureCopyLimit = "Assets";
            
            // パスをディレクトリ名の配列に分割
            var directories = fullPath.Split(Path.DirectorySeparatorChar);

            // 指定されたフォルダ名を含むインデックスを見つける
            int folderIndex = Array.IndexOf(directories, csvDirStructureCopyLimit);

            if (folderIndex == -1)
            {
                // 指定されたフォルダ名が見つからなかった場合
                throw new InvalidOperationException("Target folder not found in the path.");
            }

            // 指定されたフォルダ名より上の部分を削除
            var subPathDirectories = directories.Skip(folderIndex);
            
            // 残りの部分を結合
            var csvPathStructure =  Path.Combine(subPathDirectories.ToArray());
            // 指定のrootPathに結合
            var path = Path.Combine(targetRootPath, csvPathStructure);
            
            return path;
        }
    }
}
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Packages.DataClassGenerator.Editor
{
    public class AssetDeleteWatcher: AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            // 指定のScriptableObjectアセットかどうかを確認
            if (Path.GetExtension(assetPath) == ".asset")
            {
                ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
                if (asset != null && asset is IOnDelete deleteAsset)
                {
                    // 削除される前に実行するメソッド
                    deleteAsset.OnDelete();
                }
            }
            // DidNotDeleteを返すとUnityのデフォルトの処理により削除が行われる
            return AssetDeleteResult.DidNotDelete;
        }
    }
}
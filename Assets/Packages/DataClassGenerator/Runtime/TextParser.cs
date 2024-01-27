using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEngine;

namespace Amenonegames.CSVDataParser
{
    public static class TextParser
    {

        private static CsvConfiguration DefaultConfig { get; } = new CsvConfiguration(new CultureInfo("ja-JP", false))
        {
            TrimOptions = TrimOptions.Trim, //前後の空白を削除
            HasHeaderRecord = true, //ヘッダーアリ
            Delimiter = ",", //区切り文字カンマ
            IgnoreBlankLines = true, //空白行を無視
            DetectColumnCountChanges = true, //異なる列数を検知
            AllowComments = true,
            ShouldSkipRecord = record => SkipRecord(record) //trueを返すとSkipする
        };
        
        private static bool SkipRecord(ShouldSkipRecordArgs args)
        {
            for (int i = 0; i < args.Row.Parser.Count; i++)
            {
                if (args.Row.TryGetField<string>(i, out var field))
                {
                    if (IsNativeType(field)) // 特定の文字列が含まれていればスキップ
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        private static bool IsNativeType(string field)
        {
            string[] nativeTypes = new [] { "int", "uint" ,"float", "double", "bool", "string" ,"Vector2","Vector3" }; // 例
            return nativeTypes.Contains(field);
        }
        
        /// <summary>
        /// 読み込みCSVがUTF-8でエンコードされていることを前提とする
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="config"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] ToModels<T>(TextAsset asset, CsvConfiguration config = null)
        {
            if (config == null)
            {
                config = DefaultConfig;
            }

            T[] models = null;
            try
            {
                //Shift-Jis対応時には必要
                //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Shift-JISを扱うためのおまじない
                using (var reader = new StringReader(asset.text))
                using (var csv = new CsvHelper.CsvReader(reader, config))
                {
                    models = csv.GetRecords<T>().ToArray();
                }
            }
            catch (CsvHelper.BadDataException ex)
            {
                Debug.LogError($"エラー：{ex.Context.Parser.RawRow}行の列数が異なります。[値：{ex.Context.Parser.RawRecord}]");
            }

            return models ;
        }
        
    }
}
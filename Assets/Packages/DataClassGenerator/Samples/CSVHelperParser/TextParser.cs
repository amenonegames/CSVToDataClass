using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEngine;

namespace Amenonegames.CSVHelperParser
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
            AllowComments = true
        };
        
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

            List<T> models = new List<T>();
            int currentRow = 0; // 行カウンタの初期化
            try
            {
                using (var reader = new StringReader(asset.text))
                using (var csv = new CsvHelper.CsvReader(reader, config))
                {
                    while (csv.Read())
                    {
                        currentRow++;  // 行カウンタをインクリメント
                        if (currentRow == 2) continue;  // 2行目をスキップ
                    
                        // データ処理のロジックをここに書く
                        var record = csv.GetRecord<T>();
                        models.Add(record);  // レコードをリストに追加
                    }
                }
            }
            catch (CsvHelper.BadDataException ex)
            {
                Debug.LogError($"エラー：{ex.Context.Parser.RawRow}行の列数が異なります。[値：{ex.Context.Parser.RawRecord}]");
            }

            return models.ToArray() ;
        }
        
    }
}
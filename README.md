Createメニュー＞CsvToData/Settings からScriptableObjectを生成し、
SaveAssetボタンを押すことでCSVを監視対象に加える。
該当CSVの編集は以降常に追従され、データクラスが自動生成される。

また、NugetのCSVHelper を導入し、SamplesのAdditional CSVHelper Parserを導入することで、専用のTextParserクラスを導入できる。
これはHeader以外の行に表示された型名の入力を、無視し、必要データだけを取り込むようにセッティングされている。

Install URL
https://github.com/amenonegames/CSVToDataClass.git?path=Assets/Packages/DataClassGenerator

# ScreptableObjectの項目
 - FilePath
   csvファイルのAssetsからのパスを入力する。
 - CSVSeparetor
   csvの区切り文字を一文字で入力
 - NameSpace
   Dataクラスを生成する時のNameSpaceを指定する
 - Serializable
   Trueの場合は、クラスがSerializableになり、各メンバがSerializeFieldとして生成される
 - InterfaceEnable
   trueの場合はInterfaceが生成される
 - InterfaceName
   Interfaceの名前を入力する。同じ名前のInterfaceが複数のcsvに設定されている場合、共通するプロパティのみを実装する。
 - Usings
   DataClassの生成時にusingの宣言が必要な場合は記入する。

# CSV形式のサンプル
| PropertyName1 | PropertyName2 | ... |
| ---- | ---- | ---- |
| PropertyType1 | PropertyType2 | ... |
| Value | Value | ... |
| Value | Value | ... |

# 生成されるコード

```csharp

    [System.Serializable]
    public class CSVFileName
    {

        [SerializeField]
        private PropertyType1	 _propertyname1; //All Char to Lower case
        public PropertyType1	 PropertyName1 
        { 
            get => _propertyname1; 
            set => _propertyname1 = value;
        }

        [SerializeField]
        private PropertyType2	 _propertyname2; //All Char to Lower case
        public PropertyType2	 PropertyName2 
        { 
            get => _propertyname2; 
            set => _propertyname2 = value;
        }

    }

```

# データの読み込み方法

```csharp

    public class User 
    {
        private void GetSampleData()
        {
            TextAsset csvRaw = Resources.Load<TextAsset>(CSVFile); //Load csv as TextAsset
            CSVFileName dataInClassInstance = TextParser.ToModels<CSVFileName>(csvRaw); //AutoGenerate class is named csv filename
            Debug.Log("Damage:" + data[0].Damage.ToString()); //Now you can use class instance.
        }
    }
```

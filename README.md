Menu>Tool>GenerateCodeで利用
CSVと、出力先フォルダを入力してGenerateすると、CSVの内容に応じたクラスを自動生成する。
また、NugetのCSVHelper を導入し、SamplesのAdditional CSVHelper Parserを導入することで、専用のTextParserクラスを導入できる。
これはHeader以外の行に表示された型名の入力を、無視し、必要データだけを取り込むようにセッティングされている。

現在の対応型は以下の通り
"int", "uint" ,"float", "double", "bool", "string" , "Vector2" , "Vector3"

Install URL
https://github.com/amenonegames/DataClassGeneratorEditor.git?path=Assets/Packages/DataClassGenerator

CSV形式のサンプル
| PropertyName1 | PropertyName2 | ... |
| ---- | ---- | ---- |
| PropertyType1 | PropertyType2 | ... |
| Value | Value | ... |
| Value | Value | ... |

展開されるクラスのサンプル

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

データの読み込み方法

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

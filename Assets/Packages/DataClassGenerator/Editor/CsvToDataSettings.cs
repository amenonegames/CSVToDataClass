using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Packages.DataClassGenerator.Editor
{
    
    [CreateAssetMenu(fileName = "CsvToDataSettings", menuName = "CsvToData/Settings")]
    public class CsvToDataSettings : ScriptableObject
    {
        public List<CsvToDataSetting> Settings = new List<CsvToDataSetting>();

        [NonSerialized] public string[] prevRootPaths = new string[0];
    }
    
    [System.Serializable]
    public class CsvToDataSetting
    {
        [Tooltip("データクラスを生成したいAssembly名を指定")]
        public string AssemblyName;
        [Tooltip("Assetsからの相対位置でファイルパスを指定")]
        public string FilePath;
        public string CSVSeparator ;
        public string NameSpace;
        public bool Serializable ;
        public bool InterfaceEnable ;
        public string InterfaceName;
        public string[] Usings ;
        
    }
    

}
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Packages.DataClassGenerator.Editor
{
    
    [CreateAssetMenu(fileName = "CsvToDataSettingsAsset", menuName = "CsvToData/CsvToDataSettingsAsset")]
    public class CsvToDataSettingsAsset : ScriptableObject , IOnDelete
    {
        public CsvToDataSettings Settings = new ();

        [SerializeField,HideInInspector] public string[] prevFilePaths = new string[0];
        [SerializeField,HideInInspector] public bool hasUnsavedChanges = false;
        public void OnDelete()
        {
            SaveSettigFiles.DeleteSettingsJson();
            Debug.Log("delete settings json");
            SaveSettigFiles.RemoveFromRspFile(prevFilePaths,true);
            Debug.Log("remove file address from rsp file");
        }
    }

    [System.Serializable]
    public class CsvToDataSettings : IList<CsvToDataSetting>
    {
        public List<CsvToDataSetting> Settings = new List<CsvToDataSetting>();
        private bool _isReadOnly;

        public IEnumerator<CsvToDataSetting> GetEnumerator()
        {
            return Settings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(CsvToDataSetting item)
        {
            Settings.Add(item);
        }

        public void Clear()
        {
            Settings.Clear();
        }

        public bool Contains(CsvToDataSetting item)
        {
            return Settings.Contains(item);
        }

        public void CopyTo(CsvToDataSetting[] array, int arrayIndex)
        {
            Settings.CopyTo(array, arrayIndex);
        }

        public bool Remove(CsvToDataSetting item)
        {
            return Settings.Remove(item);
        }

        public int Count => Settings.Count;

        public bool IsReadOnly => _isReadOnly;

        public int IndexOf(CsvToDataSetting item)
        {
            return Settings.IndexOf(item);
        }

        public void Insert(int index, CsvToDataSetting item)
        {
            Settings.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Settings.RemoveAt(index);
        }

        public CsvToDataSetting this[int index]
        {
            get => Settings[index];
            set => Settings[index] = value;
        }
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

    internal interface IOnDelete
    {
        void OnDelete();
    }

}
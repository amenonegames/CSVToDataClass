﻿using System.Collections.Generic;
using System.Linq;
using Packages.DataClassGenerator.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CsvToDataSettingsAsset))]
public class CsvToDataSettingsEditor : Editor
{
    private ReorderableList reorderableList;
    private new bool hasUnsavedChanges = false;
    private string prefsKey;

    private void OnEnable()
    {
        prefsKey = $"{target.GetInstanceID()}_hasUnsavedChanges";
        hasUnsavedChanges = EditorPrefs.GetBool(prefsKey, false);

        serializedObject.Update();
        reorderableList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("Settings").FindPropertyRelative("Settings"),
            true, true, true, true);

        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "CSV To Data Settings");
        };

        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            float singleFieldHeight = EditorGUIUtility.singleLineHeight;
            float padding = 2f;

            var assemblyName = element.FindPropertyRelative("AssemblyName");
            var filePath = element.FindPropertyRelative("FilePath");
            var csvSeparator = element.FindPropertyRelative("CSVSeparator");
            var nameSpace = element.FindPropertyRelative("NameSpace");
            var serializable = element.FindPropertyRelative("Serializable");
            var interfaceEnable = element.FindPropertyRelative("InterfaceEnable");
            var interfaceName = element.FindPropertyRelative("InterfaceName");
            var usings = element.FindPropertyRelative("Usings");

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, singleFieldHeight),
                assemblyName, new GUIContent("Assembly Name"));
            rect.y += singleFieldHeight + padding;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, singleFieldHeight),
                filePath, new GUIContent("File Path"));
            rect.y += singleFieldHeight + padding;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, singleFieldHeight),
                csvSeparator, new GUIContent("CSV Separator"));
            rect.y += singleFieldHeight + padding;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, singleFieldHeight),
                nameSpace, new GUIContent("Namespace"));
            rect.y += singleFieldHeight + padding;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, singleFieldHeight),
                serializable, new GUIContent("Serializable"));
            rect.y += singleFieldHeight + padding;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, singleFieldHeight),
                interfaceEnable, new GUIContent("Interface Enable"));
            rect.y += singleFieldHeight + padding;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, singleFieldHeight),
                interfaceName, new GUIContent("Interface Name"));
            rect.y += singleFieldHeight + padding;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(usings)),
                usings, new GUIContent("Usings"), true);
            if (EditorGUI.EndChangeCheck())
            {
                hasUnsavedChanges = true;
                EditorPrefs.SetBool(prefsKey, true);
            }
        };

        reorderableList.elementHeightCallback = (index) =>
        {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            float singleFieldHeight = EditorGUIUtility.singleLineHeight;
            float padding = 2f;
            float usingsHeight = EditorGUI.GetPropertyHeight(element.FindPropertyRelative("Usings"), true);
            return (singleFieldHeight + padding) * 7 + usingsHeight + padding;
        };

        reorderableList.onAddCallback = (ReorderableList list) =>
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);

            element.FindPropertyRelative("AssemblyName").stringValue = "Assembly-CSharp";
            element.FindPropertyRelative("FilePath").stringValue = "Assets/Path/To/CSV";
            element.FindPropertyRelative("CSVSeparator").stringValue = ",";
            element.FindPropertyRelative("NameSpace").stringValue = "Namespace";
            element.FindPropertyRelative("Serializable").boolValue = true;
            element.FindPropertyRelative("InterfaceEnable").boolValue = false;
            element.FindPropertyRelative("InterfaceName").stringValue = "";
            element.FindPropertyRelative("Usings").ClearArray();

            hasUnsavedChanges = true;
            EditorPrefs.SetBool(prefsKey, true);
        };

        reorderableList.onRemoveCallback = (ReorderableList list) =>
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
            hasUnsavedChanges = true;
            EditorPrefs.SetBool(prefsKey, true);
        };

        // 初期状態の保存
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        // hasUnsavedChangesが正しく反映されるようにする
        if (GUI.changed)
        {
            hasUnsavedChanges = true;
            EditorPrefs.SetBool(prefsKey, true);
        }

        if (hasUnsavedChanges)
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 16;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.normal.background = MakeTex(2, 2, new Color(0.9f,0.1f,0.1f,1));

            GUILayout.Space(10);
            if (GUILayout.Button("▶PUSH CHANGES TO SOURCE GEN", buttonStyle, GUILayout.Height(40)))
            {
                SaveChanges();
            }
        }
    }

    public override void SaveChanges()
    {
        hasUnsavedChanges = false;
        EditorPrefs.SetBool(prefsKey, false);
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        // 編集している CsvToDataSettings インスタンスを取得
        CsvToDataSettingsAsset settingsAsset = (CsvToDataSettingsAsset)target;
        CsvToDataSettings settings = settingsAsset.Settings;
        
        RemoveCheck(settingsAsset);
        SaveSettigFiles.SaveSettingsToJson(settings);
        SaveSettigFiles.AppendToRspFile(settings);
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("Changes saved.");
    }
    
    private void RemoveCheck(CsvToDataSettingsAsset settingsAsset)
    {
        if (settingsAsset.prevFilePaths is null) return;
        CsvToDataSettings settings = settingsAsset.Settings;
        List<string> removedRootPath = new List<string>();
        foreach (var prevFilePath in settingsAsset.prevFilePaths)
        {
            bool isPrevFileStayCurrent = false;
            foreach (var currentSetting in settings.Settings)
            {
                if(currentSetting.FilePath == prevFilePath) isPrevFileStayCurrent = true;
            }
            
            if( isPrevFileStayCurrent == false)
                removedRootPath.Add(prevFilePath);
        }
                    
        if(removedRootPath.Count > 0)
            SaveSettigFiles.RemoveFromRspFile(removedRootPath);
        
        settingsAsset.prevFilePaths = settings.Settings.Select(s => s.FilePath).ToArray();
    }
    
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}

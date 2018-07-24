using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EditorTools : Editor {

	[MenuItem("Tool/SetAssetBundleName")]
    static void SetAssetBundleName()
    {
        string[] strs = Selection.assetGUIDs;
        string str = AssetDatabase.GUIDToAssetPath(strs[0]);
        string _DirName = Path.GetFileName(str);
        Debug.Log(_DirName);
        var files = Directory.GetFiles(str);
        foreach (var file in files)
        {
            if (file.Contains(".meta"))
                continue;

            AssetImporter asset = AssetImporter.GetAtPath(file);
            asset.assetBundleName = _DirName;
            asset.SaveAndReimport();
        }
        AssetDatabase.Refresh();
    }
}

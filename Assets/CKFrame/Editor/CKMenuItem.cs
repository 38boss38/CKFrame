using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CKMenuItem
{
    [MenuItem("CKFrame/打开存档路径")]
    public static void OpenArchivedDirPath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
    [MenuItem("CKFrame/打开框架文档")]
    public static void OpenDoc()
    {
        Application.OpenURL("http://www.yfjoker.com/JKFrame/");
    }
}

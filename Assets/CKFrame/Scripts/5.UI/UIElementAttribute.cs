using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI元素的特性
/// 每个UI窗口都应该添加
/// </summary>
[AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
public class UIElementAttribute : Attribute
{

    public bool isChche;
    public string resPath;
    public int layerNum;
    
    public UIElementAttribute(bool isChche, string resPath, int layerNum)
    {
        this.isChche = isChche;
        this.resPath = resPath;
        this.layerNum = layerNum;
    }
}

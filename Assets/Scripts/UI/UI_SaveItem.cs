using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CKFrame;

[Pool]
public class UI_SaveItem : MonoBehaviour
{
    private SaveItem saveItem;
    public void Init(SaveItem saveItem)
    {
        this.saveItem = saveItem;
    }

    public void Destroy()
    {
        saveItem = null;
        this.CKGameObjectPushPool();
    }
}

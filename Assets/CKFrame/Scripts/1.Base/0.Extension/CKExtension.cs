using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// CKFrame框架主要的拓展方法
/// </summary>
public static class CKExtension
{
    #region 通用
    /// <summary>
    /// 获取特性
    /// </summary>
    public static T GetAttribute<T>(this object obj) where T: Attribute
    {
       return obj.GetType().GetCustomAttribute<T>();
    }
    /// <summary>
    /// 获取特性
    /// </summary>
    /// <param name="type">特性所在的类型</param>
    /// <returns></returns>
    public static T GetAttribute<T>(this object obj, Type type) where T: Attribute
    {
        return type.GetCustomAttribute<T>();
    }
    
    /// <summary>
    /// 数组相等对比
    /// </summary>
    public static bool ArrayEquals(this object[] objs, object[] other)
    {
        if (other==null || objs.GetType()!=other.GetType())
        {
            return false;
        }
        if (objs.Length == other.Length)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (!objs[i].Equals(other[i]))
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }
    
    #endregion

    #region 资源管理
    /// <summary>
    /// GameObject放入对象池
    /// </summary>
    public static void CKGameObjectPushPool(this GameObject go)
    {
        PoolManager.Instance.PushGameObject(go);
    }
    
    /// <summary>
    /// GameObject放入对象池
    /// </summary>
    public static void CKGameObjectPushPool(this Component com)
    {
        CKGameObjectPushPool(com.gameObject);
    }
    
    /// <summary>
    /// 普通类放进池子
    /// </summary>
    public static void CKObjcetPushPool(this object obj)
    {
        PoolManager.Instance.PushObject(obj);
    }

    #endregion
}

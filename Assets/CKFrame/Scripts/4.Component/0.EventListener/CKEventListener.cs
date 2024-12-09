using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CKFrame
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum CKEventType
    {
        OnMouseEnter,
        OnMouseExit,
        OnClick,
        OnClickDown,
        OnClickUp,
        OnDrag,
        OnBeginDrag,
        OnEndDrag,
        OnCollisionEnter,
        OnCollisionStay,
        OnCollisionExit,
        OnCollisionEnter2D,
        OnCollisionStay2D,
        OnCollisionExit2D,
        OnTriggerEnter,
        OnTriggerStay,
        OnTriggerExit,
        OnTriggerEnter2D,
        OnTriggerStay2D,
        OnTriggerExit2D,
    }

    public interface IMouseEvent : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler,
        IPointerUpHandler, IBeginDragHandler, IDragHandler
    {
    }

    /// <summary>
    /// 事件工具
    /// 可以添加 鼠标、碰撞、触发等事件
    /// </summary>
    public class CKEventListener : MonoBehaviour, IMouseEvent
    {
        #region 内部类、接口等

        /// <summary>
        /// 某个事件中一个时间的数据包装类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class CKEventListenerEventInfo<T>
        {
            // T: 事件本身的参数(PointerEventData、Collision)
            // object[]:事件的参数
            public Action<T, object[]> action;
            public object[] args;

            public void Init(Action<T, object[]> action, object[] args)
            {
                this.action = action;
                this.args = args;
            }

            public void Destroy()
            {
                this.action = null;
                this.args = null;
                this.CKObjcetPushPool();
            }

            public void TriggerEvent(T eventData)
            {
                action?.Invoke(eventData, args);
            }
        }

        interface ICKEventListenerEventInfos
        {
            void RemoveAll();
        }

        /// <summary>
        /// 一类事件的数据包装类型：包含多个CKEventListenerEventInfo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class CKEventListenerEventInfos<T> : ICKEventListenerEventInfos
        {
            // 所有的事件
            private List<CKEventListenerEventInfo<T>> eventList = new List<CKEventListenerEventInfo<T>>();

            /// <summary>
            /// 添加事件
            /// </summary>
            public void AddListener(Action<T, object[]> action, params object[] args)
            {
                CKEventListenerEventInfo<T> info = PoolManager.Instance.GetObject<CKEventListenerEventInfo<T>>();
                info.Init(action, args);
                eventList.Add(info);
            }

            /// <summary>
            /// 移除事件
            /// </summary>
            public void RemoveListenter(Action<T, object[]> action, bool checkArgs = false, params object[] args)
            {
                for (int i = 0; i < eventList.Count; i++)
                {
                    // 找到这个事件
                    if (eventList[i].action.Equals(action))
                    {
                        // 是否需要检查参数
                        if (checkArgs && args.Length > 0)
                        {
                            // 参数如果相等
                            if (args.ArrayEquals(eventList[i].args))
                            {
                                // 移除
                                eventList[i].Destroy();
                                eventList.RemoveAt(i);
                                return;
                            }
                        }
                        else
                        {
                            // 移除
                            eventList[i].Destroy();
                            eventList.RemoveAt(i);
                            return;
                        }
                    }
                }
            }

            /// <summary>
            /// 移除全部，全部放进对象池
            /// </summary>
            public void RemoveAll()
            {
                for (int i = 0; i < eventList.Count; i++)
                {
                    eventList[i].Destroy();
                }

                eventList.Clear();
                this.CKObjcetPushPool();
            }

            public void TriggerEvent(T eventData)
            {
                for (int i = 0; i < eventList.Count; i++)
                {
                    eventList[i].TriggerEvent(eventData);
                }
            }
        }

        /// <summary>
        /// 枚举比较器
        /// </summary>
        private class CKEventTypeEnumComparer : Singleton<CKEventTypeEnumComparer>, IEqualityComparer<CKEventType>
        {
            public bool Equals(CKEventType x, CKEventType y)
            {
                return x == y;
            }

            public int GetHashCode(CKEventType obj)
            {
                return (int)obj;
            }
        }

        #endregion

        private Dictionary<CKEventType, ICKEventListenerEventInfos> eventInfoDic =
            new Dictionary<CKEventType, ICKEventListenerEventInfos>(CKEventTypeEnumComparer.Instance);

        #region 外部的访问

        /// <summary>
        /// 添加事件
        /// </summary>
        public void AddListener<T>(CKEventType eventType, Action<T, object[]> action,
            params object[] args)
        {
            if (eventInfoDic.ContainsKey(eventType))
            {
                (eventInfoDic[eventType] as CKEventListenerEventInfos<T>).AddListener(action, args);
            }
            else
            {
                CKEventListenerEventInfos<T> infos = PoolManager.Instance.GetObject<CKEventListenerEventInfos<T>>();
                infos.AddListener(action, args);
                eventInfoDic.Add(eventType, infos);
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        public void RemoveListener<T>(CKEventType eventType, Action<T, object[]> action, bool checkArgs = false,
            params object[] args)
        {
            if (eventInfoDic.ContainsKey(eventType))
            {
                (eventInfoDic[eventType] as CKEventListenerEventInfos<T>).RemoveListenter(action, checkArgs, args);
            }
        }

        /// <summary>
        /// 移除某一个事件类型下的全部事件
        /// </summary>
        public void RemoveAllListener(CKEventType eventType)
        {
            if (eventInfoDic.ContainsKey(eventType))
            {
                eventInfoDic[eventType].RemoveAll();
                eventInfoDic.Remove(eventType);
            }
        }

        /// <summary>
        /// 移除全部事件
        /// </summary>
        public void RemoveAllListener()
        {
            foreach (ICKEventListenerEventInfos infos in eventInfoDic.Values)
            {
                infos.RemoveAll();
            }

            eventInfoDic.Clear();
        }

        #endregion

        /// <summary>
        /// 触发事件
        /// </summary>
        private void TriggerAction<T>(CKEventType eventType, T eventData)
        {
            if (eventInfoDic.ContainsKey(eventType))
            {
                (eventInfoDic[eventType] as CKEventListenerEventInfos<T>).TriggerEvent(eventData);
            }
        }

        #region 鼠标事件

        public void OnPointerEnter(PointerEventData eventData)
        {
            TriggerAction(CKEventType.OnMouseEnter, eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TriggerAction(CKEventType.OnMouseExit, eventData);
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            TriggerAction(CKEventType.OnBeginDrag, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            TriggerAction(CKEventType.OnDrag, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            TriggerAction(CKEventType.OnEndDrag, eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            TriggerAction(CKEventType.OnClick, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            TriggerAction(CKEventType.OnClickDown, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            TriggerAction(CKEventType.OnClickUp, eventData);
        }

        #endregion

        #region 碰撞事件

        private void OnCollisionEnter(Collision collision)
        {
            TriggerAction(CKEventType.OnCollisionEnter, collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            TriggerAction(CKEventType.OnCollisionStay, collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            TriggerAction(CKEventType.OnCollisionExit, collision);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TriggerAction(CKEventType.OnCollisionEnter2D, collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            TriggerAction(CKEventType.OnCollisionStay2D, collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            TriggerAction(CKEventType.OnCollisionExit2D, collision);
        }

        #endregion

        #region 触发事件

        private void OnTriggerEnter(Collider other)
        {
            TriggerAction(CKEventType.OnTriggerEnter, other);
        }

        private void OnTriggerStay(Collider other)
        {
            TriggerAction(CKEventType.OnTriggerStay, other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerAction(CKEventType.OnTriggerExit, other);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TriggerAction(CKEventType.OnTriggerEnter2D, collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            TriggerAction(CKEventType.OnTriggerStay2D, collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            TriggerAction(CKEventType.OnTriggerExit2D, collision);
        }

        #endregion
    }
}
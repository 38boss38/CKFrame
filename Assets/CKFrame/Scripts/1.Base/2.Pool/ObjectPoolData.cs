using System.Collections.Generic;

namespace CKFrame
{
    /// <summary>
    /// 普通类 对象池数据
    /// </summary>
    public class ObjectPoolData
    {
        // 对象容器
        public Queue<object> poolQueue = new Queue<object>();

        public ObjectPoolData(object obj)
        {
            PushObj(obj);
        }
    
        /// <summary>
        /// 将对象放进对象池
        /// </summary>
        public void PushObj(object obj)
        {
            poolQueue.Enqueue(obj);
        }

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        public object GetObj()
        {
            return poolQueue.Dequeue();
        }
    }
}


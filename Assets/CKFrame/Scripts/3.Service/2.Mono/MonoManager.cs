using System;

public class MonoManager : ManagerBase<MonoManager>
{
    private Action updateEvent;
    
    /// <summary>
    /// 添加Update监听
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(Action action)
    {
        updateEvent += action;
    }
    
    /// <summary>
    /// 移除Update监听
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(Action action)
    {
        updateEvent -= action;
    }
    private void Update()
    {
        updateEvent?.Invoke();
    }
    
}

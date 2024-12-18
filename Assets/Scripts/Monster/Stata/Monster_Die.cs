using System.Collections;
using System.Collections.Generic;
using CKFrame;
using UnityEngine;

public class Monster_Die : Monster_StateBase
{
    private Coroutine dieCoroutine;
    public override void Enter()
    {
        // 修改移动状态
        SetMoveState(false);
        // 播放动画
        PlayerAnimation("Die");
        // 让玩家得分以及通知怪物管理器
        EventManager.EventTrigger("MonsterDie");
        
        dieCoroutine = this.StartCoroutine(Die());
    }

    public override void Exit()
    {
        if (dieCoroutine!=null)
        {
            this.StopCoroutine(dieCoroutine);
            dieCoroutine = null;
        }
    }

    private IEnumerator Die()
    {
        // 等待1秒，确保死亡动画播放完成
        yield return new WaitForSeconds(1.0f);
        
        // 尸体下沉效果参数
        float duration = 2.0f; // 下沉持续时间
        float sinkSpeed = 0.05f; // 下沉速度
        float elapsedTime = 0f;
        Vector3 startPosition = monster.transform.position;

        while (elapsedTime < duration)
        {
            // 每帧下沉一小段距离
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(startPosition.y, startPosition.y - 2.0f, elapsedTime / duration); // 插值计算
            monster.transform.position = new Vector3(startPosition.x, newY, startPosition.z);
            yield return null; // 等待下一帧
        }

        // 确保位置精确设置到最终目标
        monster.transform.position = new Vector3(startPosition.x, startPosition.y - 2.0f, startPosition.z);
        
        // 再等待1秒，然后调用死亡销毁逻辑
        yield return new WaitForSeconds(1.0f);
        monster.Die();
    }

    
}

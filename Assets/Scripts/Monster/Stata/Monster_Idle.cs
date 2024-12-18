using System.Collections;
using System.Collections.Generic;
using CKFrame;
using UnityEngine;

public class Monster_Idle : Monster_StateBase
{
    private Coroutine goPatrolCoroutine;
    public override void Enter()
    {
        // 修改移动状态
        SetMoveState(false);
        // 播放动画
        PlayerAnimation("Idle");
        // 延迟一个随机的时间去巡逻
        goPatrolCoroutine = this.StartCoroutine(GoPatrol(Random.Range(1f, 5f)));
    }

    private IEnumerator GoPatrol(float time)
    {
        yield return new WaitForSeconds(time);
        stateMachine.ChangeState<Monster_Patrol>((int)MonsterStateType.Patrol);
    }

    public override void Exit()
    {
        if (goPatrolCoroutine != null)
        {
            goPatrolCoroutine.StopAllCoroutines();
        }
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        // 检测和玩家的距离是否要追击
        CheckFollowAndChangeState();

    }
}

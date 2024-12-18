using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Patrol : Monster_StateBase
{
    // 巡逻的目标点
    public Vector3 target;

    public override void Enter()
    {
        // 修改移动状态
        SetMoveState(true);
        // 播放动画
        PlayerAnimation("Run");
        // 获取巡逻点
        target = monster.GetPatrolTarget();
        navMeshAgent.SetDestination(target);
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        
        // 检测和玩家的距离是否要追击
        if (CheckFollowAndChangeState())
        {
            // 如果切换去追击，就无视后面的巡逻逻辑
            return;
        }

        if (Vector3.Distance(monster.transform.position, target) < 2f)
        {
            stateMachine.ChangeState<Monster_Idle>((int)MonsterStateType.Idle);
        }
        
        
    }
}
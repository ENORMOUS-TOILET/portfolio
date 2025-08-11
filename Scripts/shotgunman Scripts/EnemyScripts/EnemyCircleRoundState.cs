using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircleRoundState : EnemyState
{
    public EnemyCircleRoundState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("进入周旋状态");
        enemy.SetCircleRoundPoint();
        enemy.currentTarget = enemy.circleRoundPosition;
        stateTimer = enemy.circleRoundTime;
    }

    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(enemy.transform.position,enemy.circleRoundPosition) < 0.5f || enemy.distanceToPlayer > 20)
        {
            enemy.SetCircleRoundPoint();
            enemy.currentTarget = enemy.circleRoundPosition;
        }
        enemy.MoveToPathPoint();
        enemy.FacePlayer();
        //Debug.Log("周旋时间还剩余" + stateTimer);


        if (stateTimer < 0)
            stateMachien.ChangeState(enemy.chaseState);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("敌人退出周旋状态");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPrepareState : EnemyState
{


    //绝对玩家移动到右侧或左侧
    private bool moveToRight;

    public EnemyAttackPrepareState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        moveToRight = Random.value > 0.5f;
        Debug.Log("进入攻击准备阶段");
    }

    public override void Exit()
    {
        base.Exit();
        enemy.reachSide = false;
    }

    public override void Update()
    {
        base.Update();
        enemy.FacePlayer();


        if (moveToRight)
            enemy.MoveToPlayerRightSide();
        else
            enemy.MoveToPlayerLeftSide();

        if (enemy.reachSide || enemy.distanceToPlayer < 2f)
            stateMachien.ChangeState(enemy.attackState);
    }
}

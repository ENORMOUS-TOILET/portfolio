using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("½øÈë×·»÷×´Ì¬");

        enemy.currentTarget = enemy.playerGO.transform.position;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.FacePathPoint();
        enemy.MoveToPathPoint();
        enemy.currentTarget = enemy.playerGO.transform.position;

        if (enemy.distanceToPlayer < enemy.attackDetermineRange)
            stateMachien.ChangeState(enemy.attackState);

        if (enemy.distanceToPlayer > enemy.maxPatrolDistance)
        {
            stateMachien.ChangeState(enemy.patrolState);
        }
    }
}

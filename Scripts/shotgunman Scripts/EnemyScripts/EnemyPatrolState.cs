using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    public EnemyPatrolState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy is in patrol state");
    }

    public override void Update()
    {
        base.Update();
        enemy.FacePathPoint();
        enemy.MoveToPathPoint();
        enemy.changeRandomPatrolPoint();
        enemy.CalculateDistanceToPatrolPoint();
        enemy.currentTarget = enemy.patrolPointList[enemy.currentPatrolIndex];

        if (enemy.distanceToPlayer < enemy.chaseDeterminRange)
        {
            stateMachien.ChangeState(enemy.chaseState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}

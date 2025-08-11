using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.9f;
        enemy.rb.velocity = Vector2.zero;
        //Debug.Log("¹¥»÷");
        enemy.FacePlayer();
        enemy.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachien.ChangeState(enemy.circleRoundState);
    }


    public override void Exit()
    {
        base.Exit();
        //Debug.Log("¹¥»÷½áÊø");
        enemy.rb.constraints = RigidbodyConstraints2D.None;
    }

}


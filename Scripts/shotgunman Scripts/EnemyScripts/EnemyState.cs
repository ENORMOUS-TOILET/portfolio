using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachien;
    protected Enemy enemy;


    protected bool triggerCalled;
    protected AnimationClip currentAnimationClip;
    private string animBoolName;
    protected float stateTimer;

    public EnemyState(Enemy _enemy,EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemy = _enemy;
        this.stateMachien = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        //Debug.Log("进入了" + animBoolName + "状态");
        enemy.animator.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        if (enemy.isDead)
            stateMachien.ChangeState(enemy.deadState);
    }

    public virtual void Exit()
    {
        //Debug.Log("退出了" + animBoolName + "状态");
        enemy.animator.SetBool(animBoolName, false);
    }
}

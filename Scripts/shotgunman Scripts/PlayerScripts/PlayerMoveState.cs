using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.StartWalkAudio();
    }


    public override void Update()
    {
        base.Update();


        //Vector2 moveDirection = new Vector2(xInput, yInput).normalized;
        player.SetVelocity(player.moveDirection);

        player.Shoot(player.shotgunBulletGO);

        player.FaceMouse();

        if (xInput == 0 && yInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        player.CheckDashInput();
    }

    public override void Exit()
    {
        base.Exit();
        player.StopWalkAudio();
    }
}

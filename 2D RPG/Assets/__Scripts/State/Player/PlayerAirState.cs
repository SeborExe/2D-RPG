using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private float slowdownMovementInAir = 0.75f;

    public PlayerAirState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.WallSlideState);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.IdleState);

        if (xInput != 0)
            player.SetVelocity(player.MoveSpeed * slowdownMovementInAir * xInput, player.Rigidbody2D.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

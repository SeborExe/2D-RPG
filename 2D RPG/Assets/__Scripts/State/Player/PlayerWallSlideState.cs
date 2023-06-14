using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    private float slowdownWhenSlide = 0.6f;

    public PlayerWallSlideState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected() == false)
            stateMachine.ChangeState(player.AirState);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.WallJumpState);
            return;
        }

        if (xInput != 0 && player.FacingDir != xInput)
            stateMachine.ChangeState(player.IdleState);

        if (yInput < 0)
            player.Rigidbody2D.velocity = new Vector2(0, player.Rigidbody2D.velocity.y);
        else
            player.Rigidbody2D.velocity = new Vector2(0, player.Rigidbody2D.velocity.y * slowdownWhenSlide);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.IdleState);


    }

    public override void Exit()
    {
        base.Exit();
    }
}

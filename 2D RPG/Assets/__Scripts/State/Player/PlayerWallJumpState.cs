using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 1f;
        player.SetVelocity(5 * -player.FacingDir, player.JumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(player.AirState);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

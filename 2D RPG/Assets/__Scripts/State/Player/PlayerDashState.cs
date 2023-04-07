using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        dashTimer = player.DashDuration;
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.DashSpeed * player.DashDir, 0);

        if (dashTimer <= 0f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0f, player.Rigidbody2D.velocity.y);
    }
}

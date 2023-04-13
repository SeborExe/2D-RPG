using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.Rigidbody2D.velocity = new Vector2(player.Rigidbody2D.velocity.x, player.JumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (player.Rigidbody2D.velocity.y < 0)
        {
            stateMachine.ChangeState(player.AirState);
        }

        if (xInput != 0)
            player.SetVelocity(player.MoveSpeed * player.JumpMovementSlowdown * xInput, player.Rigidbody2D.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

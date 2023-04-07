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

        player.Rigidbody2D.velocity = new Vector2(player.Rigidbody2D.velocity.x, 12f);
    }

    public override void Update()
    {
        base.Update();

        if (player.Rigidbody2D.velocity.y < 0)
        {
            stateMachine.ChangeState(player.AirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.Rigidbody2D.velocity = Vector2.zero;
    }

    public override void Update()
    {
        base.Update();

        if (xInput == 0)
            player.Rigidbody2D.velocity = new Vector2(0, player.Rigidbody2D.velocity.y);

        if (xInput == player.FacingDir && player.IsWallDetected())
            return;

        if (xInput != 0 && !player.isBusy)
            stateMachine.ChangeState(player.MoveState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

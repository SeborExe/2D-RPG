using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.Instance.PlaySFX(14, null);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.MoveSpeed, player.Rigidbody2D.velocity.y);

        if (xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.IdleState);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.Instance.StopSFX(14);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    private int busyAfterCatchSwordInMiliseconds = 100;

    public PlayerCatchSwordState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.Sword.transform;

        player.EntityFX.PlayDustFX();

        FacePlayerToSwordDirection();

        player.Rigidbody2D.velocity = new Vector2(player.SwordReturnImpact * -player.FacingDir, player.Rigidbody2D.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }

    public async override void Exit()
    {
        base.Exit();

        await player.BusyFor(busyAfterCatchSwordInMiliseconds);
    }

    private void FacePlayerToSwordDirection()
    {
        if (player.transform.position.x > sword.position.x && player.FacingDir == 1)
            player.Flip();
        else if (player.transform.position.x < sword.position.x && player.FacingDir == -1)
            player.Flip();
    }
}

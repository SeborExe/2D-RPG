using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        PlayerManager.Instance.InvokeOnPlayerDie();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

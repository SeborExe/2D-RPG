using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 1.5f;
    private int busyTimeAfterAttackInMiliseconds = 150;

    public PlayerPrimaryAttackState(PlayerStateMachine stateMachine, Player player, int animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.Animator.SetInteger(Resources.ComboCounter, comboCounter);

        float attackDir = player.FacingDir;
        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        float slideTimeWhenAttack = 0.1f;
        stateTimer = slideTimeWhenAttack;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0f)
            player.Rigidbody2D.velocity = Vector2.zero;

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }

    public async override void Exit()
    {
        base.Exit();

        await player.BusyFor(busyTimeAfterAttackInMiliseconds);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private EnemySkeleton enemy;
    private float blinkRepeatTime = 0.1f;

    public SkeletonStunnedState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemySkeleton enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.EntityFX.InvokeRepeating(nameof(enemy.EntityFX.RedColorBlink), 0, blinkRepeatTime);

        stateTimer = enemy.stunTime;
        enemy.Rigidbody2D.velocity = new Vector2(-enemy.FacingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(enemy.IdleState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.EntityFX.CancelColorChange();
    }
}

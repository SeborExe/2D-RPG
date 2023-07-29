using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    protected EnemySlime enemy;
    private float blinkRepeatTime = 0.1f;

    public SlimeStunnedState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemySlime enemy) : base(stateMachine, enemyBase, animBoolName)
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

        if (enemy.Rigidbody2D.velocity.y < 0.1f && enemy.IsGroundDetected())
        {
            enemy.EntityFX.CancelColorChange();
            enemy.Animator.SetTrigger(Resources.StunFold);
            enemy.CharacterStats.MakeInvincible(true);
        }


        if (stateTimer <= 0)
            stateMachine.ChangeState(enemy.IdleState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.CharacterStats.MakeInvincible(false);
    }
}

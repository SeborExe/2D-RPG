using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherJumpState : EnemyState
{
    private EnemyArcher enemy;

    public ArcherJumpState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyArcher enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.Rigidbody2D.velocity = new Vector2(enemy.JumpVelocity.x * -enemy.FacingDir, enemy.JumpVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        enemy.Animator.SetFloat(Resources.yVelocity, enemy.Rigidbody2D.velocity.y);

        if (enemy.Rigidbody2D.velocity.y < 0 && enemy.IsGroundDetected())
            stateMachine.ChangeState(enemy.BattleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

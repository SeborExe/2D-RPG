using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyMoveState : ShadyGroundedState
{
    public ShadyMoveState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyShady enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.MoveSpeed * enemy.FacingDir, enemy.Rigidbody2D.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

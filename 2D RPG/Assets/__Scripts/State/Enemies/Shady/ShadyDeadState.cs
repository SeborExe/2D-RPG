using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeadState : EnemyState
{
    private EnemyShady enemy;

    public ShadyDeadState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyShady enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        if (!enemy.HasDeadAnimation)
        {
            enemy.Animator.SetBool(enemy.LastAnimBoolName, true);
            enemy.Animator.speed = 0;
            enemy.CapsuleCollider.enabled = false;

            stateTimer = 0.1f;
        }
    }

    public override void Update()
    {
        base.Update();

        if (!enemy.HasDeadAnimation)
        {
            if (stateTimer > 0)
                enemy.Rigidbody2D.velocity = new Vector2(0, 10);
        }
        else
        {
            enemy.SetVelocity(0, 0);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

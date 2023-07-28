using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathBattleState : EnemyState
{
    private EnemyBringerOfDeath enemy;
    private Transform player;
    private int moveDir;

    public BringerOfDeathBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyBringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.player.transform;

        if (player.GetComponent<Player>().IsDead)
            stateMachine.ChangeState(enemy.IdleState);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.BattleTime;

            if (enemy.IsPlayerDetected().distance < enemy.AttackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.AttackState);
                else
                    stateMachine.ChangeState(enemy.IdleState);
            }
        }

        if (player.transform.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.transform.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.AttackDistance - .5f)
            return;

        enemy.SetVelocity(enemy.MoveSpeed * moveDir, enemy.Rigidbody2D.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttack + enemy.AttackCooldown)
        {
            enemy.AttackCooldown = Random.Range(enemy.AttackCooldownRange.x, enemy.AttackCooldownRange.y);
            enemy.lastTimeAttack = Time.time;
            return true;
        }

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyBattleState : EnemyState
{
    private EnemyShady enemy;
    private Transform player;
    private int moveDir;
    private float defaultSpeed;

    public ShadyBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyShady enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        defaultSpeed = enemy.MoveSpeed;
        enemy.MoveSpeed = enemy.RunSpeed;

        player = PlayerManager.Instance.player.transform;

        if (player.GetComponent<Player>().IsDead)
            stateMachine.ChangeState(enemy.MoveState);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.BattleTime;

            if (enemy.IsPlayerDetected().distance < enemy.AttackDistance && CanAttack())
            {
                enemy.CharacterStats.KillEntity(); //Enter dead state witch trigger explosion and drop items + currency
            }
        }
        else
        {
            if (stateTimer <= 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.MaxDistanceToFollowPlayer)
                stateMachine.ChangeState(enemy.IdleState);
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

        enemy.MoveSpeed = defaultSpeed;
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

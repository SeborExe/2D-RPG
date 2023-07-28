using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathTeleportState : EnemyState
{
    private EnemyBringerOfDeath enemy;

    public BringerOfDeathTeleportState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyBringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.CharacterStats.MakeInvincible(true);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if (enemy.CanDoSpellCast())
                stateMachine.ChangeState(enemy.SpellCastState);
            else
                stateMachine.ChangeState(enemy.BattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.CharacterStats.MakeInvincible(false);
    }
}

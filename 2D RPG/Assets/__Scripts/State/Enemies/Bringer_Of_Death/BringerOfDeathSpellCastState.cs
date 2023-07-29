using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathSpellCastState : EnemyState
{
    private EnemyBringerOfDeath enemy;

    private int amountOfSpells;
    private float spellTimer;

    public BringerOfDeathSpellCastState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyBringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpells = enemy.amountOfSpells;
        spellTimer = 0.5f;
    }

    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (CanCast())
        {
            enemy.CastSpell();
        }

        if (amountOfSpells <= 0)
            stateMachine.ChangeState(enemy.TeleportState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeCast = Time.time;
    }

    private bool CanCast()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells--;
            spellTimer = enemy.spellCooldown;
            return true;
        }

        return false;
    }
}

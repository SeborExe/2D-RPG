using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathSpellCastState : EnemyState
{
    private EnemyBringerOfDeath enemy;

    public BringerOfDeathSpellCastState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyBringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }


}

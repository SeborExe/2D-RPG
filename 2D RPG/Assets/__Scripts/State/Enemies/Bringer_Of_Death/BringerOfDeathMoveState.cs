using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathMoveState : EnemyState
{
    private EnemyBringerOfDeath enemy;

    public BringerOfDeathMoveState(EnemyStateMachine stateMachine, Enemy enemyBase, int animBoolName, EnemyBringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }
}

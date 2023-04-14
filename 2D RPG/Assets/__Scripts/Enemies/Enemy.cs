using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine StateMachine { get; private set; }

    #region States Times
    [field: Header("States Times")]
    [field: SerializeField] public float IdleTime { get; private set; } = 2f;
    #endregion

    #region Stats
    [field: Header("Stats")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 3f;
    #endregion

    protected override void Awake()
    {
        StateMachine = new EnemyStateMachine();

        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.Update();
    }
}

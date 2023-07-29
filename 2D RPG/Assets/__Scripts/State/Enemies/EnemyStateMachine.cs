using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentState { get; private set; }

    public void Initialize(EnemyState state)
    {
        CurrentState = state;
        CurrentState.Enter();
    }

    public void ChangeState(EnemyState state)
    {
        CurrentState.Exit();
        CurrentState = state;
        CurrentState.Enter();

        Debug.Log(state);
    }
}

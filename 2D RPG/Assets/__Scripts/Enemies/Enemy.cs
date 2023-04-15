using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine StateMachine { get; private set; }

    #region States Times
    [field: Header("States Times")]
    [field: SerializeField] public float IdleTime { get; private set; }
    [field: SerializeField] public float BattleTime { get; private set; }
    [field: SerializeField] public float MaxDistanceToFollowPlayer { get; private set; }
    #endregion

    #region Stats
    [field: Header("Stats")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 3f;
    #endregion

    #region Attack Info
    [field: Header("Attack Info")]
    [field: SerializeField] public float AttackDistance { get; private set; }
    [field: SerializeField] public float AttackCooldown { get; private set; }
    [HideInInspector] public float lastTimeAttack;
    #endregion

    [SerializeField] protected LayerMask whatIsPlayer;

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

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, 20f, whatIsPlayer);

    public virtual void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + AttackDistance * FacingDir, transform.position.y));
    }
}

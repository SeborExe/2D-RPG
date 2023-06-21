using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine StateMachine { get; private set; }
    public EntityFX EntityFX { get; private set; }

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
    [field: SerializeField] public float AttackCooldown { get; set; }
    [field: SerializeField] public Vector2 AttackCooldownRange { get; private set; }
    [HideInInspector] public float lastTimeAttack;
    #endregion

    #region StunState
    [field: Header("Stun state info")]
    public float stunTime;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;
    #endregion

    #region Dead
    [field: Header("Dead info")]
    [field: SerializeField] public bool HasDeadAnimation { get; private set; }
    #endregion

    [SerializeField] protected LayerMask whatIsPlayer;

    private float defaultMoveSpeed;
    public int LastAnimBoolName { get; private set; }

    protected override void Awake()
    {
        StateMachine = new EnemyStateMachine();

        base.Awake();
        EntityFX = GetComponent<EntityFX>();

        defaultMoveSpeed = MoveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.Update();
    }

    public override void SlowEntity(float slowPercentage, float slowDuration)
    {
        MoveSpeed = MoveSpeed * (1 - slowPercentage);
        Animator.speed = Animator.speed * (1 - slowPercentage);

        Invoke(nameof(ReturnDefaultSpeed), slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        MoveSpeed = defaultMoveSpeed;
    }

    public virtual void FreezTime(bool timeFrozen)
    {
        if (timeFrozen)
        {
            MoveSpeed = 0f;
            Animator.speed = 0f;
        }
        else
        {
            MoveSpeed = defaultMoveSpeed;
            Animator.speed = 1f;
        }
    }


    public virtual void FreezTimeFor(float duration) => StartCoroutine(FreezTimeCoroutine(duration));

    protected virtual IEnumerator FreezTimeCoroutine(float seconds)
    {
        FreezTime(true);

        yield return new WaitForSeconds(seconds);

        FreezTime(false);
    } 

    #region Counter Attack
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public virtual void AssignLastAnimName(int animBoolName)
    {
        LastAnimBoolName = animBoolName;
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

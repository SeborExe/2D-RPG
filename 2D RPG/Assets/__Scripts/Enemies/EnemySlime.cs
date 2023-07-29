using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : Enemy
{
    #region States
    public SlimeIdleState IdleState { get; private set; }
    public SlimeMoveState MoveState { get; private set; }
    public SlimeBattleState BattleState { get; private set; }
    public SlimeAttackState AttackState { get; private set; }
    public SlimeStunnedState StunnedState { get; private set; }
    public SlimeDeadState DeadState { get; private set; }
    #endregion

    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimeToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;

    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        IdleState = new SlimeIdleState(StateMachine, this, Resources.Idle, this);
        MoveState = new SlimeMoveState(StateMachine, this, Resources.Move, this);
        BattleState = new SlimeBattleState(StateMachine, this, Resources.Move, this);
        AttackState = new SlimeAttackState(StateMachine, this, Resources.Attack, this);
        StunnedState = new SlimeStunnedState(StateMachine, this, Resources.Stun, this);

        if (HasDeadAnimation)
            DeadState = new SlimeDeadState(StateMachine, this, Resources.Die, this);
        else
            DeadState = new SlimeDeadState(StateMachine, this, LastAnimBoolName, this);
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            StateMachine.ChangeState(StunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();

        StateMachine.ChangeState(DeadState);

        if (slimeType == SlimeType.Small) return;

        CreateSlimes(slimeToCreate, slimePrefab);
    }

    private void CreateSlimes(int amountOfSlimes, GameObject slimePrefab)
    {
        for (int i = 0; i < amountOfSlimes; i++)
        {
            GameObject newSlime = Instantiate(slimePrefab, transform.position, Quaternion.identity);
            newSlime.GetComponent<EnemySlime>().SetUpSlime(FacingDir);
        }
    }

    public void SetUpSlime(int facingDir)
    {
        if (facingDir != FacingDir)
            Flip();

        float xVelocity = Random.Range(minCreationVelocity.x, maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y, maxCreationVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(yVelocity, xVelocity);

        Invoke(nameof(CancelKnockback), 1.5f);
    }

    private void CancelKnockback() => isKnocked = false;
}

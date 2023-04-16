using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }

    #region CollisionInfo
    [field: Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    public Transform attackCheck;
    public float attackCheckRadius;
    #endregion

    #region Direction Info
    [field: Header("Direction Info")]
    public int FacingDir { get; private set; } = 1;
    protected bool FacingRight { get; private set; } = true;
    #endregion

    protected virtual void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public void SetVelocity(float xVelocity, float yVelicoty)
    {
        Rigidbody2D.velocity = new Vector2(xVelocity, yVelicoty);
        FlipController(xVelocity);
    }

    public virtual void Flip()
    {
        FacingRight = !FacingRight;
        FacingDir *= -1;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public virtual void FlipController(float move)
    {
        if (move > 0 && !FacingRight)
            Flip();
        else if (move < 0 && FacingRight)
            Flip();
    }

    public virtual void Damage()
    {
        Debug.Log(gameObject.name + " Damaged");
    }

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
}

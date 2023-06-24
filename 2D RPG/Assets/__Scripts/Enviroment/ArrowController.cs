using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float xVelocity;
    [SerializeField] private Vector2 timeToDestroyObject;

    private Rigidbody2D rb;
    private bool canMove = true;
    private bool flipped;
    private string targetLayerName = "Player";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (canMove)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            if (collision.TryGetComponent(out CharacterStats stats))
            {
                stats.TakeDamage(damage);

                StackInto(collision);
            }
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            StackInto(collision);
    }

    private void StackInto(Collider2D collision)
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Stop();
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, Random.Range(timeToDestroyObject.x, timeToDestroyObject.y));
    }

    public void FlipArrow()
    {
        if (flipped)
            return;

        xVelocity = -xVelocity;
        flipped = true;
        transform.Rotate(0, 180f, 0);
        targetLayerName = "Enemy";
    }
}

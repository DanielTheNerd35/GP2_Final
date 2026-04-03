using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float mSpeed = 3f;
    [SerializeField] private int startDirection = 1;
    [SerializeField] private bool stayOnLedges = true;

    private int currentDirection;
    private float halfWidth;
    private float halfHeight;
    private Vector2 movement;
    private bool isGrounded;
    private float movementDelay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;
        currentDirection = startDirection;
        sr.flipX = startDirection == 1 ? false : true;
    }

    void FixedUpdate()
    {

        if (movementDelay > 0f)
        {
            movementDelay -= Time.fixedDeltaTime;
            return;
        }
        movement.x = mSpeed * currentDirection;
        movement.y = rb.linearVelocity.y;
        rb.linearVelocity = movement;
        SetDirection();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }

    public void knockbackEnemy(Vector2 knockbackForce, int direction, float delay)
    {
        movementDelay = delay;
        knockbackForce.x *= direction;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
    }

    private void SetDirection()
    {

        if (!isGrounded) return;

        Vector2 rightPos = transform.position;
        Vector2 leftPos = transform.position;
        rightPos.x += halfWidth;
        leftPos.x -= halfWidth;

        if (rb.linearVelocity.x > 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.right, halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
            // Draw a ray starting at the center of our enemy and point it to the right
            // Check to see if the raycast is intersecting with a wall
            // Also Check to make sure our enemy is actually WALKING right
            // if we don't do this check the enemy will get stuck moving constantly backj and forth
            currentDirection *= -1;
            sr.flipX = true;
            }
            else if (stayOnLedges && !Physics2D.Raycast(rightPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
            {
                currentDirection *= -1;
                sr.flipX = true;
            }

        }
        else if (rb.linearVelocity.x < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left, halfWidth + 0.1f, LayerMask.GetMask("Ground")))
            {
            currentDirection *= -1;
            sr.flipX = false;
            }
            else if (stayOnLedges && !Physics2D.Raycast(leftPos, Vector2.down, halfHeight + 0.1f, LayerMask.GetMask("Ground")))
            {
                currentDirection *= -1;
                sr.flipX = false;
            }

        }

        Debug.DrawRay(transform.position, Vector2.right * (halfWidth + 0.1f), Color.red);
        Debug.DrawRay(transform.position, Vector2.left * (halfWidth + 0.1f), Color.red);
    }
}

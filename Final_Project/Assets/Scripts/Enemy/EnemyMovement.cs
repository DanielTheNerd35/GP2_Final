using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("References")]
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    public Transform detectionPoint;
    public LayerMask playerLayer;
    private Transform player;
    private EnemyState enemyState;

    [Header("Movement")]
    public float mSpeed = 3f;
    private int startDirection = 1;
    [SerializeField] private bool stayOnLedges = true;
    private int currentDirection;
    private float halfWidth;
    private float halfHeight;
    private Vector2 movement;
    private bool isGrounded;
    private float movementDelay;

    [Header("Attack Stats")]
    public float damage = 5;
    public float attackRange = 2;
    public float attackCooldown = 2;
    public float playerDetectedRange = 5;
    private float attackCooldownTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;
        currentDirection = startDirection;
        sr.flipX = startDirection == 1 ? false : true;
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (detectionPoint != null)
        {
            CheckForPLayer();
        }

        switch (enemyState)
        {
            case EnemyState.Chasing:
                ChasePlayer();
                break;
    
            case EnemyState.Attacking:
                rb.linearVelocity = Vector2.zero;
                break;
        }

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

    private void CheckForPLayer() 
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectedRange, playerLayer);
        if (hits.Length > 0)
        {
            player = hits[0].transform;

            //if the player is in attack range AND cooldown is ready
            if (Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }

            else if (Vector2.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * mSpeed;
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

    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     isGrounded = false;
    // }

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

    void ChangeState(EnemyState newState)
   {
    //Exit the current animation
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", false);
        }
        else if (enemyState == EnemyState.Chasing){
            anim.SetBool("Moving", false);
        }
        else if (enemyState == EnemyState.Attacking){
            anim.SetBool("Attacking", false);
        }

        //Update our current state
        enemyState = newState;

        //Update the new animation
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", true);
        }
        else if (enemyState == EnemyState.Chasing){
            anim.SetBool("Moving", true);
        }
        else if (enemyState == EnemyState.Attacking){
            anim.SetBool("Attacking", true);
        }
   }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
}

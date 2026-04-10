using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float mSpeed = 5;
    public float jumpForce = 2.5f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public TrailRenderer tr;
    //public Animator anim;
    public SpearBehavior spear;
    public Transform spearPosition;
    public bool hasthrown;

    private Rigidbody2D rb;
    private KeyCode restartKey;
    private float horizontal;
    private bool isFacingRight = true;

    [Header("Dashing")]
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restartKey = KeyCode.R;
        rb = GetComponent<Rigidbody2D>();
    }

    //Fixed Update is called once per frame every game second
    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.linearVelocity = new Vector2(horizontal * mSpeed, rb.linearVelocity.y);

        if (horizontal > 0 || horizontal < 0)
        {
            //anim.SetBool("IsMoving", true);
        }
        else if(horizontal == 0)
        {
            //anim.SetBool("IsMoving", false);
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Update is called once per frame every second
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            //anim.SetBool("IsJumping", true);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            //anim.SetBool("IsJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();

        if (isDashing)
        {
            return;
        }

         if (Input.GetKeyDown(restartKey)) // Restart button
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Makes it so then it will load the current scene it is on. 
        }

        if (Input.GetMouseButtonDown(0))
        {
             if (!hasthrown)
            {
                ThrowSpear();
            }
            else
            {
                spear.TeleportPlayer();
            }
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void knockbackPlayer(Vector2 knockbackForce, int direction)
    {
        knockbackForce.x *= direction;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
    }

    void ThrowSpear()
    {
        hasthrown = true;

        spear.transform.SetParent(null, true);

        float direction = isFacingRight ? 1f : -1f;

        spear.rb.linearVelocity = new Vector2(direction * spear.speed, 0f);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}

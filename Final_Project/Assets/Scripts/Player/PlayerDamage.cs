using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;

    public Health health;

    [Header("Knockback Settings")]
    public float knockbackForce = 20;
    public float knockbackDuration = .2f;
    private float knockbackVelocity;
    private float timer;

    private void OnEnable()
    {
        health.OnDamaged += HandleDamage;
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDamaged -= HandleDamage;
        health.OnDeath -= HandleDeath;
    }


    void HandleDamage(Vector2 sourcePosition)
    {
        player.anim.SetBool("IsDamaged", true);
        int knockbackDir = 0;
        knockbackDir = transform.position.x > sourcePosition.x ? 1 : -1;
        knockbackVelocity = knockbackDir * knockbackForce;
        player.rb.linearVelocity = new Vector2(knockbackVelocity, player.rb.linearVelocity.y);
    }

    public void FixedUpdate()
    {
        knockbackDuration -= Time.fixedDeltaTime;
        if(knockbackDuration <= 0)
        {
            player.rb.linearVelocity = Vector2.zero;
            player.anim.SetBool("IsDamaged", false);
        }
    }
    
    void HandleDeath()
    {

    }
}

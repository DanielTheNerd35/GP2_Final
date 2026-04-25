using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;

    public Transform attackPoint;
    public float weaponRange = 1;
    public LayerMask enemyLayer;
    public int damage = 2;
    public float cooldown = 1;
    private float timer;

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if(timer <= 0)
        {
            anim.SetBool("IsAttacking", true);
            FindObjectOfType<AudioManager>().Play("AttackNoise");

            timer = cooldown;
        }
    }

    public void DealDamage()
    {
         Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

            if(enemies.Length > 0)
            {
                enemies[0].GetComponent<Health>().ChangeHealth(-damage, transform.position);
            }
    }

    public void FinishAttacking()
    {
        anim.SetBool("IsAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public int health;
    public int damage;

    public void HitPlayer(Transform playerTransform)
    {
        int direction = GetDirection(playerTransform);
        FindObjectOfType<PlayerHealth>().TakeDamage(damage);
    }

    private int GetDirection(Transform playerTransform)
    {
        if (transform.position.x > playerTransform.position.x)
        {
            // Our enemy  is to the right of the player
            return -1;
        }
        else
        {
            return 1;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <=0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}

using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public int health;
    public int currentHealth {get; private set;}
    public int maxHealth {get; private set;}
    public static Action<int> OnPlayerTakeDamage;
    public GameObject player;
    private const string flashRedAnim = "FlashRed";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currentHealth = health;
        maxHealth = health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Took Damage!");
        OnPlayerTakeDamage?.Invoke(currentHealth);
        anim.SetTrigger(flashRedAnim);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}

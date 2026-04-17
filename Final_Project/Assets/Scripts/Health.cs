using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action OnDamaged;
    public event Action OnDeath;
    public int maxHealth;
    public int currentHealth;

    public HealthBar healthbar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        healthbar.SetHealth(currentHealth);

        if (currentHealth <=0)
        {
            OnDeath?.Invoke();
        }
        else if (amount <0)
        {
            OnDamaged?.Invoke();
        }
    }
}

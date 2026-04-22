using UnityEngine;

public class EnemyDamaged : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    public Health health;

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
        int knockbackDir = 0;
        knockbackDir = transform.position.x > sourcePosition.x ? 1 : -1;

        enemy.StateMachine.ChangeState(new DamagedState(enemy, knockbackDir));

    }

    void HandleDeath()
    {
        enemy.StateMachine.ChangeState(new DeadState(enemy));
    }
}

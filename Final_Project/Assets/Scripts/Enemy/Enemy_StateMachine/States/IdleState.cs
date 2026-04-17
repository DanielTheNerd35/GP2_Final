using UnityEngine;

public class IdleState : State
{
    private Transform target;

    protected override string AnimBoolName => "Idling";
    
    public IdleState(Enemy enemy) : base (enemy){}

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
    }

    public override void FixedUpdate()
    {
        //Check For Target
        target = senses.GetChaseTarget();

        if(!target)
        {
            stateMachine.ChangeState(new PatrolState(enemy));
            return;
        }

        enemy.FaceTarget(target);

        if(senses.IsInMeleeRange(target) && combat.CanMeleeAttack())
        {
            stateMachine.ChangeState(new MeleeAttackState(enemy));
            return;
        }

        // Check if enemy has reached the target
        float distance = Mathf.Abs(target.position.x - enemy.transform.position.x);
        if(distance <= config.turnThreshold)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        //Check for Obstacles
        if(senses.IsHittingWall() || senses.IsAtCliff())
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        // There is a target, target has not been reached, AND no obstacles
        stateMachine.ChangeState(new ChaseState(enemy));
    }
}

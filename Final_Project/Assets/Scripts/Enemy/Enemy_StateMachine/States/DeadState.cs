using UnityEngine;

public class DeadState : State
{
    protected override string AnimBoolName => "IsDead";

    public DeadState(Enemy enemy) : base (enemy) {}

    public override void Enter()
    {
        base.Enter();
        
    }
    
}

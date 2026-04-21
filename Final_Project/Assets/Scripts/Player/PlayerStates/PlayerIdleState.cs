using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState (PlayerMovement player) : base (player) {}
    
    public override void Enter()
    {
        anim.SetBool("IsIdling", true);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        anim.SetBool("IsIdling", false);
    }
}

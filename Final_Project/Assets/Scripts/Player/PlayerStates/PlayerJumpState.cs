 using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState (PlayerMovement player) : base (player) {}

     public override void Enter()
    {
        base.Enter();
        anim.SetBool("IsJumping", true);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("IsJumping", false);
    }
}

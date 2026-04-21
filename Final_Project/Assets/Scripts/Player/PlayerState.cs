using UnityEngine;

public abstract class PlayerState 
{
    protected PlayerMovement player;
    protected Animator anim;
    protected Rigidbody2D rb;

    public PlayerState (PlayerMovement player)
    {
        this.player = player;
        this.anim = player.anim;
    }

    public virtual void Enter(){}
    public virtual void Exit(){}

    public virtual void Update(){}
    public virtual void FixedUpdate(){}
}

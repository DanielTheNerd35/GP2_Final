using UnityEngine;

public class Enemy : MonoBehaviour
{
    //-- VARIABLES--
    public int FacingDirection{get; private set;} = 1;

    //-- COMPONENTS -- 
    public Rigidbody2D RB {get; private set;}
    public EnemyConfig Config;
    public EnemySenses Senses {get; private set;}
    public StateMachine StateMachine {get; private set;}

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        StateMachine = new StateMachine();
        Senses = GetComponent<EnemySenses>();
    }

    public void Start()
    {
        StateMachine.Initialize(new PatrolState(this));
    }

    private void Update() => StateMachine.CurrentState?.Update();
    private void FixedUpdate() => StateMachine.CurrentState?.FixedUpdate();

    public void Flip()
    {
        FacingDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x = FacingDirection;
        transform.localScale = scale;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : BaseGameEntity
{
    public float speed;
    private StateMachine<PacMan> stateMachine;
    private Animator animator;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    public void Initialize()
    {
        animator = GetComponent<Animator>();
        stateMachine = new StateMachine<PacMan>(this);
        stateMachine.currentState = Wander.Instance;
        stateMachine.currentState.Enter(this);
        rb = GetComponent<Rigidbody2D>();
    }

    public override bool HandleMessage(Telegram telegram)
    {
        return stateMachine.HandleMessage(telegram);
    }

    public void ChangeState(State<PacMan> newState)
    {
        stateMachine.ChangeState(newState);
    }

    public void Reset()
    {
        transform.position = Vector2.zero;
        animator.Play("Right");
    }

    public void Move()
    {
        Vector2 position = transform.position;
        float horizontalMovement = 0.0f;
        float verticalMovement = 0.0f;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            verticalMovement = 1.0f;
            horizontalMovement = 0.0f;
            animator.Play("Up");
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            verticalMovement = -1.0f;
            horizontalMovement = 0.0f;
            animator.Play("Down");
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalMovement = -1.0f;
            verticalMovement = 0.0f;
            animator.Play("Left");
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            horizontalMovement = 1.0f;
            verticalMovement = 0.0f;
            animator.Play("Right");
        }
        else
        {
            verticalMovement = 0.0f;
            horizontalMovement = 0.0f;
        }

        animator.SetFloat("Speed", speed);

        rb.velocity = new Vector2(speed * horizontalMovement, speed * verticalMovement);
    }

    public void Die()
    {
        rb.velocity = Vector2.zero;
        animator.Play("PacManDeath");
    }

    public void NotifyDeath()
    {
        MessageDispatcher.Instance.DispatchMessage(0.0f, m_ID, (int)Entities.GAME_MANAGER, MessageType.PAC_MAN_DEATH);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Breadcrumb"))
        {
            Destroy(collider.gameObject);
            MessageDispatcher.Instance.DispatchMessage(0.0f, m_ID, (int)Entities.GAME_MANAGER, MessageType.INCREASE_SCORE, "1");
        }
        else if(collider.gameObject.CompareTag("SpecialBreadcrumb"))
        {
            Destroy(collider.gameObject);
            MessageDispatcher.Instance.DispatchMessage(0.0f, m_ID, (int)Entities.GHOST, MessageType.GO_BLUE);
            MessageDispatcher.Instance.DispatchMessage(0.0f, m_ID, (int)Entities.GAME_MANAGER, MessageType.INCREASE_SCORE, "10");
        }
        else if(collider.gameObject.CompareTag("BlueGhost"))
        {
            Destroy(collider.gameObject);
            EntityManager.DeleteEntity(collider.gameObject.GetComponent<BaseGameEntity>());
            MessageDispatcher.Instance.DispatchMessage(0.0f, m_ID, (int)Entities.GAME_MANAGER, MessageType.GHOST_DESTROYED);
            MessageDispatcher.Instance.DispatchMessage(0.0f, m_ID, (int)Entities.GAME_MANAGER, MessageType.INCREASE_SCORE, "200");
        }
        else if(collider.gameObject.CompareTag("Cherry"))
        {
            Destroy(collider.gameObject);
            MessageDispatcher.Instance.DispatchMessage(0.0f, m_ID, (int)Entities.GAME_MANAGER, MessageType.INCREASE_SCORE, "100");
        }
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }
}

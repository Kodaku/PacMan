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
    public void Initialize(int id)
    {
        stateMachine = new StateMachine<PacMan>(this);
        m_ID = id;
        stateMachine.currentState = Wander.Instance;
        stateMachine.currentState.Enter(this);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Breadcrumb"))
        {
            Destroy(collider.gameObject);
        }
        else if(collider.gameObject.CompareTag("SpecialBreadcrumb"))
        {
            Destroy(collider.gameObject);
            GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
            foreach(GameObject ghost in ghosts)
            {
                MessageDispatcher.Instance.DispatchMessage(0.0f, m_ID, ghost.GetComponent<Ghost>().ID, MessageType.GO_BLUE);
            }
        }
        else if(collider.gameObject.CompareTag("BlueGhost"))
        {
            Destroy(collider.gameObject);
        }
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }
}

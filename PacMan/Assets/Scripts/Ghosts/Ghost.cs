using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : BaseGameEntity
{
    public float speed;
    public GhostType ghostType;
    protected PacMan target;
    protected Vector2 m_scatterPoint;
    protected StateMachine<Ghost> stateMachine;
    protected Distances m_distances;
    protected List<Vector2> pathToDestination = new List<Vector2>();
    protected Cell m_currentCell;
    protected Vector2 nextDestination;
    protected MyGrid m_grid;
    protected Animator animator;
    protected Rigidbody2D rb;
    private bool m_isBlue = false;
    private float blueTimer = 40.0f;
    private float currentBlueTimer = 0.0f;
    public GameObject debugSymbol;
    // Start is called before the first frame update
    public virtual void Initialize(int id)
    {
        stateMachine = new StateMachine<Ghost>(this);
        m_ID = id;
        stateMachine.currentState = Scatter.Instance;
        stateMachine.currentState.Enter(this);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PacMan>();
    }

    public Vector2 scatterPoint
    {
        get { return m_scatterPoint; }
        set { m_scatterPoint = value; }
    }

    public Distances distances
    {
        get { return m_distances; }
        set { m_distances = value; }
    }

    public Cell currentCell
    {
        get { return m_currentCell; }
        set { m_currentCell = value; }
    }

    public MyGrid grid
    {
        set { m_grid = value; }
    }

    public bool isBlue
    {
        get { return m_isBlue; }
        set { m_isBlue = value; }
    }

    public virtual void ChangeState(State<Ghost> newState)
    {
        stateMachine.ChangeState(newState);
    }

    public override bool HandleMessage(Telegram telegram)
    {
        return stateMachine.HandleMessage(telegram);
    }

    public virtual void FindPathToScatterPoint()
    {
        pathToDestination.Clear();
        m_currentCell = m_grid.WorldPointToCell(transform.position);
        Distances distances = m_currentCell.Distances;
        m_distances = distances.PathToGoal(m_grid.WorldPointToCell(scatterPoint));
        foreach(Cell cell in m_distances.Cells())
        {
            // Instantiate(debugSymbol, new Vector2(cell.column, cell.row), Quaternion.identity);
            Vector2 position = new Vector2(cell.column, cell.row);
            pathToDestination.Add(position);
        }
        pathToDestination.Reverse();
        nextDestination = pathToDestination[0];
        pathToDestination.Remove(nextDestination);
        if(pathToDestination.Count > 0)
        {
            pathToDestination.Remove(pathToDestination[pathToDestination.Count - 1]);
        }
    }

    public virtual void FindPathToEscapePoint()
    {
        pathToDestination.Clear();
        m_currentCell = m_grid.WorldPointToCell(transform.position);
        Vector2 escapePosition = transform.position;
        Distances distances = m_currentCell.Distances;
        int choice = Random.Range(0, 4);
        switch(choice)
        {
            case 0:
            {
                escapePosition.x += 5.0f;
                break;
            }
            case 1:
            {
                escapePosition.x -= 5.0f;
                break;
            }
            case 2:
            {
                escapePosition.y += 5.0f;
                break;
            }
            case 3:
            {
                escapePosition.y -= 5.0f;
                break;
            }
        }
        escapePosition.x = Mathf.Clamp(escapePosition.x, 0.0f, m_grid.columns - 1);
        escapePosition.y = Mathf.Clamp(escapePosition.y, 0.0f, m_grid.rows - 1);
        // print(escapePosition);
        m_distances = distances.PathToGoal(m_grid.WorldPointToCell(escapePosition));
        foreach(Cell cell in m_distances.Cells())
        {
            // Instantiate(debugSymbol, new Vector2(cell.column, cell.row), Quaternion.identity);
            Vector2 position = new Vector2(cell.column, cell.row);
            pathToDestination.Add(position);
        }
        pathToDestination.Reverse();
        nextDestination = pathToDestination[0];
        pathToDestination.Remove(nextDestination);
        if(pathToDestination.Count > 0)
        {
            pathToDestination.Remove(pathToDestination[pathToDestination.Count - 1]);
        }
    }

    public virtual void FindPathToTarget()
    {
        m_currentCell = m_grid.WorldPointToCell(transform.position);
    }

    public virtual void MoveScatter()
    {
        Vector2 playerPosition = transform.position;
        if(Vector2.Distance(playerPosition, nextDestination) <= 0.1f)
        {
            if(pathToDestination.Count > 0)
            {
                nextDestination = pathToDestination[0];
                pathToDestination.Remove(nextDestination);
                Vector2 direction = (nextDestination - playerPosition).normalized;
                PlayAnimation(direction);
            }
            else if(!m_isBlue)
            {
                ChangeState(Chase.Instance);
            }
            else
            {
                ChangeState(Frightened.Instance);
            }
        }
        else
        {
            Vector2 direction = (nextDestination - playerPosition).normalized;
            playerPosition.x += direction.x * speed * Time.deltaTime;
            playerPosition.y += direction.y * speed * Time.deltaTime;
            transform.position = playerPosition;
        }
    }

    public virtual void MoveChase()
    {
        Vector2 playerPosition = transform.position;
        if(Vector2.Distance(playerPosition, nextDestination) <= 0.1f && !m_isBlue)
        {
            Vector2 direction = (nextDestination - playerPosition).normalized;
            PlayAnimation(direction);
            FindPathToTarget();
        }
        else if(!m_isBlue)
        {
            Vector2 direction = (nextDestination - playerPosition).normalized;
            playerPosition.x += direction.x * speed * Time.deltaTime;
            playerPosition.y += direction.y * speed * Time.deltaTime;
            transform.position = playerPosition;
        }
        else
        {
            ChangeState(Frightened.Instance);
        }
    }

    public virtual void MoveFrightened()
    {
        Vector2 playerPosition = transform.position;
        if(Vector2.Distance(playerPosition, nextDestination) <= 0.1f)
        {
            if(pathToDestination.Count > 0)
            {
                nextDestination = pathToDestination[0];
                pathToDestination.Remove(nextDestination);
                Vector2 direction = (nextDestination - playerPosition).normalized;
                PlayAnimation(direction);
            }
            else if(!m_isBlue)
            {
                ChangeState(Chase.Instance);
            }
            else
            {
                FindPathToEscapePoint();
            }
        }
        else
        {
            Vector2 direction = (nextDestination - playerPosition).normalized;
            playerPosition.x += direction.x * speed * Time.deltaTime;
            playerPosition.y += direction.y * speed * Time.deltaTime;
            transform.position = playerPosition;
        }
    }

    private void PlayAnimation(Vector2 direction)
    {
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0.0f)
            {
                animator.SetTrigger("Right");
            }
            else if(direction.x < 0.0f)
            {
                animator.SetTrigger("Left");
            }
        }
        else if(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if(direction.y > 0.0f)
            {
                animator.SetTrigger("Up");
            }
            else if(direction.y < 0.0f)
            {
                animator.SetTrigger("Down");
            }
        }
    }

    public void GoBlue()
    {
        currentBlueTimer = 0.0f;
        m_isBlue = true;
        animator.SetTrigger("GoBlue");
        gameObject.tag = "BlueGhost";
        ChangeState(Scatter.Instance);
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
        if(m_isBlue)
            currentBlueTimer += Time.deltaTime;
        if(m_isBlue && currentBlueTimer >= blueTimer)
        {
            gameObject.tag = "Ghost";
            m_isBlue = false;
            animator.SetTrigger("GoNormal");
            ChangeState(Chase.Instance);
        }
    }
}

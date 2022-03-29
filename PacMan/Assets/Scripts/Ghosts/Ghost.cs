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
    private float blueTimer = 40.0f;
    private float currentBlueTimer = 0.0f;
    protected Dictionary<PathFinderType, GhostPathFinder> pathFinders = new Dictionary<PathFinderType, GhostPathFinder>();
    protected Dictionary<MovementType, GhostMovement> ghostMovements = new Dictionary<MovementType, GhostMovement>();
    private GhostPathFinder currentPathFinder;
    private GhostAnimator ghostAnimator;
    // Start is called before the first frame update
    public virtual void Initialize(MyGrid grid)
    {
        InitializePathFinders(grid);
        InitializeStateMachine();
        InitializeAnimator();
        InitializeMovements();

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PacMan>();
    }

    private void InitializeStateMachine()
    {
        stateMachine = new StateMachine<Ghost>(this);
        stateMachine.currentState = Scatter.Instance;
        stateMachine.currentState.Enter(this);
    }

    public virtual void InitializePathFinders(MyGrid grid)
    {
        pathFinders.Add(PathFinderType.SCATTER, new ScatterPathFinder(grid));
        pathFinders.Add(PathFinderType.ESCAPE, new EscapePathFinder(grid));
        pathFinders.Add(PathFinderType.SCATTER_FRIGHTENED, new ScatterFrightenedPathFinder(grid));
    }

    private void InitializeAnimator()
    {
        ghostAnimator = new GhostAnimator(this);
    }

    private void InitializeMovements()
    {
        ghostMovements.Add(MovementType.SCATTER, new ScatterMovement(speed));
        ghostMovements.Add(MovementType.SCATTER_FRIGHTENED, new ScatterFrightenedMovement(speed));
        ghostMovements.Add(MovementType.ESCAPE, new EscapeMovement(speed));
        ghostMovements.Add(MovementType.CHASE, new ChaseMovement(speed));
    }

    public Vector2 scatterPoint
    {
        get { return m_scatterPoint; }
        set { m_scatterPoint = value; }
    }

    public virtual void ChangeState(State<Ghost> newState)
    {
        stateMachine.ChangeState(newState);
    }

    public override bool HandleMessage(Telegram telegram)
    {
        return stateMachine.HandleMessage(telegram);
    }

    public void FindPath(PathFinderType pathFinderType)
    {
        currentPathFinder = pathFinders[pathFinderType];
        Vector2 destination = Vector2.zero;
        switch(pathFinderType)
        {
            case PathFinderType.SCATTER : case PathFinderType.SCATTER_FRIGHTENED:
            {
                destination = m_scatterPoint;
                break;
            }
            case PathFinderType.ESCAPE:
            {
                destination = transform.position;
                break;
            }
            case PathFinderType.CHASE:
            {
                destination = target.transform.position;
                break;
            }
        }
        currentPathFinder.FindPathToPoint(this, destination);
    }

    public void DecideNextState(StateType stateType)
    {
        switch(stateType)
        {
            case StateType.SCATTER:
            {
                ChangeState(Scatter.Instance);
                break;
            }
            case StateType.CHASE:
            {
                ChangeState(Chase.Instance);
                break;
            }
            case StateType.SCATTER_FRIGHTENED:
            {
                ChangeState(ScatterFrightened.Instance);
                break;
            }
            case StateType.FRIGHTENED:
            {
                ChangeState(Frightened.Instance);
                break;
            }
        }
    }

    public virtual void MoveScatter()
    {
        ghostMovements[MovementType.SCATTER].Move(this);
    }

    public virtual void MoveScatterFrightened()
    {
        ghostMovements[MovementType.SCATTER_FRIGHTENED].Move(this);
    }

    public virtual void MoveChase()
    {
        ghostMovements[MovementType.CHASE].Move(this);
    }

    public virtual void MoveFrightened()
    {
        ghostMovements[MovementType.ESCAPE].Move(this);
    }

    public void PlayAnimation(Vector2 playerPosition)
    {
        Vector2 direction = (GetNextDestination() - playerPosition).normalized;
        ghostAnimator.PlayAnimation(direction);
    }

    public Vector2 GetNextDestination()
    {
        return currentPathFinder.nextDestination;
    }

    public bool IsCurrentPathFinderEmpty()
    {
        return currentPathFinder.IsEmpty();
    }

    public void DecideNextDestination(Vector2 playerPosition)
    {
        currentPathFinder.SetNextDestinationFromPath();
        PlayAnimation(playerPosition);
    }

    public void GoBlue()
    {
        currentBlueTimer = 0.0f;
        ghostAnimator.GoBlue();
        gameObject.tag = "BlueGhost";
        DecideNextState(StateType.SCATTER_FRIGHTENED);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player") && !gameObject.CompareTag("BlueGhost"))
        {
            MessageDispatcher.Instance.DispatchMessage(0.0f, m_ID, (int)Entities.PACMAN, MessageType.KILL_PAC_MAN);
        }
    }

    public void IncreaseFrightenedTimer()
    {
        currentBlueTimer += Time.deltaTime;
    }

    public void CheckFrightenedEnd()
    {
        if(currentBlueTimer >= blueTimer)
        {
            gameObject.tag = "Ghost";
            ghostAnimator.GoNormal();
            DecideNextState(StateType.CHASE);
        }
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }
}

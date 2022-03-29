using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Game : BaseGameEntity
{
    private StateMachine<Game> stateMachine;
    private GameInitializer gameInitializer;
    private GhostsManager ghostsManager;
    private PacManManager pacManManager;
    private ScoreManager scoreManager;
    private LivesManager livesManager;
    private MyGrid grid;
    private Dictionary<Tuple<int, int>, List<string>> world;
    private float ghostTimer = 10.0f;
    private float currentGhostTimer = 0.0f;
    // Start is called before the first frame update
    public void Initialize()
    {
        gameInitializer = GetComponent<GameInitializer>();
        ghostsManager = GetComponent<GhostsManager>();
        pacManManager = GetComponent<PacManManager>();
        scoreManager = GetComponent<ScoreManager>();
        livesManager = GetComponent<LivesManager>();

        pacManManager.SpawnPacMan();

        stateMachine = new StateMachine<Game>(this);
        stateMachine.currentState = StartGame.Instance;
        stateMachine.currentState.Enter(this);
    }

    public void ChangeState(State<Game> newState)
    {
        stateMachine.ChangeState(newState);
    }

    public override bool HandleMessage(Telegram telegram)
    {
        return stateMachine.HandleMessage(telegram);
    }

    public void InitializeGame()
    {
        gameInitializer.Reset();
        grid = gameInitializer.InitializeGrid();
        world = gameInitializer.BuildMaze(grid);
        gameInitializer.AddSpecialBreadcrumbs(grid);
        gameInitializer.AddCherries(grid);
        scoreManager.ResetScore();
        livesManager.ResetLives();
        ChangeState(ResetGame.Instance);
    }

    public void SoftInitialization()
    {
        ghostsManager.ClearGhosts();
        pacManManager.ResetPacMan();
        ghostsManager.SpawnGhost(grid);
        ChangeState(PlayGame.Instance);
    }

    public void IncreaseGhostTimer()
    {
        currentGhostTimer += Time.deltaTime;
        if(currentGhostTimer >= ghostTimer)
        {
            currentGhostTimer = 0.0f;
            ghostsManager.SpawnGhost(grid);
        }
    }

    public void DecreaseGhostCount()
    {
        ghostsManager.DecreaseGhostCount();
    }

    public void IncreaseScore(int amount)
    {
        scoreManager.IncreaseScore(amount);
    }

    public void DecreasePacManLives()
    {
        livesManager.DecreaseLife();
        if(livesManager.HasLostGame())
        {
            scoreManager.ResetScore();
            livesManager.ResetLives();
            ChangeState(StartGame.Instance);
        }
        else
        {
            ChangeState(ResetGame.Instance);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        stateMachine.Update();
    }
}

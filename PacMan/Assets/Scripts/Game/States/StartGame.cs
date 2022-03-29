using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : State<Game>
{
    private static StartGame m_instance;
    public static StartGame Instance { get { return m_instance; }}

    void Awake()
    {
        if(m_instance != null && m_instance != this)
        {
            Destroy(this);
        }
        else
        {
            m_instance = this;
        }
    }
    public override void Enter(Game game)
    {
        game.InitializeGame();
    }

    public override void Execute(Game game)
    {
        
    }

    public override void Exit(Game game)
    {
        
    }

    public override bool OnMessage(Game game, Telegram telegram)
    {
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGame : State<Game>
{
    private static PlayGame m_instance;
    public static PlayGame Instance { get { return m_instance; }}

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
        
    }

    public override void Execute(Game game)
    {
        game.IncreaseGhostTimer();
    }

    public override void Exit(Game game)
    {
        
    }

    public override bool OnMessage(Game game, Telegram telegram)
    {
        // print(telegram.messageType);
        switch(telegram.messageType)
        {
            case MessageType.GHOST_DESTROYED:
            {
                game.DecreaseGhostCount();
                break;
            }
            case MessageType.PAC_MAN_DEATH:
            {
                game.DecreasePacManLives();
                break;
            }
            case MessageType.INCREASE_SCORE:
            {
                int amount = int.Parse(telegram.extraInfo);
                game.IncreaseScore(amount);
                break;
            }
        }
        return true;
    }
}

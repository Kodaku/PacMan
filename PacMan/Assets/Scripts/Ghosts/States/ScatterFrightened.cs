using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterFrightened : State<Ghost>
{
    private static ScatterFrightened m_instance;

    public static ScatterFrightened Instance
    {
        get { return  m_instance; }
    }

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
    public override void Enter(Ghost ghost)
    {
        ghost.FindPath(PathFinderType.SCATTER_FRIGHTENED);
    }

    public override void Execute(Ghost ghost)
    {
        ghost.MoveScatterFrightened();
        ghost.IncreaseFrightenedTimer();
        ghost.CheckFrightenedEnd();
    }

    public override void Exit(Ghost ghost)
    {
        
    }

    public override bool OnMessage(Ghost ghost, Telegram telegram)
    {
        // print("Received " + telegram.messageType);
        ghost.GoBlue();
        return true;
    }
}

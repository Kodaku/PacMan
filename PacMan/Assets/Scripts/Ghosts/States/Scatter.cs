using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatter : State<Ghost>
{
    private static Scatter m_instance;

    public static Scatter Instance
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
        ghost.FindPath(PathFinderType.SCATTER);
    }

    public override void Execute(Ghost ghost)
    {
        ghost.MoveScatter();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frightened : State<Ghost>
{
    private static Frightened m_instance;

    public static Frightened Instance
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
        ghost.FindPathToEscapePoint();
    }

    public override void Execute(Ghost ghost)
    {
        ghost.MoveFrightened();
    }

    public override void Exit(Ghost ghost)
    {
        
    }

    public override bool OnMessage(Ghost ghost, Telegram telegram)
    {
        print("Received " + telegram.messageType);
        ghost.GoBlue();
        return true;
    }
}

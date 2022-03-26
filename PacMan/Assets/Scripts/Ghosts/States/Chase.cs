using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State<Ghost>
{
    private static Chase m_instance;

    public static Chase Instance
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
        ghost.FindPathToTarget();
    }

    public override void Execute(Ghost ghost)
    {
        ghost.MoveChase();

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

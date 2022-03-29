using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : State<PacMan>
{
    private static Wander m_instance;
    public static Wander Instance { get { return m_instance; }}

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
    public override void Enter(PacMan pacMan)
    {
        pacMan.Reset();
    }

    public override void Execute(PacMan pacMan)
    {
        pacMan.Move();
    }

    public override void Exit(PacMan pacMan)
    {
        
    }

    public override bool OnMessage(PacMan pacMan, Telegram telegram)
    {
        switch(telegram.messageType)
        {
            case MessageType.KILL_PAC_MAN:
            {
                pacMan.ChangeState(Dead.Instance);
                break;
            }
        }
        return true;
    }
}

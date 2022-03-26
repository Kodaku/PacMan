using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private T m_owner;
    private State<T> m_previousState;
    private State<T> m_currentState;
    private State<T> m_globalState;

    public StateMachine(T owner)
    {
        m_owner = owner;
        m_previousState = null;
        m_currentState = null;
        m_globalState = null;
    }

    public State<T> previousState
    {
        set { m_previousState = value; }
    }

    public State<T> currentState
    {
        get { return m_currentState; }
        set { m_currentState = value; }
    }

    public State<T> globalState
    {
        set { m_globalState = value; }
    }

    public void ChangeState(State<T> newState)
    {
        if(m_currentState != null && newState != null)
        {
            m_previousState = m_currentState;
            m_currentState.Exit(m_owner);
            m_currentState = newState;
            m_currentState.Enter(m_owner);
        }
    }

    public void RevertToPreviousState()
    {
        ChangeState(m_previousState);
    }

    public bool HandleMessage(Telegram telegram)
    {
        if(m_currentState != null && m_currentState.OnMessage(m_owner, telegram))
        {
            return true;
        }
        if(m_globalState != null && m_globalState.OnMessage(m_owner, telegram))
        {
            return true;
        }
        return false;
    }

    public void Update()
    {
        if(m_globalState != null)
        {
            m_globalState.Execute(m_owner);
        }
        if(m_currentState != null)
        {
            m_currentState.Execute(m_owner);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telegram
{
    private int m_senderID;
    private int m_receiverID;
    private MessageType m_messageType;
    private float m_dispatchTime;
    private float m_currentTime;
    private string m_extraInfo;

    public Telegram(float dispatchTime, int senderID, int receiverID, MessageType messageType, string extraInfo = "")
    {
        m_dispatchTime = dispatchTime;
        m_senderID = senderID;
        m_receiverID = receiverID;
        m_messageType = messageType;
        m_extraInfo = extraInfo;
    }

    public int senderID
    {
        get { return m_senderID; }
        set { m_senderID = value; }
    }

    public int receiverID
    {
        get { return m_receiverID; }
        set { m_receiverID = value; }
    }

    public MessageType messageType
    {
        get { return m_messageType; }
        set { m_messageType = value; }
    }

    public float dispatchTime
    {
        get { return m_dispatchTime; }
        set { m_dispatchTime = value; }
    }

    public float currentTime
    {
        get { return m_currentTime; }
        set { m_currentTime = value; }
    }

    public string extraInfo
    {
        get { return m_extraInfo; }
        set { m_extraInfo = value; }
    }
}

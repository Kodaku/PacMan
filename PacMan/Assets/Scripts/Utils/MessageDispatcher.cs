using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDispatcher : MonoBehaviour
{
    private List<Telegram> messageList = new List<Telegram>();
    private static MessageDispatcher m_instance;

    public static MessageDispatcher Instance
    {
        get { return m_instance; }
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
    
    private void Discharge(BaseGameEntity entity, Telegram telegram)
    {
        // print(telegram.messageType);
        entity.HandleMessage(telegram);
    }

    public void DispatchMessage(float delay, int senderID, int receiverID, MessageType messageType, string extraInfo = "")
    {
        List<BaseGameEntity> entities = EntityManager.GetEntityByID(receiverID);
        Telegram telegram = new Telegram(0.0f, senderID, receiverID, messageType, extraInfo:extraInfo);

        if(delay <= 0.0f)
        {
            foreach(BaseGameEntity entity in entities)
                Discharge(entity, telegram);
        }
        else
        {
            telegram.dispatchTime = delay;
            messageList.Add(telegram);
        }
    }

    private void DispatchDelayedMessage(Telegram telegram)
    {
        List<BaseGameEntity> entities = EntityManager.GetEntityByID(telegram.receiverID);
        foreach(BaseGameEntity entity in entities)
            Discharge(entity, telegram);
        messageList.Remove(telegram);
    }

    void Update()
    {
        List<Telegram> messagesToDispatch = new List<Telegram>();
        
        //Check for messages to send
        foreach(Telegram telegram in messageList)
        {
            telegram.currentTime += Time.deltaTime;
            if(telegram.currentTime >= telegram.dispatchTime)
            {
                messagesToDispatch.Add(telegram);
            }
        }

        //Sending delayed messages
        foreach(Telegram telegram in messagesToDispatch)
        {
            DispatchDelayedMessage(telegram);
        }
    }
}

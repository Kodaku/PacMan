using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameEntity : MonoBehaviour
{
    protected int m_ID = -1;

    public int ID
    {
        get { return m_ID; }
        set { m_ID = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void RegisterEntity(int id)
    {
        m_ID = id;
        EntityManager.RegisterEntity(this);
    }

    public virtual bool HandleMessage(Telegram telegram)
    {
        return false;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(m_ID == -1) return;
    }
}

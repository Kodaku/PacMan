using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimator
{
    private Animator m_animator;
    public GhostAnimator(Ghost ghost)
    {
        m_animator = ghost.GetComponent<Animator>();
    }
    
    public void PlayAnimation(Vector2 direction)
    {
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0.0f)
            {
                m_animator.SetTrigger("Right");
            }
            else if(direction.x < 0.0f)
            {
                m_animator.SetTrigger("Left");
            }
        }
        else if(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if(direction.y > 0.0f)
            {
                m_animator.SetTrigger("Up");
            }
            else if(direction.y < 0.0f)
            {
                m_animator.SetTrigger("Down");
            }
        }
    }

    public void GoBlue()
    {
        m_animator.SetTrigger("GoBlue");
    }

    public void GoNormal()
    {
        m_animator.SetTrigger("GoNormal");
    }
}

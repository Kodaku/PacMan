using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement
{
    protected float m_speed;
    public GhostMovement(float speed)
    {
        m_speed = speed;
    }
    public virtual void Move(Ghost ghost)
    {

    }

    public void MoveGhost(Ghost ghost)
    {
        Vector2 playerPosition = ghost.transform.position;
        Vector2 direction = (ghost.GetNextDestination() - playerPosition).normalized;
        playerPosition.x += direction.x * m_speed * Time.deltaTime;
        playerPosition.y += direction.y * m_speed * Time.deltaTime;
        ghost.transform.position = playerPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterFrightenedMovement : GhostMovement
{
    public ScatterFrightenedMovement(float speed) : base(speed) {}

    public override void Move(Ghost ghost)
    {
        base.Move(ghost);
        Vector2 playerPosition = ghost.transform.position;
        Vector2 nextDestination = ghost.GetNextDestination();
        if(Vector2.Distance(playerPosition, nextDestination) <= 0.1f)
        {
            if(!ghost.IsCurrentPathFinderEmpty())
            {
                ghost.DecideNextDestination(playerPosition);
            }
            else
            {
                ghost.DecideNextState(StateType.FRIGHTENED);
            }
        }
        else
        {
            MoveGhost(ghost);
        }
    }
}

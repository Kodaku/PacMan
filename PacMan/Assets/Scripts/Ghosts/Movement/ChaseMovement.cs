using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMovement : GhostMovement
{
    public ChaseMovement(float speed) : base(speed) {}

    public override void Move(Ghost ghost)
    {
        base.Move(ghost);
        Vector2 playerPosition = ghost.transform.position;
        if(Vector2.Distance(playerPosition, ghost.GetNextDestination()) <= 0.1f)
        {
            ghost.PlayAnimation(playerPosition);
            ghost.FindPath(PathFinderType.CHASE);
        }
        else
        {
            MoveGhost(ghost);
        }
    }
}

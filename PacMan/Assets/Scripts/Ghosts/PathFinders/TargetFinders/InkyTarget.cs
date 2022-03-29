using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkyTarget : GhostPathFinder
{
    public InkyTarget(MyGrid grid) : base(grid){}
    public override void FindPath(Distances distances, Vector2 destination)
    {
        base.FindPath(distances, destination);
        Vector2 blinkyTarget = destination;
        int choice = Random.Range(0, 1);
        Vector2 pinkyTarget = destination;
        if(choice == 0)
        {
            pinkyTarget.y += 4.0f;
        }
        else
        {
            pinkyTarget.x -= 4.0f;
        }
        pinkyTarget.x = Mathf.Clamp(pinkyTarget.x, 0.0f, m_grid.columns - 1);
        pinkyTarget.y = Mathf.Clamp(pinkyTarget.y, 0.0f, m_grid.rows - 1);
        Vector2 targetPosition = 2 * (blinkyTarget - pinkyTarget) + blinkyTarget;
        targetPosition.x = Mathf.Clamp(targetPosition.x, 0.0f, m_grid.columns - 1);
        targetPosition.y = Mathf.Clamp(targetPosition.y, 0.0f, m_grid.rows - 1);
        Distances distancesToPoint = distances.PathToGoal(m_grid.WorldPointToCell(targetPosition));
        List<Cell> cells = distancesToPoint.Cells();
        if(cells.Count > 0)
        {
            cells.Reverse();
            m_nextDestination = new Vector2(cells[0].column, cells[0].row);
        }
    }
}

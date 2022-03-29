using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkyTarget : GhostPathFinder
{
    public PinkyTarget(MyGrid grid) : base(grid){}
    public override void FindPath(Distances distances, Vector2 destination)
    {
        base.FindPath(distances, destination);
        int choice = Random.Range(0, 1);
        Vector2 targetPosition = destination;
        if(choice == 0)
        {
            targetPosition.y += 4.0f;
        }
        else
        {
            targetPosition.x -= 4.0f;
        }
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

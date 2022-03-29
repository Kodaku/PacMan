using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkyTarget : GhostPathFinder
{
    public BlinkyTarget(MyGrid grid) : base(grid){}
    public override void FindPath(Distances distances, Vector2 destination)
    {
        base.FindPath(distances, destination);
        Distances distancesToPoint = distances.PathToGoal(m_grid.WorldPointToCell(destination));
        List<Cell> cells = distancesToPoint.Cells();
        if(cells.Count > 0)
        {
            cells.Reverse();
            m_nextDestination = new Vector2(cells[0].column, cells[0].row);
        }
    }
}

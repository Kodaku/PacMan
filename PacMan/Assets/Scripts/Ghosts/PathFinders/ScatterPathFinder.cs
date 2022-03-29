using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterPathFinder : GhostPathFinder
{
    public ScatterPathFinder(MyGrid grid) : base(grid)
    {
        
    }
    public override void FindPath(Distances distances, Vector2 destination)
    {
        base.FindPath(distances, destination);
        Distances distancesToPoint = distances.PathToGoal(m_grid.WorldPointToCell(destination));
        foreach(Cell cell in distancesToPoint.Cells())
        {
            // Instantiate(debugSymbol, new Vector2(cell.column, cell.row), Quaternion.identity);
            Vector2 position = new Vector2(cell.column, cell.row);
            m_pathToDestination.Add(position);
        }
        m_pathToDestination.Reverse();
        m_nextDestination = m_pathToDestination[0];
        m_pathToDestination.Remove(m_nextDestination);
        if(m_pathToDestination.Count > 0)
        {
            m_pathToDestination.Remove(m_pathToDestination[m_pathToDestination.Count - 1]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPathFinder
{
    protected List<Vector2> m_pathToDestination = new List<Vector2>();
    protected Vector2 m_nextDestination;
    protected Cell m_currentCell;
    protected MyGrid m_grid;

    public GhostPathFinder(MyGrid grid)
    {
        m_grid = grid;
    }

    public Cell currentCell
    {
        get { return m_currentCell; }
        set { m_currentCell = value; }
    }

    public MyGrid grid
    {
        set { m_grid = value; }
    }

    public Vector2 nextDestination
    {
        get { return m_nextDestination; }
        set { m_nextDestination = value; }
    }

    public virtual Distances GetCurrentCellDistances(Ghost ghost)
    {
        m_currentCell = m_grid.WorldPointToCell(ghost.transform.position);
        return m_currentCell.Distances;
    }

    public void FindPathToPoint(Ghost ghost, Vector2 destination)
    {
        m_pathToDestination.Clear();
        Distances distances = GetCurrentCellDistances(ghost);
        FindPath(distances, destination);
    }

    public virtual void FindPath(Distances distances, Vector2 destination)
    {

    }

    public void SetNextDestinationFromPath()
    {
        m_nextDestination = m_pathToDestination[0];
        m_pathToDestination.Remove(m_nextDestination);
    }

    public bool IsEmpty()
    {
        return m_pathToDestination.Count == 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePathFinder : GhostPathFinder
{
    public EscapePathFinder(MyGrid grid) : base(grid){}
    public override void FindPath(Distances distances, Vector2 destination)
    {
        base.FindPath(distances, destination);
        Vector2 escapePosition = destination;
        int choice = Random.Range(0, 4);
        switch(choice)
        {
            case 0:
            {
                escapePosition.x += 5.0f;
                break;
            }
            case 1:
            {
                escapePosition.x -= 5.0f;
                break;
            }
            case 2:
            {
                escapePosition.y += 5.0f;
                break;
            }
            case 3:
            {
                escapePosition.y -= 5.0f;
                break;
            }
        }
        escapePosition.x = Mathf.Clamp(escapePosition.x, 0.0f, m_grid.columns - 1);
        escapePosition.y = Mathf.Clamp(escapePosition.y, 0.0f, m_grid.rows - 1);
        // print(escapePosition);
        Distances distancesToPoint = distances.PathToGoal(m_grid.WorldPointToCell(escapePosition));
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

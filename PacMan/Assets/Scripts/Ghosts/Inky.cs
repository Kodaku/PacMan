using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : Ghost
{
    public override void Initialize(int id)
    {
        base.Initialize(id);
        EntityManager.RegisterEntity(this);
    }

    public override void FindPathToScatterPoint()
    {
        base.FindPathToScatterPoint();
    }

    public override void MoveScatter()
    {
        base.MoveScatter();
    }

    public override void FindPathToTarget()
    {
        base.FindPathToTarget();
        Distances distances = m_currentCell.Distances;
        Vector2 blinkyTarget = target.transform.position;
        int choice = Random.Range(0, 1);
        Vector2 pinkyTarget = target.transform.position;
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
        m_distances = distances.PathToGoal(m_grid.WorldPointToCell(targetPosition));
        List<Cell> cells = m_distances.Cells();
        if(cells.Count > 0)
        {
            cells.Reverse();
            nextDestination = new Vector2(cells[0].column, cells[0].row);
        }
    }

    public override void Update()
    {
        base.Update();
    }
}

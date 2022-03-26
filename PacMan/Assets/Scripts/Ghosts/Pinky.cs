using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : Ghost
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
        int choice = Random.Range(0, 1);
        Vector2 targetPosition = target.transform.position;
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

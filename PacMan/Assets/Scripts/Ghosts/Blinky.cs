using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
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
        m_distances = distances.PathToGoal(m_grid.WorldPointToCell(target.transform.position));
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

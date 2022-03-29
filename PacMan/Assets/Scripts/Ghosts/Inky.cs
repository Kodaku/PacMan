using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : Ghost
{
    public override void Initialize(MyGrid grid)
    {
        base.Initialize(grid);
    }

    public override void InitializePathFinders(MyGrid grid)
    {
        base.InitializePathFinders(grid);
        pathFinders.Add(PathFinderType.CHASE, new InkyTarget(grid));
    }

    public override void MoveScatter()
    {
        base.MoveScatter();
    }

    public override void Update()
    {
        base.Update();
    }
}

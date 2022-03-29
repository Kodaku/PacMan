using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Ghost
{
    public override void Initialize(MyGrid grid)
    {
        base.Initialize(grid);
    }

    public override void InitializePathFinders(MyGrid grid)
    {
        base.InitializePathFinders(grid);
        pathFinders.Add(PathFinderType.CHASE, new ClydeTarget(grid));
    }

    public override void MoveScatter()
    {
        base.MoveScatter();
    }

    public override void MoveChase()
    {
        base.MoveChase();
        if(Vector2.Distance(transform.position, target.transform.position) <= 5.0f)
        {
            DecideNextState(StateType.SCATTER);
        }
    }

    public override void ChangeState(State<Ghost> ghost)
    {
        base.ChangeState(ghost);
    }

    public override void Update()
    {
        base.Update();
    }
}

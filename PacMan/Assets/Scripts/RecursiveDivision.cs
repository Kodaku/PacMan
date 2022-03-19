using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveDivision
{
    private MyGrid grid;

    public RecursiveDivision(MyGrid _grid)
    {
        grid = _grid;
    }
    public void On()
    {
        for(int i = 0; i < grid.rows; i++)
        {
            for(int j = 0; j < grid.columns; j++)
            {
                Cell cell = grid.CellAt(i, j);
                
            }
        }
    }
}

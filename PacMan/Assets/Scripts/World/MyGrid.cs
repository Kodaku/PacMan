using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MyGrid
{
    protected Cell[,] grid;
    private int m_rows;
    private int m_columns;
    [System.NonSerialized]
    private Distances m_distances;
    public Distances distances { get { return m_distances; } set { m_distances = value; } }

    public MyGrid(int rows, int columns)
    {
        m_rows = rows;
        m_columns = columns;
        PrepareGrid();
        ConfigureCells();
    }

    protected virtual void PrepareGrid()
    {
        grid = new Cell[m_rows, m_columns];
        for (var i = 0; i < m_rows; i++)
        {
            for (var j = 0; j < m_columns; j++)
            {
                grid[i, j] = new Cell(i, j);
            }
        }
    }

    public int rows
    {
        get { return m_rows; }
    }

    public int columns
    {
        get { return m_columns; }
    }

    private void ConfigureCells()
    {
        for(int i = 0; i < m_rows; i++)
        {
            for(int j = 0; j < m_columns; j++)
            {
                Cell cell = grid[i, j];
                if(i - 1 >= 0)
                {
                    cell.Unlink(CellLocation.SOUTH, grid[i - 1, j]);
                }
                if(j - 1 >= 0)
                {
                    cell.Unlink(CellLocation.WEST, grid[i, j - 1]);
                }
                if(i + 1 < m_rows)
                {
                    cell.Unlink(CellLocation.NORTH, grid[i + 1, j]);
                }
                if(j + 1 < m_columns)
                {
                    cell.Unlink(CellLocation.EAST, grid[i, j + 1]);
                }
                grid[i, j] = cell;
            }
        }
    }

    public Cell CellAt(int row, int column)
    {
        if(row >= 0 && row < rows && column >= 0 && column < columns)
        {
            return grid[row, column];
        }
        return null;
    }

    public Cell WorldPointToCell(Vector3 position)
    {
        int row = Mathf.RoundToInt(position.y);
        int column = Mathf.RoundToInt(position.x);
        return grid[row, column];
    }

    public void SetCellAt(int row, int column, Cell cell)
    {
        if(row >= 0 && row < rows && column >= 0 && column < columns)
        {
            grid[row, column] = cell;
        }
    }

    public Dictionary<CellLocation, Cell> GetCellNeighbors(int row, int column)
    {
        Cell current = grid[row, column];
        Dictionary<CellLocation, Cell> neighbors = new Dictionary<CellLocation, Cell>();

        if(row - 1 >= 0)
        {
            neighbors.Add(CellLocation.SOUTH, grid[row - 1, column]);
        }
        if(column - 1 >= 0)
        {
            neighbors.Add(CellLocation.WEST, grid[row, column - 1]);
        }
        if(row + 1 < m_rows)
        {
            neighbors.Add(CellLocation.NORTH, grid[row + 1, column]);
        }
        if(column + 1 < m_columns)
        {
            neighbors.Add(CellLocation.EAST, grid[row, column + 1]);
        }

        return neighbors;
    }

    public virtual Cell GetRandomCell
    {
        get
        {
            var i = Random.Range(0, m_rows - 1);
            var j = Random.Range(0, m_columns - 1);
            return grid[i, j];
        }
    }

    public virtual string ContentsOf(Cell cell)
    {
        return "   ";
    }

    public virtual string ToString(bool displayGridCoordinates)
    {
        return string.Empty;
    }

    public override string ToString()
    {
        return ToString(false);
    }

    public string ToDebug()
    {
        var output = string.Empty;

        return output;
    }

    public List<Cell> Deadends()
    {
        List<Cell> deadends = new List<Cell>();
        for(int i = 0; i < m_rows; i++)
        {
            for(int j = 0; j < m_columns; j++)
            {
                Cell cell = grid[i, j];
                if(cell.GetLinks().Count == 1)
                {
                    deadends.Add(cell);
                }
            }
        }

        return deadends;
    }
    
    public void Braid(float p=1.0f)
    {
        List<Cell> deadends = Deadends();
        for (int i = 0; i < deadends.Count; i++)
        {
            Cell temp = deadends[i];
            int randomIndex = Random.Range(i, deadends.Count);
            deadends[i] = deadends[randomIndex];
            deadends[randomIndex] = temp;
        }
        foreach(Cell cell in deadends)
        {
            float num = Random.Range(0.0f, 1.0f);
            if(cell.GetLinks().Count != 1 || num > p)
            {
                continue;
            }
            Dictionary<CellLocation, Cell> neighbours = GetCellNeighbors(cell.row, cell.column);
            Dictionary<CellLocation, Cell> notLinkedNeighbours = new Dictionary<CellLocation, Cell>();
            Dictionary<CellLocation, Cell> best = new Dictionary<CellLocation, Cell>();
            foreach(CellLocation cellLocation in neighbours.Keys)
            {
                Cell neighbour = neighbours[cellLocation];
                if(!cell.IsLinked(cellLocation))
                {
                    notLinkedNeighbours.Add(cellLocation, neighbour);
                }
            }
            foreach(CellLocation cellLocation in notLinkedNeighbours.Keys)
            {
                Cell neighbour = notLinkedNeighbours[cellLocation];
                if(neighbour.GetLinks().Count == 1)
                {
                    best.Add(cellLocation, neighbour);
                }
            }

            if(best.Count == 0)
            {
                best = notLinkedNeighbours;
            }

            int index = Random.Range(0, best.Keys.Count);
            Cell neighbor = best[best.Keys.ToArray()[index]];
            cell.Link(best.Keys.ToArray()[index]);
            SetCellAt(cell.row, cell.column, cell);
        }
    }

}

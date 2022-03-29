using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public GameObject cell;
    public GameObject cellWallNorth;
    public GameObject cellWallSouth;
    public GameObject cellWallEast;
    public GameObject cellWallWest;
    public GameObject maze;
    public GameObject breadcrumb;
    public GameObject specialBreadcrumb;
    public GameObject cherry;
    private GameObject currentMaze = null;

    public MyGrid InitializeGrid()
    {
        MyGrid grid = new MyGrid(25, 25);
        RecursiveBacktracker recursiveBacktracker = new RecursiveBacktracker ();
        recursiveBacktracker.On(ref grid);
        grid.Braid(p:1.0f);
        return grid;
    }

    public Dictionary<Tuple<int, int>, List<string>> BuildMaze(MyGrid grid)
    {
        // Debug.Log(maze);
        currentMaze = Instantiate(maze, Vector3.zero, Quaternion.identity);
        Dictionary<Tuple<int, int>, List<string>> world = new Dictionary<Tuple<int, int>, List<string>>();
        // breadcrumbs = new Dictionary<Vector3, GameObject>();
        for(int i = 0; i < grid.rows; i++)
        {
            for(int j = 0; j < grid.columns; j++)
            {
                List<string> cellState = new List<string>();
                Cell currentCell = grid.CellAt(i, j);
                Vector3 position = new Vector3(currentCell.column * 1, currentCell.row * 1, 0.0f);
                AddObjectToParentMaze(cell, position);
                AddObjectToParentMaze(breadcrumb, position);
                Dictionary<CellLocation, Cell> unlinkedCells = currentCell.GetUnlinks();
                foreach(CellLocation cellLocation in unlinkedCells.Keys)
                {
                    // print(cellLocation);
                    if(cellLocation == CellLocation.NORTH)
                    {
                        Vector3 tmpPosition = position;
                        tmpPosition.y += 0.451f;
                        AddObjectToParentMaze(cellWallNorth, tmpPosition);
                        cellState.Add("NorthBlocked");
                    }
                    else if(cellLocation == CellLocation.SOUTH)
                    {
                        Vector3 tmpPosition = position;
                        tmpPosition.y -= 0.451f;
                        AddObjectToParentMaze(cellWallSouth, tmpPosition);
                        cellState.Add("SouthBlocked");
                    }
                    else if(cellLocation == CellLocation.EAST)
                    {
                        Vector3 tmpPosition = position;
                        tmpPosition.x += 0.451f;
                        AddObjectToParentMaze(cellWallEast, tmpPosition);
                        cellState.Add("EastBlocked");
                    }
                    else if(cellLocation == CellLocation.WEST)
                    {
                        Vector3 tmpPosition = position;
                        tmpPosition.x -= 0.451f;
                        AddObjectToParentMaze(cellWallWest, tmpPosition);
                        cellState.Add("WestBlocked");
                    }
                }
                world.Add(Tuple.Create(currentCell.column, currentCell.row), cellState);
            }
        }
        return world;
    }

    public void AddSpecialBreadcrumbs(MyGrid grid)
    {
        Vector2 position1 = new Vector2(5, 5);
        Vector2 position2 = new Vector2(grid.columns - 5, 5);
        Vector2 position3 = new Vector2(5, grid.rows - 5);
        Vector2 position4 = new Vector2(grid.columns - 5, grid.rows - 5);
        AddObjectToParentMaze(specialBreadcrumb, position1);
        AddObjectToParentMaze(specialBreadcrumb, position2);
        AddObjectToParentMaze(specialBreadcrumb, position3);
        AddObjectToParentMaze(specialBreadcrumb, position4);
    }

    public void AddCherries(MyGrid grid)
    {
        Vector2 position1 = new Vector2(2, 2);
        Vector2 position2 = new Vector2(grid.columns - 6, 4);
        Vector2 position3 = new Vector2(7, grid.rows - 2);
        Vector2 position4 = new Vector2(grid.columns - 3, grid.rows - 4);
        AddObjectToParentMaze(cherry, position1);
        AddObjectToParentMaze(cherry, position2);
        AddObjectToParentMaze(cherry, position3);
        AddObjectToParentMaze(cherry, position4);
    }

    private void AddObjectToParentMaze(GameObject newObject, Vector3 position)
    {
        GameObject objectInstance = Instantiate(newObject, position, Quaternion.identity);
        objectInstance.transform.parent = currentMaze.transform;
    }

    private void AddCellToParentMaze(Vector3 position)
    {
        GameObject newCell = Instantiate(cell, position, Quaternion.identity);
        newCell.transform.parent = currentMaze.transform;
    }

    private void AddWallToParentMaze(GameObject wall, Vector3 position)
    {
        GameObject newWall = Instantiate(wall, position, Quaternion.identity);
        newWall.transform.parent = currentMaze.transform;
    }

    private void AddBreadcrumbToParentMaze(Game game, Vector3 position)
    {
        GameObject newBreadcrumb = Instantiate(breadcrumb, position, Quaternion.identity);
        newBreadcrumb.transform.parent = currentMaze.transform;
    }
    private void AddSpecialBreadcrumbToParentMaze(Game game, Vector3 position)
    {
        GameObject newSpecialBreadcrumb = Instantiate(specialBreadcrumb, position, Quaternion.identity);
        newSpecialBreadcrumb.transform.parent = currentMaze.transform;
    }

    private void AddCherryToParentMaze(Game game, Vector3 position)
    {
        GameObject newCherry = Instantiate(cherry, position, Quaternion.identity);
        newCherry.transform.parent = currentMaze.transform;
    }

    public void Reset()
    {
        if(currentMaze != null)
            Destroy(currentMaze);
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] ghosts;
    public GameObject pacMan;
    public GameObject maze;
    public GameObject cell;
    public GameObject cellWallEast;
    public GameObject cellWallSouth;
    public GameObject cellWallNorth;
    public GameObject cellWallWest;
    public GameObject breadcrumb;
    public GameObject specialBreadcrumb;
    private GameObject currentMaze;
    private Dictionary<Tuple<int, int>, List<string>> world;
    private Dictionary<Tuple<int, int>, GameObject> breadcrumbs = new Dictionary<Tuple<int, int>, GameObject>();
    private Dictionary<Tuple<int, int>, GameObject> specialBreadcrumbs = new Dictionary<Tuple<int, int>, GameObject>();
    private List<Ghost> spawnedGhosts = new List<Ghost>();
    private MyGrid grid;
    private int ghostIndex = 0;
    private float ghostTimer = 5.0f;
    private float currentGhostTimer = 0.0f;
    private int entityCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        grid = new MyGrid(25, 25);
        RecursiveBacktracker recursiveBacktracker = new RecursiveBacktracker ();
        recursiveBacktracker.On(ref grid);
        grid.Braid(p:1.0f);
        Initialize();
    }

    private void Initialize()
    {
        currentMaze = Instantiate(maze, Vector3.zero, Quaternion.identity);
        BuildMaze();
        AddSpecialBreadcrumbs();
        SpawnPacMan();
        SpawnGhost();
    }

    private void BuildMaze()
    {
        world = new Dictionary<Tuple<int, int>, List<string>>();
        // breadcrumbs = new Dictionary<Vector3, GameObject>();
        for(int i = 0; i < grid.rows; i++)
        {
            for(int j = 0; j < grid.columns; j++)
            {
                List<string> cellState = new List<string>();
                Cell currentCell = grid.CellAt(i, j);
                Vector3 position = new Vector3(currentCell.column * 1, currentCell.row * 1, 0.0f);
                AddCellToParentMaze(position);
                AddBreadcrumbToParentMaze(position);
                Dictionary<CellLocation, Cell> unlinkedCells = currentCell.GetUnlinks();
                foreach(CellLocation cellLocation in unlinkedCells.Keys)
                {
                    // print(cellLocation);
                    if(cellLocation == CellLocation.NORTH)
                    {
                        Vector3 tmpPosition = position;
                        tmpPosition.y += 0.451f;
                        AddWallToParentMaze(cellWallNorth, tmpPosition);
                        cellState.Add("NorthBlocked");
                    }
                    else if(cellLocation == CellLocation.SOUTH)
                    {
                        Vector3 tmpPosition = position;
                        tmpPosition.y -= 0.451f;
                        AddWallToParentMaze(cellWallSouth, tmpPosition);
                        cellState.Add("SouthBlocked");
                    }
                    else if(cellLocation == CellLocation.EAST)
                    {
                        Vector3 tmpPosition = position;
                        tmpPosition.x += 0.451f;
                        AddWallToParentMaze(cellWallEast, tmpPosition);
                        cellState.Add("EastBlocked");
                    }
                    else if(cellLocation == CellLocation.WEST)
                    {
                        Vector3 tmpPosition = position;
                        tmpPosition.x -= 0.451f;
                        AddWallToParentMaze(cellWallWest, tmpPosition);
                        cellState.Add("WestBlocked");
                    }
                }
                world.Add(Tuple.Create(currentCell.column, currentCell.row), cellState);
            }
        }
    }

    private void AddSpecialBreadcrumbs()
    {
        Vector2 position1 = new Vector2(5, 5);
        Vector2 position2 = new Vector2(grid.columns - 5, 5);
        Vector2 position3 = new Vector2(5, grid.rows - 5);
        Vector2 position4 = new Vector2(grid.columns - 5, grid.rows - 5);
        AddSpecialBreadcrumbToParentMaze(position1);
        AddSpecialBreadcrumbToParentMaze(position2);
        AddSpecialBreadcrumbToParentMaze(position3);
        AddSpecialBreadcrumbToParentMaze(position4);
    }

    private void AddCellToParentMaze(Vector3 position)
    {
        GameObject newCell = Instantiate(cell, position, Quaternion.identity);
        newCell.transform.parent = currentMaze.transform;
    }
    private void AddBreadcrumbToParentMaze(Vector3 position)
    {
        GameObject newBreadcrumb = Instantiate(breadcrumb, position, Quaternion.identity);
        newBreadcrumb.transform.parent = currentMaze.transform;
        breadcrumbs.Add(Tuple.Create((int)position.x, (int)position.y), newBreadcrumb);
    }
    private void AddSpecialBreadcrumbToParentMaze(Vector3 position)
    {
        Tuple<int, int> specialPosition = Tuple.Create((int)position.x, (int)position.y);
        GameObject newSpecialBreadcrumb = Instantiate(specialBreadcrumb, position, Quaternion.identity);
        newSpecialBreadcrumb.transform.parent = currentMaze.transform;
        specialBreadcrumbs.Add(specialPosition, newSpecialBreadcrumb);
        breadcrumbs.Remove(specialPosition);
    }
    private void AddWallToParentMaze(GameObject wall, Vector3 position)
    {
        GameObject newWall = Instantiate(wall, position, Quaternion.identity);
        newWall.transform.parent = currentMaze.transform;
    }

    private void SpawnGhost()
    {
        Cell ghostCell = grid.CellAt(Mathf.RoundToInt(grid.rows / 2), Mathf.RoundToInt(grid.columns / 2));
        Vector2 ghostPosition = new Vector2(ghostCell.column, ghostCell.row);
        GameObject newGhost = Instantiate(ghosts[ghostIndex], ghostPosition, Quaternion.identity);
        Ghost ghost = newGhost.GetComponent<Ghost>();
        ghost.grid = grid;
        Cell scatterCell = null;
        switch(ghost.ghostType)
        {
            case GhostType.BLINKY:
            {
                scatterCell = grid.CellAt(grid.rows - 4, grid.columns - 4);
                break;
            }
            case GhostType.INKY:
            {
                scatterCell = grid.CellAt(4, grid.columns - 4);
                break;
            }
            case GhostType.CLYDE:
            {
                scatterCell = grid.CellAt(4, 4);
                break;
            }
            case GhostType.PINKY:
            {
                scatterCell = grid.CellAt(grid.rows - 4, 4);
                break;
            }
        }
        ghost.currentCell = ghostCell;
        ghost.scatterPoint = new Vector2(scatterCell.column, scatterCell.row);
        ghost.Initialize(entityCount++);
        spawnedGhosts.Add(ghost);
        ghostIndex = (ghostIndex + 1) % ghosts.Length;
    }

    private void SpawnPacMan()
    {
        GameObject newPacMan = Instantiate(pacMan, Vector2.zero, Quaternion.identity);
        newPacMan.GetComponent<PacMan>().Initialize(entityCount++);
    }

    // Update is called once per frame
    void Update()
    {
        currentGhostTimer += Time.deltaTime;
        if(currentGhostTimer >= ghostTimer)
        {
            currentGhostTimer = 0.0f;
            if(spawnedGhosts.Count <= 5)
                SpawnGhost();
        }
    }
}

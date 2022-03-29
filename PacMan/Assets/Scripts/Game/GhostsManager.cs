using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostsManager : MonoBehaviour
{
    public GameObject[] ghostPrefabs;
    private int ghostIndex = 0;
    private int ghostCount = 0;
    private int maxGhostCount = 5;

    public void SpawnGhost(MyGrid grid)
    {
        // print(ghostCount);
        if(ghostCount < maxGhostCount)
        {
            Cell ghostCell = grid.CellAt(Mathf.RoundToInt(grid.rows / 2), Mathf.RoundToInt(grid.columns / 2));
            Vector2 ghostPosition = new Vector2(ghostCell.column, ghostCell.row);
            GameObject newGhost = Instantiate(ghostPrefabs[ghostIndex], ghostPosition, Quaternion.identity);
            Ghost ghost = newGhost.GetComponent<Ghost>();
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
            ghost.scatterPoint = new Vector2(scatterCell.column, scatterCell.row);
            ghost.Initialize(grid);
            ghost.RegisterEntity((int)Entities.GHOST);
            ghostCount++;
            ghostIndex = (ghostIndex + 1) % ghostPrefabs.Length;
        }
    }

    public void ClearGhosts()
    {
        ghostCount = 0;
        List<BaseGameEntity> ghosts = EntityManager.GetEntityByID((int)Entities.GHOST);
        foreach(BaseGameEntity ghost in ghosts)
        {
            Destroy(ghost.gameObject);
        }
        EntityManager.ClearEntities((int)Entities.GHOST);
    }

    public void DecreaseGhostCount()
    {
        ghostCount--;
    }
}

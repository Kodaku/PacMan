using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacManManager : MonoBehaviour
{
    public GameObject pacMan;
    private PacMan pacManScript;

    public void SpawnPacMan()
    {
        GameObject newPacMan = Instantiate(pacMan, Vector2.zero, Quaternion.identity);
        pacManScript = newPacMan.GetComponent<PacMan>();
        pacManScript.Initialize();
        pacManScript.RegisterEntity((int)Entities.PACMAN);
    }

    public void ResetPacMan()
    {
        pacManScript.ChangeState(Wander.Instance);
    }
}

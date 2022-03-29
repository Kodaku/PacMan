using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public Image[] lives;
    private List<Image> currentLives = new List<Image>();

    public void DecreaseLife()
    {
        // print(currentLives.Count);
        Image currentLife = currentLives[currentLives.Count - 1];
        currentLives.Remove(currentLife);
        currentLife.gameObject.SetActive(false);
    }

    public bool HasLostGame()
    {
        return currentLives.Count == 0;
    }

    public void ResetLives()
    {
        currentLives.Clear();
        foreach(Image life in lives)
        {
            life.gameObject.SetActive(true);
            currentLives.Add(life);
        }
    }
}

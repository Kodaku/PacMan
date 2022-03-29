using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    void Start()
    {
        SetScoreText();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        SetScoreText();
    }

    public void ResetScore()
    {
        score = 0;
        SetScoreText();
    }

    private void SetScoreText()
    {
        scoreText.text = score.ToString();
    }
}

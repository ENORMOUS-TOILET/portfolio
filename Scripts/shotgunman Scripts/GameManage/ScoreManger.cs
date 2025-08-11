using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManger : MonoBehaviour
{
    public Text scoreText;
    public int score;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        gameManager = gameObject.GetComponent<GameManager>();
        scoreText.text = "SCORE:" + score.ToString() + "/" + gameManager.goalScore;
    }

    public void AddScore()
    {
        score++;
        scoreText.text = "SCORE:" + score.ToString() + "/" + gameManager.goalScore;
    }
}

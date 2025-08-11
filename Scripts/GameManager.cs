using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI player1WinText;
    public TextMeshProUGUI player2WinText;
    private BallScore ballScore;
    private GameObject ball;
    public MainMenu mainMenu;
    public bool gameOver;

    void Start()
    {
        ball = GameObject.FindWithTag("Ball");
        ballScore = GameObject.FindWithTag("Ball").GetComponent<BallScore>();
        gameOver = false;
        scoreText.enabled = true;
        player1WinText.enabled = false;
        player2WinText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ballScore.player1Score >= 13)
            Player1Win();
        if (ballScore.player2Score >= 13)
            Player2Win();

        if (gameOver && mainMenu.pauseMenu.activeSelf)
        {
            player1WinText.enabled = false;
            player2WinText.enabled = false;
        }



    }

    public void Player1Win()
    {
        player1WinText.enabled = true;
        scoreText.enabled = false;
        ball.SetActive(false);
        gameOver = true;
    }

    public void Player2Win()
    {
        player2WinText.enabled = true;
        scoreText.enabled = false;
        ball.SetActive(false);
        gameOver = true;
    }
}

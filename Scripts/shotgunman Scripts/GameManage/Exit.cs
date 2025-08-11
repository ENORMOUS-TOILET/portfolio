using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private GameObject playerGO;
    private Player player;
    private GameObject gameManagerGO;
    public GameManager gameManager;
    public ScoreManger scoreManger;

    private void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        gameManagerGO = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = gameManagerGO.GetComponent<GameManager>();
        scoreManger = gameManagerGO.GetComponent<ScoreManger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (canWin())
            {
                gameManager.Win();
            }
        }
    }

    public bool canWin()
    {
        if (scoreManger.score > gameManager.goalScore - 1)
            return true;
        else
            return false;
    }
}

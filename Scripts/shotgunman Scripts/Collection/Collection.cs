using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    public ScoreManger scoreManger;
    public GameObject gameManager;
    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        scoreManger = gameManager.GetComponent<ScoreManger>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            scoreManger.AddScore();
            Destroy(gameObject);
        }
    }
}

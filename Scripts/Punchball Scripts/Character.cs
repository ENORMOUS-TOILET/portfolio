using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxHealth = 5;
    public float currentHealth = 5;
    private Vector2 player1SpawnPositon = new Vector2(-5, 0);
    private Vector2 player2SpawnPositon = new Vector2(5, 0);

    void Start()
    {
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            Death();
        }
    }

    public void GetHit()
    {
        currentHealth -= 1;
        Debug.Log("Player±»»÷ÖÐÁË");
    }

    void Death()
    {
        if(gameObject.CompareTag("Player1"))
        {
            transform.position = player1SpawnPositon;
        }
        else if (gameObject.CompareTag("Player2"))
        {
            transform.position = player2SpawnPositon;
        }

        currentHealth = maxHealth;
    }
}

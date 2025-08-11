using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoChest : MonoBehaviour
{
    public GameObject gameManager;
    private GameObject playerGO;
    private Player player;
    public int minAmmoAddCount = 2;
    public int maxAmmoAddCount = 5;
    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        playerGO = GameObject.FindWithTag("Player");
        player = playerGO.GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddAmmoForPlayer();
            Destroy(gameObject);
        }
    }

    private void AddAmmoForPlayer()
    {
        int ammoToAdd = Random.Range(minAmmoAddCount, maxAmmoAddCount + 1);
        player.ammoCount += ammoToAdd;
    }
}

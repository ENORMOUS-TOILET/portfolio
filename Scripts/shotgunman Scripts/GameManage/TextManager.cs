using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Text ammoCountText;
    public Text winText;
    public Text scoreText;
    public Text loseText;
    public Text HPText;
    private GameObject playerGO;
    private GameManager gameManager;
    public Player player;

    private void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        gameManager = gameObject.GetComponent<GameManager>();
        player = playerGO.GetComponent<Player>();
    }

    private void Update()
    {
        ammoCountText.text = "Ammo: " + player.ammoCount;
        HPText.text = "HP:" + gameManager.playerCharacter.currentHP;
    }
}

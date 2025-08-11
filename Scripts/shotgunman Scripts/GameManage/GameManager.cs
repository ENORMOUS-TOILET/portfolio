using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int goalScore;
    private TextManager textManager;
    private GameObject playerGO;
    public Player player;
    public Character playerCharacter;

    private void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = playerGO.GetComponent<Player>();
        playerCharacter = player.GetComponent<Character>();
        textManager = gameObject.GetComponent<TextManager>();
    }

    private void Update()
    {
        if (playerCharacter.currentHP <= 0)
            Lose();
    }

    public void Win()
    {
        //检测当前是否为最后一个场景
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            disableTexts();
            textManager.winText.enabled = true;
            Time.timeScale = 0;
            Debug.Log("Game Win!");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void Lose()
    {
        disableTexts();
        textManager.loseText.enabled = true;
        Time.timeScale = 0;
        Debug.Log("Game Lose");
    }

    private void disableTexts()
    {
        textManager.scoreText.enabled = false;
        textManager.ammoCountText.enabled = false;
        textManager.player.enabled = false;
        textManager.HPText.enabled = false;
    }
}

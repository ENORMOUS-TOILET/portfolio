using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间

public class BallScore : MonoBehaviour
{
    public int player1Score = 0; // 玩家1的分数
    public int player2Score = 0; // 玩家2的分数

    public TMP_Text scoreText; // 引用 TextMeshPro 文本组件

    void Start()
    {
        UpdateScoreUI(); // 初始化时更新分数显示
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RightGate"))
        {
            player1Score++;
            ResetBallPosition();
            UpdateScoreUI();
        }
        else if (collision.gameObject.CompareTag("LeftGate"))
        {
            player2Score++;
            ResetBallPosition();
            UpdateScoreUI();
        }
    }

    void ResetBallPosition()
    {
        transform.position = Vector2.zero;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    void UpdateScoreUI()
    {
        // 更新分数显示
        scoreText.text = player1Score + " - " + player2Score;
    }
}

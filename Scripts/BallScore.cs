using UnityEngine;
using TMPro; // ���� TextMeshPro �����ռ�

public class BallScore : MonoBehaviour
{
    public int player1Score = 0; // ���1�ķ���
    public int player2Score = 0; // ���2�ķ���

    public TMP_Text scoreText; // ���� TextMeshPro �ı����

    void Start()
    {
        UpdateScoreUI(); // ��ʼ��ʱ���·�����ʾ
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
        // ���·�����ʾ
        scoreText.text = player1Score + " - " + player2Score;
    }
}

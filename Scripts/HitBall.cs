using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBall : MonoBehaviour
{
    public float punchForce = 10f; // ��������
    public string targetPlayerStr;
    public string thisPlayerStr;
    private GameObject targetPlayerGO;
    private GameObject thisPlayerGO;
    private GameObject punch;
    public Character targetPlayerCharacter;

    private void Start()
    {
        targetPlayerGO = GameObject.FindWithTag(targetPlayerStr);
        thisPlayerGO = GameObject.FindWithTag(thisPlayerStr);
        punch = this.gameObject;
        targetPlayerCharacter = targetPlayerGO.GetComponent<Character>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����ײ�����Ƿ�����
        if (collision.gameObject.CompareTag("Ball"))
        {
            // ��ȡ��ĸ������
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (ballRb != null)
            {
                // ������˷���
                Vector2 punchDirection = collision.transform.position - thisPlayerGO.transform.position;
                punchDirection.Normalize();

                // ʩ����������
                ballRb.AddForce(punchDirection * punchForce, ForceMode2D.Impulse);
            }
        }

        if (collision.gameObject.CompareTag(targetPlayerStr))
        {
            Rigidbody2D playerRB = targetPlayerGO.GetComponent<Rigidbody2D>();

            Vector2 pushPlayerDirection = targetPlayerGO.transform.position - thisPlayerGO.transform.position;

            pushPlayerDirection.Normalize();

            playerRB.AddForce(pushPlayerDirection * 50, ForceMode2D.Impulse);
            Debug.Log(pushPlayerDirection);

            targetPlayerCharacter.GetHit();
        }


    }
}

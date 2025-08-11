using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBall : MonoBehaviour
{
    public float punchForce = 10f; // 击退力度
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
        // 检查碰撞对象是否是球
        if (collision.gameObject.CompareTag("Ball"))
        {
            // 获取球的刚体组件
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (ballRb != null)
            {
                // 计算击退方向
                Vector2 punchDirection = collision.transform.position - thisPlayerGO.transform.position;
                punchDirection.Normalize();

                // 施加力到球上
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

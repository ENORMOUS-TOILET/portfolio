using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float initialSpeed = 5f;
    private Rigidbody2D ballRB;
    public float decelerationRate = 0.1f; // ������
    public float minimumSpeed = 0.1f; // ��С�ٶȣ����ڴ��ٶ���ֹͣ��
    public float dragAmount = 0.5f;

    void Start()
    {
        ballRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ApplySlowDown();
    }

    void AvoidStuck()
    {
        if (ballRB.velocity.y == 0 && ballRB.velocity.x != 0)
        {
            if (gameObject.transform.position.y < 0)
            {
                ballRB.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
            }
            else
            {
                ballRB.AddForce(new Vector2(0,-1), ForceMode2D.Impulse);
            }
        }
        if (ballRB.velocity.y != 0 && ballRB.velocity.x == 0)
        {
            if (gameObject.transform.position.x < 0)
            {
                ballRB.AddForce(new Vector2(1, 0), ForceMode2D.Impulse);
            }
            else
            {
                ballRB.AddForce(new Vector2(-1, 0), ForceMode2D.Impulse);
            }
        }
    }
    

    //���һ��������
    void RandomStartBall()
    {
        // ����һ���������ĳ��ٶ�
        //ballRB.velocity = Random.insideUnitCircle.normalized * initialSpeed;
        ballRB.velocity = new Vector2(1, 0) * initialSpeed;
    }

    //������
    void RetardBall()
    {
        if (ballRB.velocity.magnitude > minimumSpeed)
        {
            // �𽥼�������ٶ�
            ballRB.velocity = ballRB.velocity * (1 - decelerationRate * Time.deltaTime);
        }
        else
        {
            // ���ٶ��㹻С��ʱ�򣬽��ٶ���Ϊ0��ֹͣ��
            ballRB.velocity = Vector2.zero;
        }
    }

    void ApplySlowDown()
    {
        if (ballRB.velocity.magnitude > 0.1f) // ������ٶȴ���ĳ��ֵʱ��Ӧ����ק
        {
            ballRB.drag = dragAmount;
        }
        else
        {
            ballRB.drag = 0; // ���ٶȷǳ�С��ʱ��ֹͣ��ק
            ballRB.velocity = Vector2.zero; // ȷ���ٶ�����Ϊ0
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float initialSpeed = 5f;
    private Rigidbody2D ballRB;
    public float decelerationRate = 0.1f; // 减速率
    public float minimumSpeed = 0.1f; // 最小速度，低于此速度则停止球
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
    

    //随机一个方向开球
    void RandomStartBall()
    {
        // 设置一个随机方向的初速度
        //ballRB.velocity = Random.insideUnitCircle.normalized * initialSpeed;
        ballRB.velocity = new Vector2(1, 0) * initialSpeed;
    }

    //减速球
    void RetardBall()
    {
        if (ballRB.velocity.magnitude > minimumSpeed)
        {
            // 逐渐减少球的速度
            ballRB.velocity = ballRB.velocity * (1 - decelerationRate * Time.deltaTime);
        }
        else
        {
            // 当速度足够小的时候，将速度设为0，停止球
            ballRB.velocity = Vector2.zero;
        }
    }

    void ApplySlowDown()
    {
        if (ballRB.velocity.magnitude > 0.1f) // 当球的速度大于某个值时，应用拖拽
        {
            ballRB.drag = dragAmount;
        }
        else
        {
            ballRB.drag = 0; // 当速度非常小的时候，停止拖拽
            ballRB.velocity = Vector2.zero; // 确保速度最终为0
        }
    }
}

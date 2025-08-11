using UnityEngine;
using System.Collections;
using TMPro;

public class Player1Punch : MonoBehaviour
{
    //拳头锁定使用
    public GameObject[] potentialTargets; // 所有可以锁定的目标
    private Vector2 punchDirection; // 出拳的方向
    private GameObject currentTarget; // 当前锁定的目标
    public int currentTargetIndex = 0; // 当前锁定的目标索引

    public GameObject aimCircleBlue;//指示圈
    public float indicatorMoveSpeed = 20f; // 指示圈移动的速度
    private Rigidbody2D aimCircleRB;

    public GameObject punchPrefab; // 拳头预制体
    public float punchSpeed = 10f; // 拳头移动速度
    public float punchLifetime = 0.5f; // 拳头存在的时间
    public float punchCoolDownTime = 1;
    private float punchCoolDownTimer = 0;
    private bool punchReady;

    private void Start()
    {
        aimCircleRB = aimCircleBlue.GetComponent<Rigidbody2D>();
        // 默认锁定第一个目标
        if (potentialTargets.Length > 0)
        {
            currentTarget = potentialTargets[currentTargetIndex];
            aimCircleBlue.transform.position = currentTarget.transform.position;
        }
    }

    void Update()
    {
        // 检测按下J键后出拳
        if (Input.GetKeyDown(KeyCode.J) && punchReady)
        {
            Punch();
        }
        PunchCoolDown();

        // 玩家1按下K键切换锁定目标
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchTarget();
        }

    }

    private void FixedUpdate()
    {
        AimCircleMovement();
    }

    void Punch()
    {
        // 查找场景中的球对象
        GameObject ball = GameObject.FindWithTag("Ball");

        if (currentTarget != null)
        {
            punchCoolDownTimer = punchCoolDownTime;

            // 计算拳头生成的位置和方向
            Vector2 punchDirection = (currentTarget.transform.position - transform.position).normalized;

            // 计算拳头的旋转角度，使其朝向球
            float angle = Mathf.Atan2(punchDirection.y, punchDirection.x) * Mathf.Rad2Deg;

            // 生成拳头并设置其旋转角度
            GameObject punch = Instantiate(punchPrefab, transform.position, Quaternion.Euler(0, 0, angle - 90));

            // 设置拳头的移动
            Rigidbody2D rb = punch.GetComponent<Rigidbody2D>();
            rb.velocity = punchDirection * punchSpeed;

            // 在一定时间后销毁拳头
            Destroy(punch, punchLifetime);
        }
    }


    //拳头CD设置
    void PunchCoolDown()
    {
        punchCoolDownTimer -= Time.deltaTime;
        if (punchCoolDownTimer <= 0)
        {
            punchReady = true;
        }
        else
        {
            punchReady = false;
        }
    }


    //切换锁定目标
    void SwitchTarget()
    {
        if (potentialTargets.Length > 0)
        {
            currentTargetIndex = (currentTargetIndex + 1) % potentialTargets.Length; // 循环切换目标
            currentTarget = potentialTargets[currentTargetIndex];
            Debug.Log("当前锁定目标：" + currentTarget.name);

        }
    }


    //指示圈向目标点移动
    void AimCircleMovement()
    {
        Vector2 aimCirclePosition = aimCircleRB.position;
        Vector2 aimCircleDirection = (currentTarget.transform.position - aimCircleBlue.transform.position).normalized;
        Vector2 aimCircleMovement = aimCircleDirection * indicatorMoveSpeed * Time.fixedDeltaTime;
        // 确保不会超过目标位置
        if (Vector2.Distance(aimCircleBlue.transform.position, currentTarget.transform.position) <= aimCircleMovement.magnitude)
        {
            // 如果已经接近目标位置，直接移动到目标位置
            aimCircleRB.MovePosition(currentTarget.transform.position);
        }
        else
        {
            // 持续移动到目标
            aimCircleRB.MovePosition(aimCirclePosition + aimCircleMovement);
        }
    }
}

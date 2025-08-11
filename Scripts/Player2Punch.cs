using UnityEngine;
using System.Collections;

public class Player2Punch : MonoBehaviour
{
    //拳头锁定使用
    public GameObject[] potentialTargets; // 所有可以锁定的目标
    private Vector2 punchDirection; // 出拳的方向
    private GameObject currentTarget; // 当前锁定的目标
    public int currentTargetIndex = 0; // 当前锁定的目标索引

    public GameObject aimCircleRed;//指示圈
    public float indicatorMoveSpeed = 20f; // 指示圈移动的速度
    private Rigidbody2D aimCircleRB;


    public GameObject punchPrefab;
    public float punchSpeed = 10f;
    public float punchLifetime = 0.5f;
    public float punchCoolDownTime = 1;
    private float punchCoolDownTimer = 0;
    private bool punchReady;

    private void Start()
    {
        aimCircleRB = aimCircleRed.GetComponent<Rigidbody2D>();
        // 默认锁定第一个目标
        if (potentialTargets.Length > 0)
        {
            currentTarget = potentialTargets[currentTargetIndex];
            aimCircleRed.transform.position = currentTarget.transform.position;
        }
    }

    void Update()
    {
        // 使用小键盘的1键进行出拳
        if (Input.GetKeyDown(KeyCode.Keypad1) && punchReady)
        {
            Punch();
        }

        // 玩家2按下2键切换锁定目标
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SwitchTarget();
        }
        PunchCoolDown();
    }

    private void FixedUpdate()
    {
        AimCircleMovement();
    }

    void Punch()
    {
        GameObject ball = GameObject.FindWithTag("Ball");

        if (currentTarget != null)
        {
            punchCoolDownTimer = punchCoolDownTime;

            Vector2 punchDirection = (currentTarget.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(punchDirection.y, punchDirection.x) * Mathf.Rad2Deg;

            GameObject punch = Instantiate(punchPrefab, transform.position, Quaternion.Euler(0, 0, angle - 90));

            Rigidbody2D rb = punch.GetComponent<Rigidbody2D>();
            rb.velocity = punchDirection * punchSpeed;

            Destroy(punch, punchLifetime);
        }
    }
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
        Vector2 aimCircleDirection = (currentTarget.transform.position - aimCircleRed.transform.position).normalized;
        Vector2 aimCircleMovement = aimCircleDirection * indicatorMoveSpeed * Time.fixedDeltaTime;
        // 确保不会超过目标位置
        if (Vector2.Distance(aimCircleRed.transform.position, currentTarget.transform.position) <= aimCircleMovement.magnitude)
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

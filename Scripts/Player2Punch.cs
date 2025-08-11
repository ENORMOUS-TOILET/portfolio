using UnityEngine;
using System.Collections;

public class Player2Punch : MonoBehaviour
{
    //ȭͷ����ʹ��
    public GameObject[] potentialTargets; // ���п���������Ŀ��
    private Vector2 punchDirection; // ��ȭ�ķ���
    private GameObject currentTarget; // ��ǰ������Ŀ��
    public int currentTargetIndex = 0; // ��ǰ������Ŀ������

    public GameObject aimCircleRed;//ָʾȦ
    public float indicatorMoveSpeed = 20f; // ָʾȦ�ƶ����ٶ�
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
        // Ĭ��������һ��Ŀ��
        if (potentialTargets.Length > 0)
        {
            currentTarget = potentialTargets[currentTargetIndex];
            aimCircleRed.transform.position = currentTarget.transform.position;
        }
    }

    void Update()
    {
        // ʹ��С���̵�1�����г�ȭ
        if (Input.GetKeyDown(KeyCode.Keypad1) && punchReady)
        {
            Punch();
        }

        // ���2����2���л�����Ŀ��
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
            currentTargetIndex = (currentTargetIndex + 1) % potentialTargets.Length; // ѭ���л�Ŀ��
            currentTarget = potentialTargets[currentTargetIndex];
            Debug.Log("��ǰ����Ŀ�꣺" + currentTarget.name);
        }
    }


    //ָʾȦ��Ŀ����ƶ�
    void AimCircleMovement()
    {
        Vector2 aimCirclePosition = aimCircleRB.position;
        Vector2 aimCircleDirection = (currentTarget.transform.position - aimCircleRed.transform.position).normalized;
        Vector2 aimCircleMovement = aimCircleDirection * indicatorMoveSpeed * Time.fixedDeltaTime;
        // ȷ�����ᳬ��Ŀ��λ��
        if (Vector2.Distance(aimCircleRed.transform.position, currentTarget.transform.position) <= aimCircleMovement.magnitude)
        {
            // ����Ѿ��ӽ�Ŀ��λ�ã�ֱ���ƶ���Ŀ��λ��
            aimCircleRB.MovePosition(currentTarget.transform.position);
        }
        else
        {
            // �����ƶ���Ŀ��
            aimCircleRB.MovePosition(aimCirclePosition + aimCircleMovement);
        }
    }
}

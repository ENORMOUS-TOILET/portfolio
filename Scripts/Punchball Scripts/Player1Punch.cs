using UnityEngine;
using System.Collections;
using TMPro;

public class Player1Punch : MonoBehaviour
{
    //ȭͷ����ʹ��
    public GameObject[] potentialTargets; // ���п���������Ŀ��
    private Vector2 punchDirection; // ��ȭ�ķ���
    private GameObject currentTarget; // ��ǰ������Ŀ��
    public int currentTargetIndex = 0; // ��ǰ������Ŀ������

    public GameObject aimCircleBlue;//ָʾȦ
    public float indicatorMoveSpeed = 20f; // ָʾȦ�ƶ����ٶ�
    private Rigidbody2D aimCircleRB;

    public GameObject punchPrefab; // ȭͷԤ����
    public float punchSpeed = 10f; // ȭͷ�ƶ��ٶ�
    public float punchLifetime = 0.5f; // ȭͷ���ڵ�ʱ��
    public float punchCoolDownTime = 1;
    private float punchCoolDownTimer = 0;
    private bool punchReady;

    private void Start()
    {
        aimCircleRB = aimCircleBlue.GetComponent<Rigidbody2D>();
        // Ĭ��������һ��Ŀ��
        if (potentialTargets.Length > 0)
        {
            currentTarget = potentialTargets[currentTargetIndex];
            aimCircleBlue.transform.position = currentTarget.transform.position;
        }
    }

    void Update()
    {
        // ��ⰴ��J�����ȭ
        if (Input.GetKeyDown(KeyCode.J) && punchReady)
        {
            Punch();
        }
        PunchCoolDown();

        // ���1����K���л�����Ŀ��
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
        // ���ҳ����е������
        GameObject ball = GameObject.FindWithTag("Ball");

        if (currentTarget != null)
        {
            punchCoolDownTimer = punchCoolDownTime;

            // ����ȭͷ���ɵ�λ�úͷ���
            Vector2 punchDirection = (currentTarget.transform.position - transform.position).normalized;

            // ����ȭͷ����ת�Ƕȣ�ʹ�䳯����
            float angle = Mathf.Atan2(punchDirection.y, punchDirection.x) * Mathf.Rad2Deg;

            // ����ȭͷ����������ת�Ƕ�
            GameObject punch = Instantiate(punchPrefab, transform.position, Quaternion.Euler(0, 0, angle - 90));

            // ����ȭͷ���ƶ�
            Rigidbody2D rb = punch.GetComponent<Rigidbody2D>();
            rb.velocity = punchDirection * punchSpeed;

            // ��һ��ʱ�������ȭͷ
            Destroy(punch, punchLifetime);
        }
    }


    //ȭͷCD����
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


    //�л�����Ŀ��
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
        Vector2 aimCircleDirection = (currentTarget.transform.position - aimCircleBlue.transform.position).normalized;
        Vector2 aimCircleMovement = aimCircleDirection * indicatorMoveSpeed * Time.fixedDeltaTime;
        // ȷ�����ᳬ��Ŀ��λ��
        if (Vector2.Distance(aimCircleBlue.transform.position, currentTarget.transform.position) <= aimCircleMovement.magnitude)
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

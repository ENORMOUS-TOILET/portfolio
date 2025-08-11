using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;



public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }

    public EnemyStateMachine stateMachine { get; private set; }

    public EnemyChaseState chaseState { get; private set; }
    public EnemyAttackPrepareState attackPrepareState { get; private set; }
    public EnemyAttackState attackState { get; private set; }
    public EnemyCircleRoundState circleRoundState { get; private set; }
    public EnemyPatrolState patrolState { get; private set; }
    public EnemyDeadState deadState { get; private set; }

    public GameObject bloodEffect;
    public GameObject playerGO;
    public GameObject enemyGO;
    private Character thisCharacter;
    public SpriteRenderer sr;
    private BoxCollider2D thisCollider;

    //�õ��˺���ҵľ���
    public float distanceToPlayer { get; private set; }
    //�����ƶ�����
    private Vector2 moveDirection;

    [Header("���˻�������")]
    //����׷������
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    public float attackDetermineRange;
    [SerializeField] private float sideOffset;
    public SpriteRenderer weaponSR;
    [HideInInspector]public bool isDead;

    [Header("����״̬����")]
    //����״̬����
    //��������������ҵľ���
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;
    [SerializeField] private float circleRoundSpeed;
    public float circleRoundTime;
    public Vector2 circleRoundPosition { get; private set; }//����Ŀ���

    [Header("����׼��״̬����")]
    //����׼��״̬����
    //���˾�����Ҳ�������ܹ���
    [SerializeField] private float attackRangeTolerance;
    //�Ƿ񵽴���Ҳ��棬�����˳�AttackPrepare״̬
    [HideInInspector]public bool reachSide;

    //a��Ѱ·����
    [Header("A��Ѱ·����")]
    public float pathGenerateInterval = 0.5f;//·�����ɼ��
    private Seeker seeker;
    private List<Vector3> pathPointList;//·�����б�
    private int currentPathIndex;//��ǰ·��������
    private float pathGenerateTimer = 0;//·�����ɼ�ʱ��
    [HideInInspector]public Vector2 currentTarget;//��ǰĿ���

    [Header("Ѳ�߱���")]
    public float maxPatrolDistance;
    [HideInInspector]public float distanceToCurrentPatrolPoint;
    [HideInInspector]public List<Vector2> patrolPointList;
    [HideInInspector]public int currentPatrolIndex;
    public Transform patrolPointTransform;
    public float chaseDeterminRange;

    [Header("��Ұ�������")]
    public float viewRadius;
    public float viewAngle;

    public bool angleCheck;
    public bool distanceCheck;
    public bool hitCheck;
    private bool a;




    private void Awake()
    {
        stateMachine = new EnemyStateMachine();
        chaseState = new EnemyChaseState(this, stateMachine, "Move");
        attackPrepareState = new EnemyAttackPrepareState(this,stateMachine,"Move");
        circleRoundState = new EnemyCircleRoundState(this, stateMachine, "Move");
        patrolState = new EnemyPatrolState(this, stateMachine, "Move");
        attackState = new EnemyAttackState(this, stateMachine, "Attack");
        deadState = new EnemyDeadState(this, stateMachine, "Idle");

        playerGO = GameObject.FindWithTag("Player");
        patrolPointTransform = GameObject.FindWithTag("PatrolPoint").transform;

        thisCharacter = gameObject.GetComponent<Character>();
        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        thisCollider = gameObject.GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        seeker = gameObject.GetComponent<Seeker>();

        enemyGO = gameObject;
    }

    private void Start()
    {
        reachSide = false;
        isDead = false;
        stateMachine.Initialize(circleRoundState);

        InitializePatrolPoint();
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        CalculateDistanceToPlayer();

        a = CanChasePlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in thisCharacter.harmTags)
        {
            if (collision.CompareTag(tag))
            {
                // ���ƥ�䣬��ִ����Ӧ�ķ���
                thisCharacter.currentHP--;
                GetHit();
                break; // �ҵ�ƥ��ı�ǩ������ѭ��
            }
        }
    }

    //����
    public void GetHit()
    {
        if (isDead)
            return;
        if(thisCharacter.currentHP < 0)
        {
            Death();
            return;
        }
        Vector2 direction = transform.position - playerGO.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Instantiate(bloodEffect, transform.position, Quaternion.Euler(-angle, 90, 0));
    }

    //����
    private void Death()
    {
        SetColorFromHex("#8C8C8C", sr);
        SetColorFromHex("#8C8C8C", weaponSR);
        SetOpacity(0.5f, sr);
        SetOpacity(0.5f, weaponSR);
        rb.velocity = Vector2.zero;
        moveSpeed = 0;
        isDead = true;
        Destroy(enemyGO, 2f);
    }

    //ʹ����ɫ����������ɫ
    void SetColorFromHex(string _hexCode,SpriteRenderer _sr)
    {
        Color color;

        // ��ʮ��������ɫ����ת��ΪColor����
        if (ColorUtility.TryParseHtmlString(_hexCode, out color))
        {
            _sr.color = color;
        }
        else
        {
            Debug.LogError("��Ч��ʮ��������ɫ����");
        }
    }

    // ����͸���ȣ�Alphaֵ��
    void SetOpacity(float _alpha,SpriteRenderer _sr)
    {
        // ��ȡ��ǰ����ɫ
        Color currentColor = sr.color;

        // ����һ���µ���ɫ������ԭ���� RGB ֵ��ֻ�޸� Alpha
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, _alpha);

        // �����µ���ɫ
        _sr.color = newColor;
    }

    //�������ҵľ���
    private void CalculateDistanceToPlayer()
    {
        Vector2 _thisToPlayer = playerGO.transform.position - transform.position;
        distanceToPlayer = _thisToPlayer.magnitude;
    }


    //�����Ѳ�ߵ�ľ���
    public void CalculateDistanceToPatrolPoint()
    {
        if (patrolPointList == null || patrolPointList.Count == 0)
        {
            return;
        }
        distanceToCurrentPatrolPoint = Vector2.Distance(transform.position, patrolPointList[currentPatrolIndex]);
    }

    //�������
    public void FacePlayer()
    {
        // ��ȡ�������Ļ�ϵ�λ��
        Vector3 playerPositon = playerGO.transform.position;
        // ��ȡ����ĵ�ǰλ��
        Vector3 direction = playerPositon - transform.position;
        // ����Ŀ����ת�Ƕ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // �����������ת
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
    }

    //����ǰ·����
    public void FacePathPoint()
    {
        if (pathPointList == null || pathPointList.Count == 0 || currentPathIndex >= pathPointList.Count - 1)
        {
            return;
        }
        Vector3 _targetPosition = pathPointList[currentPathIndex + 1];
        Vector3 _direction = _targetPosition - transform.position;
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
    }

    //׷��Ŀ��
    public void ChasePlayer()
    {
        if (distanceToPlayer > attackDetermineRange)
        {
            moveDirection = playerGO.transform.position - transform.position;
        }
        else
        {
            moveDirection = playerGO.transform.position + playerGO.transform.right * sideOffset;
        }
        rb.velocity = moveDirection.normalized * moveSpeed;
    }

    //ͨ��·�����ƶ�
    public void MoveToPathPoint()
    {
        AutoPath();
        if (pathPointList == null || pathPointList.Count - 1 <= 0)
        {
            return;
        }
        Vector3 _targetPosition = pathPointList[currentPathIndex];
        moveDirection = (Vector2)_targetPosition - (Vector2)transform.position;
        rb.velocity = moveDirection.normalized * moveSpeed;
    }
    

    //�Ƶ���Ҳ��湥����δ����
    public void MoveToPlayerDirectly()
    {
        Vector2 _moveDirection = playerGO.transform.position - transform.position;
        rb.velocity = _moveDirection.normalized * moveSpeed;
    }
    public void MoveToPlayerRightSide()
    {
        Vector2 _targetPosition = playerGO.transform.position + playerGO.transform.right * sideOffset;
        moveDirection = _targetPosition - (Vector2)transform.position;
        rb.velocity = moveDirection.normalized * moveSpeed;
        if (Vector2.Distance(transform.position, _targetPosition) < attackRangeTolerance || Vector2.Distance(transform.position, _targetPosition) < attackRangeTolerance)
            reachSide = true;
    }
    public void MoveToPlayerLeftSide()
    {
        Vector2 _targetPosition = playerGO.transform.position + -playerGO.transform.right * sideOffset;
        moveDirection = _targetPosition - (Vector2)transform.position;
        rb.velocity = moveDirection.normalized * moveSpeed;
        if (Vector2.Distance(transform.position, _targetPosition) < attackRangeTolerance || Vector2.Distance(transform.position, _targetPosition) < attackRangeTolerance)
            reachSide = true;
    }

    //���û��ʼ��������λ��
    public void SetCircleRoundPoint()
    {
        Vector2 _playerPosition = playerGO.transform.position;
        Vector2 _enemyPosition = enemyGO.transform.position;

        // ������˷���
        Vector2 directionToEnemy = (_enemyPosition - _playerPosition).normalized;

        // ���ѡ��һ���Ƕȣ����޿������˵İ�Բ
        float angle = Random.Range(-90f, 90f); // ����Ƕȷ�Χ����Բ
        Vector2 randomDirection = Quaternion.Euler(0, 0, angle) * directionToEnemy;

        // ������ɰ뾶ֵ
        float randomRadius = Random.Range(innerRadius, outerRadius);

        //���������
        circleRoundPosition = _playerPosition + randomDirection * randomRadius;
    }

    public void MoveToCircleRoundPosition()
    {
        Vector2 _moveDirection = (Vector2)circleRoundPosition - (Vector2)transform.position;
        rb.velocity = _moveDirection.normalized * circleRoundSpeed;
    }

    
    //��ȡ·����
    private void GeneratePath(Vector3 _target)
    {
        currentPathIndex = 0;
        //��������㣬�յ㣬�ص�����
        seeker.StartPath(transform.position, _target, Path => 
        {
            pathPointList = Path.vectorPath;
        });
    }

    //�Զ�Ѱ·
    private void AutoPath()
    {
        //·����ʱ����
        pathGenerateTimer += Time.deltaTime;
        if(pathGenerateTimer >= pathGenerateInterval)
        {
            pathGenerateTimer = 0;
            GeneratePath(currentTarget);
        }

        //·���б�Ϊ��ʱ����ʼ�Զ�Ѱ·
        if(pathPointList == null || pathPointList.Count == 0)
        {
            GeneratePath(currentTarget);
        }
        //����·����ʱ������������������һ��·����
        else if(Vector2.Distance(transform.position, pathPointList[currentPathIndex]) < 0.1f)
        {
            currentPathIndex++;
            if(currentPathIndex >= pathPointList.Count)
            {
                GeneratePath(currentTarget);
            }
        }
    }


    //��ʼ��Ѳ�ߵ�
    public void InitializePatrolPoint()
    {
        if (patrolPointTransform == null)
        {
            Debug.Log("Ѳ�ߵ�Ϊ�գ�");
            return;
        }
        currentPatrolIndex = 0;
        patrolPointList.Clear();
        foreach (Transform child in patrolPointTransform)
        {
            patrolPointList.Add(child.position);
        }
    }


    //����Ѳ�ߵ������л���һ��Ѳ�ߵ㡢
    public void changeRandomPatrolPoint()
    {
        if (patrolPointList == null || patrolPointList.Count == 0)
        {
            return;
        }
        if (distanceToCurrentPatrolPoint < 1f)
            currentPatrolIndex = Random.Range(0, patrolPointList.Count);
    }


    //׷������������,�����ԣ���������
    public bool CanChasePlayer()
    {
        bool _distanceCheck;
        bool _angleCheck;
        bool _raycastCheck;

        // ������˺���ҵľ���
        if (distanceToPlayer < chaseDeterminRange)
        {
            _distanceCheck = true;
        }
        else
            _distanceCheck = false;

        // ���˷�������ҷ���ļн�
        Vector2 _directionToPlayer = (playerGO.transform.position - transform.position).normalized;
        float _angleToPlayer = Vector3.Angle(transform.forward, _directionToPlayer);
        if (_angleToPlayer < viewAngle / 2)
        {
            _angleCheck = true;
        }
        else
        {
            _angleCheck = false;
        }


        //raycast���
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _directionToPlayer, viewRadius);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            _raycastCheck = true;
        }
        else
        {
            _raycastCheck = false;
        }

        angleCheck = _angleCheck;
        distanceCheck = _distanceCheck;
        hitCheck = _raycastCheck;

        // �����ж�
        if (_distanceCheck && _angleCheck && _raycastCheck)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

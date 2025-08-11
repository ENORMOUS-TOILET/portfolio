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

    //该敌人和玩家的距离
    public float distanceToPlayer { get; private set; }
    //敌人移动方向
    private Vector2 moveDirection;

    [Header("敌人基础属性")]
    //敌人追击属性
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    public float attackDetermineRange;
    [SerializeField] private float sideOffset;
    public SpriteRenderer weaponSR;
    [HideInInspector]public bool isDead;

    [Header("周旋状态变量")]
    //周旋状态变量
    //敌人周旋点与玩家的距离
    [SerializeField] private float innerRadius;
    [SerializeField] private float outerRadius;
    [SerializeField] private float circleRoundSpeed;
    public float circleRoundTime;
    public Vector2 circleRoundPosition { get; private set; }//周旋目标点

    [Header("攻击准备状态变量")]
    //攻击准备状态变量
    //敌人距离玩家侧面点多近能攻击
    [SerializeField] private float attackRangeTolerance;
    //是否到达玩家侧面，用于退出AttackPrepare状态
    [HideInInspector]public bool reachSide;

    //a星寻路变量
    [Header("A星寻路变量")]
    public float pathGenerateInterval = 0.5f;//路径生成间隔
    private Seeker seeker;
    private List<Vector3> pathPointList;//路径点列表
    private int currentPathIndex;//当前路径点索引
    private float pathGenerateTimer = 0;//路径生成计时器
    [HideInInspector]public Vector2 currentTarget;//当前目标点

    [Header("巡逻变量")]
    public float maxPatrolDistance;
    [HideInInspector]public float distanceToCurrentPatrolPoint;
    [HideInInspector]public List<Vector2> patrolPointList;
    [HideInInspector]public int currentPatrolIndex;
    public Transform patrolPointTransform;
    public float chaseDeterminRange;

    [Header("视野监测属性")]
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
                // 如果匹配，则执行相应的方法
                thisCharacter.currentHP--;
                GetHit();
                break; // 找到匹配的标签后跳出循环
            }
        }
    }

    //受伤
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

    //死亡
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

    //使用颜色代码设置颜色
    void SetColorFromHex(string _hexCode,SpriteRenderer _sr)
    {
        Color color;

        // 将十六进制颜色代码转换为Color类型
        if (ColorUtility.TryParseHtmlString(_hexCode, out color))
        {
            _sr.color = color;
        }
        else
        {
            Debug.LogError("无效的十六进制颜色代码");
        }
    }

    // 设置透明度（Alpha值）
    void SetOpacity(float _alpha,SpriteRenderer _sr)
    {
        // 获取当前的颜色
        Color currentColor = sr.color;

        // 创建一个新的颜色，保持原来的 RGB 值，只修改 Alpha
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, _alpha);

        // 设置新的颜色
        _sr.color = newColor;
    }

    //计算和玩家的距离
    private void CalculateDistanceToPlayer()
    {
        Vector2 _thisToPlayer = playerGO.transform.position - transform.position;
        distanceToPlayer = _thisToPlayer.magnitude;
    }


    //计算和巡逻点的距离
    public void CalculateDistanceToPatrolPoint()
    {
        if (patrolPointList == null || patrolPointList.Count == 0)
        {
            return;
        }
        distanceToCurrentPatrolPoint = Vector2.Distance(transform.position, patrolPointList[currentPatrolIndex]);
    }

    //面向玩家
    public void FacePlayer()
    {
        // 获取玩家在屏幕上的位置
        Vector3 playerPositon = playerGO.transform.position;
        // 获取物体的当前位置
        Vector3 direction = playerPositon - transform.position;
        // 计算目标旋转角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // 设置物体的旋转
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
    }

    //面向当前路径点
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

    //追击目标
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

    //通过路径点移动
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
    

    //绕到玩家侧面攻击，未启用
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

    //重置或初始化周旋点位置
    public void SetCircleRoundPoint()
    {
        Vector2 _playerPosition = playerGO.transform.position;
        Vector2 _enemyPosition = enemyGO.transform.position;

        // 计算敌人方向
        Vector2 directionToEnemy = (_enemyPosition - _playerPosition).normalized;

        // 随机选择一个角度，仅限靠近敌人的半圆
        float angle = Random.Range(-90f, 90f); // 随机角度范围：半圆
        Vector2 randomDirection = Quaternion.Euler(0, 0, angle) * directionToEnemy;

        // 随机生成半径值
        float randomRadius = Random.Range(innerRadius, outerRadius);

        //生成随机点
        circleRoundPosition = _playerPosition + randomDirection * randomRadius;
    }

    public void MoveToCircleRoundPosition()
    {
        Vector2 _moveDirection = (Vector2)circleRoundPosition - (Vector2)transform.position;
        rb.velocity = _moveDirection.normalized * circleRoundSpeed;
    }

    
    //获取路径点
    private void GeneratePath(Vector3 _target)
    {
        currentPathIndex = 0;
        //参数：起点，终点，回调函数
        seeker.StartPath(transform.position, _target, Path => 
        {
            pathPointList = Path.vectorPath;
        });
    }

    //自动寻路
    private void AutoPath()
    {
        //路径定时生成
        pathGenerateTimer += Time.deltaTime;
        if(pathGenerateTimer >= pathGenerateInterval)
        {
            pathGenerateTimer = 0;
            GeneratePath(currentTarget);
        }

        //路径列表不为空时，开始自动寻路
        if(pathPointList == null || pathPointList.Count == 0)
        {
            GeneratePath(currentTarget);
        }
        //到达路径点时，递增索引，生成下一个路径点
        else if(Vector2.Distance(transform.position, pathPointList[currentPathIndex]) < 0.1f)
        {
            currentPathIndex++;
            if(currentPathIndex >= pathPointList.Count)
            {
                GeneratePath(currentTarget);
            }
        }
    }


    //初始化巡逻点
    public void InitializePatrolPoint()
    {
        if (patrolPointTransform == null)
        {
            Debug.Log("巡逻点为空！");
            return;
        }
        currentPatrolIndex = 0;
        patrolPointList.Clear();
        foreach (Transform child in patrolPointTransform)
        {
            patrolPointList.Add(child.position);
        }
    }


    //到达巡逻点后，随机切换下一个巡逻点、
    public void changeRandomPatrolPoint()
    {
        if (patrolPointList == null || patrolPointList.Count == 0)
        {
            return;
        }
        if (distanceToCurrentPatrolPoint < 1f)
            currentPatrolIndex = Random.Range(0, patrolPointList.Count);
    }


    //追击玩家条件检测,做不对！！！！！
    public bool CanChasePlayer()
    {
        bool _distanceCheck;
        bool _angleCheck;
        bool _raycastCheck;

        // 距离敌人和玩家的距离
        if (distanceToPlayer < chaseDeterminRange)
        {
            _distanceCheck = true;
        }
        else
            _distanceCheck = false;

        // 敌人方向与玩家方向的夹角
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


        //raycast检测
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

        // 条件判断
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

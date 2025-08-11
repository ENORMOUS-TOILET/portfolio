using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    [HideInInspector]public Rigidbody2D rb;

    [Header("霰弹枪音效控制")]
    //霰弹枪音效
    public AudioSource shotgunShootAS;
    public AudioSource shotgunReloadAS;
    public float reloadWaitTime;
    private float reloadWaitTimer;
    private bool shouldPlayReloadAS;

    [Header("步行音效控制")]
    public FootStepAudioPlayer footStepPlayer;
    public float stepGapTime;

    private Character thisCharacter;

    [HideInInspector]public Vector2 moveDirection;

    [Header("射击属性")]
    public GameObject muzzle;
    public GameObject shotgunBulletGO;
    public float shotCD;// 射击冷却时间
    private float shootTimer;
    public int ammoCount;
    public int ammoCapacity;
    //public ShotgunBullet shotGunBullet;

    [SerializeField] private float moveSpeed;

    [Header("冲刺属性")]
    [SerializeField] private float dashCoolDown;
    public float dashDuration;
    public float dashSpeed;
    private float dashUsageTimer;

    [Header("人物属性")]
    public float currentHP;
    [SerializeField] private float maxHP;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        rb = gameObject.GetComponent<Rigidbody2D>();
        thisCharacter = GetComponent<Character>();
        //shotGunBullet = shotgunBulletGO.GetComponent<ShotgunBullet>();
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
        Initialize();
    }

    private void Update()
    {
        stateMachine.currentState.Update();

        dashUsageTimer -= Time.deltaTime;

        shootTimer -= Time.deltaTime;

        reloadWaitTimer -= Time.deltaTime;
    }

    private void Initialize()
    {
        currentHP = maxHP;
        shouldPlayReloadAS = false;
        ammoCount = ammoCapacity;
        shootTimer = 0;
        dashUsageTimer = 0;
    }

    public void SetVelocity(Vector2 _moveDirection)
    {
        moveDirection = new Vector2(stateMachine.currentState.xInput, stateMachine.currentState.yInput).normalized;
        rb.velocity = _moveDirection * moveSpeed;
    }

    public void FaceMouse()
    {
        // 获取鼠标在屏幕上的位置
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        // 获取物体的当前位置
        Vector3 direction = mousePosition - transform.position;
        // 计算目标旋转角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // 设置物体的旋转
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
    }

    public void Shoot(GameObject _bullet)
    {
        ShotgunBullet _shotgunBullet = _bullet.GetComponent<ShotgunBullet>();

        // 获取鼠标在屏幕上的位置
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        // 获取物体的当前位置
        Vector3 direction = mousePosition - muzzle.transform.position;
        // 计算目标旋转角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (UnityEngine.Input.GetKey(KeyCode.Mouse0) && shootTimer < 0 && ammoCount > 0)
        { 
            shootTimer = shotCD;
            ammoCount--;
            for(int i = 0; i<_shotgunBullet.amount; i++)
            {
                float _bulletDiffusion = Random.Range(-_shotgunBullet.bulletDiffusion, _shotgunBullet.bulletDiffusion);
                Instantiate(_bullet, muzzle.transform.position, Quaternion.Euler(new Vector3(0, 0, angle - 90f + _bulletDiffusion)));
            }
            shotgunShootAS.Play();
            reloadWaitTimer = reloadWaitTime;
            shouldPlayReloadAS = true;
            //Debug.Log("射击一次");
        }

        ShotgunReloadAudioControl();
    }


    //霰弹枪装弹音效控制
    public void ShotgunReloadAudioControl()
    {
        if (shouldPlayReloadAS && reloadWaitTimer < 0)
        {
            shotgunReloadAS.Play();
            shouldPlayReloadAS = false;
        }
    }

    public void CheckDashInput()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && dashUsageTimer <0)
        {
            dashUsageTimer = dashCoolDown;
            stateMachine.ChangeState(dashState);
        }
    }


    //受击判断
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


    //受击后执行的动作
    private void GetHit()
    {
        Debug.Log("玩家受到伤害");
        if (thisCharacter.currentHP < 0)
        {
            Death();
            return;
        }
    }


    //死亡方法
    private void Death()
    {
        return;
    }


    //播放步行音效
    private void PlayOneFootStepAudio()
    {
        if (footStepPlayer != null)
        {
            footStepPlayer.PlayNextFootstep();
        }
    }
    public void StartWalkAudio()
    {
        InvokeRepeating("PlayOneFootStepAudio", stepGapTime/3,stepGapTime);
    }
    public void StopWalkAudio()
    {
        CancelInvoke("PlayOneFootStepAudio");
    }
}

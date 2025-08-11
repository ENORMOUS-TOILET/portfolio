using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("×Óµ¯ÊôÐÔ")]
    public float speed;
    private float actualSpeed;
    public float speedChangeRate;
    //private float _speedChangeRateValue = 0.5f;
    //public float speedChangeRate
    //{
    //    get { return _speedChangeRateValue; }
    //    set { _speedChangeRateValue = Mathf.Clamp(speedChangeRate, 0, 1); }
    //}
    public int amount;
    public float lifeTime;
    public float bulletDiffusion;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
        actualSpeed = speed * Random.Range(1-speedChangeRate, 1+speedChangeRate);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.up * actualSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") || collision.CompareTag("Obstacle"))
            Destroy(gameObject);
    }
}

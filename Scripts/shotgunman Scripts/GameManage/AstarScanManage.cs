using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarScanManage : MonoBehaviour
{
    private AstarPath pathfinder;
    public float scanCooldown;
    private float scanCooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = GameObject.FindWithTag("AstarPath").GetComponent<AstarPath>();
    }

    // Update is called once per frame
    void Update()
    {
        scanCooldownTimer -= Time.deltaTime;
        if (scanCooldownTimer <= 0)
        {
            pathfinder.Scan();
            scanCooldownTimer = scanCooldown;
        }
    }
}

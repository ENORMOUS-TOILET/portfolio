using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    public float destroyTime;

    private ParticleSystem ps;

    [System.Obsolete]
    private void Start()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
        destroyTime = ps.startLifetime;
        Destroy(gameObject, destroyTime + 0.05f);
    }
}

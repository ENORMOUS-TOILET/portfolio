using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float currentHP;
    public float maxHP;
    public string[] harmTags;

    private void Start()
    {
        currentHP = maxHP;
    }

}

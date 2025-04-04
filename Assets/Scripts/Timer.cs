using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timer;
    private void Awake()
    {
        timer = 0f;
    }

    private void Update()
    {
       timer += Time.deltaTime;
    }

    public float GetGlobalTimer()
    {
        return timer;
    }
}
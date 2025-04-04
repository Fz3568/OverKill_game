using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public float[] XLimit = new float[2];
    public float[] YLimit = new float[2];
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (XLimit[0] < Player.transform.position.x && Player.transform.position.x < XLimit[1])
        {
            transform.position = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);    
        }

        if (YLimit[0] < Player.transform.position.y && Player.transform.position.y < YLimit[1])
        {
            transform.position = new Vector3(transform.position.x, Player.transform.position.y, transform.position.z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5;
    public Rigidbody2D player;
    private Vector2 moveInput;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        //Normalize the moveInput to have magnitude of 1
        moveInput.Normalize();

        player.linearVelocity = moveInput * moveSpeed;


    }

}

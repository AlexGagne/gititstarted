﻿using UnityEngine;
using System.Collections.Generic;

public class Player : GitCharacterController
{
    // Use this for initialization
    void Start()
    {
        initialize();
        name = "Player";
        speed = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse button 0 is left click
        if (Input.GetMouseButton(0))
        {
            var newTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newTarget.z = transform.position.z;
            addTarget(newTarget);
        }

        if (PlayerFlags.isPlayerBeingFollowed)
        {
            speed = 5.0f;
        }
        else
        {
            speed = 20.0f;
        }            

        MoveToPosition();

        PlayerFlags.playerPosition = transform.position;
    }
}

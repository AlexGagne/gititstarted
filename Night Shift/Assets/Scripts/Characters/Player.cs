using UnityEngine;
using System.Collections.Generic;
using System;

public class Player : GitCharacterController
{
    Patient currentPatient;

    // Use this for initialization
    void Start()
    {
        initialize();
        name = "Player";
        speed = 12.0f;
        currentPatient = null;
    }

    // Update is called once per frame
    void Update()
    {
        try {
            // Mouse button 0 is left click
            if (Input.GetMouseButton(0) && !PlayerFlags.disableLeftClick)
            {
                var newTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newTarget.z = transform.position.z;
                if (newTarget.x < 20.0f && newTarget.x > -20.0f && newTarget.y < 20.0f && newTarget.y > -20.0f)
                {
                    target = newTarget;
                }
            }
            /*
            if (PlayerFlags.isPlayerBeingFollowed)
            {
                speed = 6.0f;
            }
            else
            {
                speed = 12.0f;
            }
            */
            PlayerFlags.playerPosition = transform.position;

            hackMoveToPosition();
        }
        catch(Exception e)
        {
            //Because we hate exceptions
            print("Exception " + e.Message);
        }
    }

    public void StartCarrying(Patient patient)
    {
        speed = 6.0f;
        currentPatient = patient;
    }

    public void StopCarrying(Patient patient)
    {
        speed = 12.0f;
        currentPatient = null;
    }

    private void hackMoveToPosition()
    {
        var tempPath = pathFinding.FindPath(transform.position, target);
        if (tempPath.Count != 0)
        {
            path = tempPath;
            nextPosition = 0;
        }

        // If a path is available to move
        if (path.Count != 0)
        {
            animationController.SetBool("Moving", true);
            // If we haven't reached our goal yet
            if (nextPosition < path.Count)
            {
                var nextPos = path[nextPosition].worldPosition_;
                nextPos.z = transform.position.z;
                // If we haven't reached the position of this node
                if (transform.position != nextPos)
                {
                    RotateToPoint(transform.position, nextPos);
                    var oldPos = transform.position;
                    transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
                    if (name == "Player")
                    {
                        print("Moved: " + Vector3.Magnitude(oldPos - transform.position));
                    }

                }
                else
                {
                    // Otherwise, we seek the next position
                    nextPosition++;
                    if (nextPosition < path.Count)
                    {
                        RotateToPoint(transform.position, nextPos);
                        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
                    }
                }
            }
            else
            {
                // We reached our goal, we can reset the path
                path = new List<Node>();
                nextPosition = 0;
                animationController.SetBool("Moving", false);
            }
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public abstract class GitCharacterController : MonoBehaviour {

    public float speed = 15.0f;
    public float turnSpeed = 10.0f;
    protected Vector3 target;
    protected PathFinding pathFinding;
    protected List<Node> path = new List<Node>();
    protected int nextPosition = 0;
    protected bool targetChanged = false;

    protected Animator animationController;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

    }

    protected void initialize()
    {
        target = transform.position;
        pathFinding = GetComponent<PathFinding>();
        animationController = GetComponent<Animator>();
        animationController.SetBool("Moving", false);
    }

    private void RotateToPoint(Vector3 origin, Vector3 destination)
    {
        var direction = destination - origin;

        if (direction != Vector3.zero)
        {
            direction.Normalize();
            float correction = -90.0f;
            float angle = correction + (Mathf.Atan2(direction.y, direction.x)) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), turnSpeed * Time.deltaTime);
        }
    }

    protected void MoveToPosition(Vector3 destination)
    {
        if (targetChanged)
        {
            path = pathFinding.FindPath(transform.position, destination);
            targetChanged = false;
        }


        // If a path is available to move
        if (path.Count != 0)
        {
            animationController.SetBool("Moving", true);
            // If we haven't reached our goal yet
            if (nextPosition < path.Count)
            {
                // If we haven't reached the position of this node
                if (transform.position != path[nextPosition].worldPosition_)
                {
                    var nextPos = path[nextPosition].worldPosition_;
                    nextPos.z = transform.position.z;
                    RotateToPoint(transform.position, nextPos);
                    transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);


                }
                else
                {
                    // Otherwise, we seek the next position
                    nextPosition++;
                    if (nextPosition < path.Count)
                    {
                        var nextPos = path[nextPosition].worldPosition_;
                        nextPos.z = transform.position.z;
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

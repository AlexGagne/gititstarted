using UnityEngine;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour {

    public float speed = 15.0f;
    private Vector3 target;
    private PathFinding pathFinding;
    private List<Node> path = new List<Node>();
    private int nextPosition = 0;

    // Use this for initialization
    void Start () {
        target = transform.position;
        pathFinding = GetComponent<PathFinding>();
    }
	
	// Update is called once per frame
	void Update () {
        // Mouse button 0 is left click
        if(Input.GetMouseButton(0)) {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;

            path = pathFinding.FindPath(transform.position, target);
        }

        // If a path is available to move
        if (path.Count != 0)
        {
            // If we haven't reached our goal yet
            if (nextPosition < path.Count)
            {
                // If we haven't reached the position of this node
                if (transform.position != path[nextPosition].worldPosition_)
                {
                    var nextPos = path[nextPosition].worldPosition_;
                    transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
                }
                else
                {
                    // Otherwise, we seek the next position
                    nextPosition++;
                    if (nextPosition < path.Count)
                    {
                        var nextPos = path[nextPosition].worldPosition_;
                        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
                    }
                }
            }
            else
            {
                // We reached our goal, we can reset the path
                path = new List<Node>();
                nextPosition = 0;
            }
        }
    }
}

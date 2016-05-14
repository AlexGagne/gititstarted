using UnityEngine;
using System.Collections.Generic;

public class Player : GitCharacterController
{
    // Use this for initialization
    void Start()
    {
        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse button 0 is left click
        if (Input.GetMouseButton(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
            targetChanged = true;
        }

        MoveToPosition(target);
    }
}

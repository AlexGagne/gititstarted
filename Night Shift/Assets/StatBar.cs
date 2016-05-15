using UnityEngine;
using System.Collections;

public class StatBar : MonoBehaviour {

    public float radius;
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = gameObject.transform.parent.GetChild(0).position + new Vector3(0, radius);
    }
}

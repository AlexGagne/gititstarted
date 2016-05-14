using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Patient : GitCharacterController
{

    public PatientState State;
    public PatientWounds Wound;
    
    // Use this for initialization
    void Start () {
        initialize();
        var spriteRenderer = GetComponent<SpriteRenderer>();

        var sprite = new Sprite();

        // Change sprite according to illness
        switch (Wound)
        {
            case PatientWounds.Healthy:
                break;
            case PatientWounds.Dead:
                break;
            case PatientWounds.Hemorhagie:
                break;
            case PatientWounds.Psychology:
                break;
            case PatientWounds.Vomitorium:
                break;
            case PatientWounds.Surgery:
                break;
            case PatientWounds.Exorcism:
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Patient : MonoBehaviour {

    public PatientState State;
    public PatientWounds Wound;

    public Patient(PatientState state, PatientWounds wound) {
        State = state;
        Wound = wound;
    }

    // Use this for initialization
    void Start () {
        var spriteRenderer = GetComponent<SpriteRenderer>();
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

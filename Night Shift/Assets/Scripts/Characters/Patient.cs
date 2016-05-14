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

        // Change sprite according to illness
        switch (Wound)
        {
            case PatientWounds.Healthy:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/patient");
                break;
            case PatientWounds.Dead:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/patient");
                break;
            case PatientWounds.Hemorhagie:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/patient");
                break;
            case PatientWounds.Psychology:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/patient");
                break;
            case PatientWounds.Vomitorium:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/patient");
                break;
            case PatientWounds.Surgery:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/patient");
                break;
            case PatientWounds.Exorcism:
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/patient");
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        switch (State)
        {
            case PatientState.DoneTreatment:
                break;
            case PatientState.GettingTreated:
                break;
            case PatientState.WaitingForTreatment:
                GoToAvailableChair();
                break;
        }
	}

    private void GoToAvailableChair()
    {

    }
}

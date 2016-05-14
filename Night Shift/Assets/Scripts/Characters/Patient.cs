using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;

public class Patient : GitCharacterController
{

    public PatientState State;
    public PatientWounds Wound;

    private static List<bool> isChairOccupied = new List<bool>();
    private static List<Vector3> chairPositions = new List<Vector3>();
    private static List<Vector3> panicModeWaypoints = new List<Vector3>();
    private static bool listsInitialized = false;

    private bool isSeated = false;
    private int id;
    private static int nextId = 0;

    private bool panicMode = false;
    private int panicModeIndex = 0;

    // Use this for initialization
    void Start () {
        initialize();

        id = nextId;
        nextId++;

        // Change sprite according to illness
        switch (Wound)
        {
            case PatientWounds.Healthy:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", false);
                animationController.SetBool("Sick", false);
                animationController.SetBool("Healthy", true);
                animationController.SetBool("Dead", false);
                break;
            case PatientWounds.Dead:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", false);
                animationController.SetBool("Sick", false);
                animationController.SetBool("Healthy", false);
                animationController.SetBool("Dead", true);
                break;
            case PatientWounds.Hemorhagie:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", false);
                animationController.SetBool("Sick", false);
                animationController.SetBool("Healthy", false);
                animationController.SetBool("Dead", true);
                break;
            case PatientWounds.Psychology:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", false);
                animationController.SetBool("Sick", false);
                animationController.SetBool("Healthy", false);
                animationController.SetBool("Dead", false);
                break;
            case PatientWounds.Vomitorium:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", false);
                animationController.SetBool("Sick", true);
                animationController.SetBool("Healthy", false);
                animationController.SetBool("Dead", false);
                break;
            case PatientWounds.Surgery:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", true);
                animationController.SetBool("Sick", false);
                animationController.SetBool("Healthy", false);
                animationController.SetBool("Dead", false);
                break;
            case PatientWounds.Exorcism:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", false);
                animationController.SetBool("Sick", false);
                animationController.SetBool("Healthy", false);
                animationController.SetBool("Dead", false);
                break;
        }

        if (!listsInitialized)
        {
            for (int i = 0; i < 12; i++)
            {
                isChairOccupied.Add(false);
            }

            // Add every position of the chairs
            chairPositions.Add(new Vector3(-6.0f, 1.0f, -1.0f));
            chairPositions.Add(new Vector3(-4.0f, 1.0f, -1.0f));
            chairPositions.Add(new Vector3(-1.0f, 1.0f, -1.0f));
            chairPositions.Add(new Vector3(1.0f, 1.0f, -1.0f));
            chairPositions.Add(new Vector3(4.0f, 1.0f, -1.0f));
            chairPositions.Add(new Vector3(6.0f, 1.0f, -1.0f));
            chairPositions.Add(new Vector3(-6.0f, -3.0f, -1.0f));
            chairPositions.Add(new Vector3(-4.0f, -3.0f, -1.0f));
            chairPositions.Add(new Vector3(-1.0f, -3.0f, -1.0f));
            chairPositions.Add(new Vector3(1.0f, -3.0f, -1.0f));
            chairPositions.Add(new Vector3(4.0f, -3.0f, -1.0f));
            chairPositions.Add(new Vector3(6.0f, -3.0f, -1.0f));

            panicModeWaypoints.Add(new Vector3(-18.0f, 3.0f, -1.0f));
            panicModeWaypoints.Add(new Vector3(-18.0f, -3.0f, -1.0f));
            panicModeWaypoints.Add(new Vector3(-13.0f, -3.0f, -1.0f));
            panicModeWaypoints.Add(new Vector3(-13.0f, 3.0f, -1.0f));

            listsInitialized = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (id != 0)
        {
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
	}

    private void GoToAvailableChair()
    {
        if (!isSeated)
        {
            panicMode = true;
            for (int i = 0; i < 12; i++)
            {
                if (!isChairOccupied[i])
                {
                    clearTargets();
                    var newTarget = chairPositions[i];
                    newTarget.z = transform.position.z;

                    addTarget(newTarget);
                    
                    isChairOccupied[i] = true;
                    isSeated = true;
                    panicMode = false;
                    speed = 10;
                    break;
                }
            }
        }

        if (panicMode)
        {
            speed = 15;
            addTarget(panicModeWaypoints[panicModeIndex]);
            if(panicModeIndex >= panicModeWaypoints.Count - 1)
            {
                panicModeIndex = 0;
            }
            else
            {
                panicModeIndex++;
            }
        }
        MoveToPosition();
    }
}

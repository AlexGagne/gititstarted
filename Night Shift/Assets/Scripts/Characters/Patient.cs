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
    private bool goingToSeat = false;
    private int id;
    private static int nextId = 0;

    private bool panicMode = false;
    private int panicModeIndex = 0;

    private bool transportedByPlayer = false;
    private bool firstInitTransportPlayer = true;

    private int occupiedChairIndex;

    // Use this for initialization
    void Start () {
        initialize();
        name = "Patient";

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
                animationController.SetBool("Crazy", false);
                animationController.SetBool("Dead", false);
                break;
            case PatientWounds.Dead:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", false);
                animationController.SetBool("Sick", false);
                animationController.SetBool("Healthy", false);
                animationController.SetBool("Crazy", false);
                animationController.SetBool("Dead", true);
                break;
            case PatientWounds.Hemorhagie:
                animationController.SetBool("Bleeding", true);
                animationController.SetBool("Knife", false);
                animationController.SetBool("Sick", false);
                animationController.SetBool("Crazy", false);
                animationController.SetBool("Healthy", false);
                animationController.SetBool("Dead", false);
                break;
            case PatientWounds.Psychology:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", false);
                animationController.SetBool("Sick", false);
                animationController.SetBool("Crazy", true);
                animationController.SetBool("Healthy", false);
                animationController.SetBool("Dead", false);
                break;
            case PatientWounds.Vomitorium:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", false);
                animationController.SetBool("Sick", true);
                animationController.SetBool("Crazy", false);
                animationController.SetBool("Healthy", false);
                animationController.SetBool("Dead", false);
                break;
            case PatientWounds.Surgery:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", true);
                animationController.SetBool("Sick", false);
                animationController.SetBool("Crazy", false);
                animationController.SetBool("Healthy", false);
                animationController.SetBool("Dead", false);
                break;
            case PatientWounds.Exorcism:
                animationController.SetBool("Bleeding", false);
                animationController.SetBool("Knife", false);
                animationController.SetBool("Sick", false);
                animationController.SetBool("Crazy", false);
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
        if (id > 4)
        {
            if (!transportedByPlayer)
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
            else
            {
                if (firstInitTransportPlayer)
                {
                    clearTargets();
                    firstInitTransportPlayer = false;
                }
                if (Wound == PatientWounds.Dead)
                {
                    transform.position = PlayerFlags.playerPosition;
                }
                else
                {
                    addTarget(PlayerFlags.playerPosition);
                    MoveToPosition();
                }
            }
        }
        if(Wound == PatientWounds.Exorcism)
        {
            transform.Rotate(new Vector3(0, 0, 2*turnSpeed));
        }
    }

    private void GoToAvailableChair()
    {
        if (!goingToSeat)
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
                    occupiedChairIndex = i;
                    goingToSeat = true;
                    panicMode = false;
                    speed = 10;
                    break;
                }
            }
        }

        if (reachedDestination)
        {
            isSeated = true;
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

    protected override void collidedWith(Collision2D coll)
    {
        if (panicMode)
        {
            return;
        }

        var nameOfCollided = coll.gameObject.name;
       
        if(nameOfCollided == "Player")
        {
            if (isSeated)
            {
                if (!PlayerFlags.isPlayerBeingFollowed)
                {
                    PlayerFlags.isPlayerBeingFollowed = true;
                    transportedByPlayer = true;
                    isSeated = false;
                    isChairOccupied[occupiedChairIndex] = false;
                    speed = 15.0f;
                }
            }
        }
        else if(transportedByPlayer)
        {
            bool deposited = false;
            switch (nameOfCollided)
            {
                case "Hemorhagie":
                    if(Wound != PatientWounds.Hemorhagie)
                    {
                        return;
                    }

                    if (TreatmentFlags.isHemorhagieOccupied)
                    {
                        return;
                    }

                    TreatmentFlags.isHemorhagieOccupied = true;
                    deposited = true;
                    break;
                case "Psychology":
                    if (Wound == PatientWounds.Psychology)
                    {
                        return;
                    }

                    if (TreatmentFlags.isPsychologyOccupied)
                    {
                        return;
                    }

                    TreatmentFlags.isPsychologyOccupied = true;
                    deposited = true;
                    break;
                case "Vomitorium":
                    if (Wound == PatientWounds.Vomitorium)
                    {
                        return;
                    }

                    if (TreatmentFlags.isVomitoriumOccupied)
                    {
                        return;
                    }

                    TreatmentFlags.isVomitoriumOccupied = true;
                    deposited = true;
                    break;
                case "Surgery":
                    if (Wound == PatientWounds.Surgery)
                    {
                        return;
                    }

                    if (TreatmentFlags.isSurgeryOccupied)
                    {
                        return;
                    }

                    TreatmentFlags.isSurgeryOccupied = true;
                    deposited = true;
                    break;
                case "Exorcism":
                    if (Wound == PatientWounds.Exorcism)
                    {
                        return;
                    }

                    if (TreatmentFlags.isExorcismOccupied)
                    {
                        return;
                    }

                    TreatmentFlags.isExorcismOccupied = true;
                    deposited = true;
                    break;
                case "Morgue":
                    if (Wound == PatientWounds.Dead)
                    {
                        return;
                    }

                    transportedByPlayer = false;
                    PlayerFlags.isPlayerBeingFollowed = false;
                    Destroy(this.gameObject);
                    break;
            }
            if (deposited)
            {
                State = PatientState.GettingTreated;
                transportedByPlayer = false;
                PlayerFlags.isPlayerBeingFollowed = false;
            }
        }
    }
}

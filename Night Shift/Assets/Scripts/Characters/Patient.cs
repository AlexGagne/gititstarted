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


    /*public GameObject tileSelectionMarker;
    private GameObject selectorSprite;*/

    private bool isSeated = false;
    private bool goingToSeat = false;
    private int id;
    private static int nextId = 0;

    private bool panicMode = false;
    private int panicModeIndex = 0;

    private bool transportedByPlayer = false;
    private bool firstInitTransportPatient = true;

    private int occupiedChairIndex;

    private bool firstInitHealthyPatient = true;

    private int healthyCounter = 600;

    private bool firstInitCollisionWithTreatment = true;

    // Use this for initialization
    void Start () {
        initialize();
        name = "Patient";

        id = nextId;
        nextId++;

        Physics.queriesHitTriggers = true;

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

        //selectorSprite = Instantiate((Object) tileSelectionMarker, new Vector3(0, 0, 0), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
        if (id <= 4)
        {
            return;
        }
        if(Wound == PatientWounds.Healthy)
        {
            if (firstInitHealthyPatient)
            {
                clearTargets();
                firstInitHealthyPatient = false;
            }

            addTarget(TreatmentFlags.EntrancePosition);
            MoveToPosition();
        }
        if (!transportedByPlayer)
        {
            switch (State)
            {
                case PatientState.GettingTreated:
                    // Wait for a bit of time
                    healthyCounter--;
                    if(healthyCounter <= 0)
                    {
                        BecomeHealthy();
                    }
                    MoveToPosition();
                    break;
                case PatientState.WaitingForTreatment:
                    GoToAvailableChair();
                    break;
            }
        }
        else
        {
            if (firstInitTransportPatient)
            {
                clearTargets();
                firstInitTransportPatient = false;
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
            if(PlayerFlags.IdOfLastClickedPatient == id)
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
        }
        else if(nameOfCollided == "Entrance")
        {
            if (Wound == PatientWounds.Healthy)
            {
                Destroy(this.gameObject);
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
                    if (firstInitCollisionWithTreatment)
                    {
                        clearTargets();
                        firstInitCollisionWithTreatment = false;
                    }
                    addTarget(TreatmentFlags.HemorhagiePosition);
                    break;
                case "Psychology":
                    if (Wound != PatientWounds.Psychology)
                    {
                        return;
                    }

                    if (TreatmentFlags.isPsychologyOccupied)
                    {
                        return;
                    }

                    TreatmentFlags.isPsychologyOccupied = true;
                    deposited = true;
                    if (firstInitCollisionWithTreatment)
                    {
                        clearTargets();
                        firstInitCollisionWithTreatment = false;
                    }
                    addTarget(TreatmentFlags.PsychologyPosition);
                    break;
                case "Vomitorium":
                    if (Wound != PatientWounds.Vomitorium)
                    {
                        return;
                    }

                    if (TreatmentFlags.isVomitoriumOccupied)
                    {
                        return;
                    }

                    TreatmentFlags.isVomitoriumOccupied = true;
                    deposited = true;
                    if (firstInitCollisionWithTreatment)
                    {
                        clearTargets();
                        firstInitCollisionWithTreatment = false;
                    }
                    addTarget(TreatmentFlags.VomitoriumPosition);
                    break;
                case "Surgery":
                    if (Wound != PatientWounds.Surgery)
                    {
                        return;
                    }

                    if (TreatmentFlags.isSurgeryOccupied)
                    {
                        return;
                    }

                    TreatmentFlags.isSurgeryOccupied = true;
                    deposited = true;
                    if (firstInitCollisionWithTreatment)
                    {
                        clearTargets();
                        firstInitCollisionWithTreatment = false;
                    }
                    addTarget(TreatmentFlags.SurgeryPosition);
                    break;
                case "Exorcism":
                    if (Wound != PatientWounds.Exorcism)
                    {
                        return;
                    }

                    if (TreatmentFlags.isExorcismOccupied)
                    {
                        return;
                    }

                    TreatmentFlags.isExorcismOccupied = true;
                    deposited = true;
                    if (firstInitCollisionWithTreatment)
                    {
                        clearTargets();
                        firstInitCollisionWithTreatment = false;
                    }
                    addTarget(TreatmentFlags.ExorcismPosition);
                    break;
                case "Morgue":
                    if (Wound != PatientWounds.Dead)
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

    private void BecomeHealthy()
    {
        var oldWound = Wound;
        Wound = PatientWounds.Healthy;

        animationController.SetBool("Bleeding", false);
        animationController.SetBool("Knife", false);
        animationController.SetBool("Sick", false);
        animationController.SetBool("Healthy", true);
        animationController.SetBool("Crazy", false);
        animationController.SetBool("Dead", false);
        // The next bool resets the animation
        animationController.SetBool("Moving", false);

        switch (oldWound)
        {
            case PatientWounds.Hemorhagie:
                TreatmentFlags.isHemorhagieOccupied = false;
                break;
            case PatientWounds.Psychology:
                TreatmentFlags.isPsychologyOccupied = false;
                break;
            case PatientWounds.Vomitorium:
                TreatmentFlags.isVomitoriumOccupied = false;
                break;
            case PatientWounds.Surgery:
                TreatmentFlags.isSurgeryOccupied = false;
                break;
            case PatientWounds.Exorcism:
                TreatmentFlags.isExorcismOccupied = false;
                break;
        }
    }
    
    void OnLeftClick(object frontMostRayCast)
    {
        PlayerFlags.IdOfLastClickedPatient = id;
    }

    void OnRightClick(object frontMostRayCast)
    {
        PlayerFlags.IdOfLastClickedPatient = id;
    }
}

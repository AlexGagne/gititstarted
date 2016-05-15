﻿using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using System;

public class Patient : GitCharacterController, IEquatable<Patient>
{
    static Player player;
    public PatientState State;
    public PatientWounds Wound;

    public int HP = 100;
    public int Calm = 100;

    //Time the patient has existed
    public int timeExisted = 0;

    public bool panicMode = false;

    public int healthyCounter = 500;

    public GameManager gameManager;

    private static List<bool> isChairOccupied = new List<bool>();
    private static List<Vector3> chairPositions = new List<Vector3>();
    private static List<Vector3> panicModeWaypoints = new List<Vector3>();
    private static bool listsInitialized = false;

    private bool isSeated = false;
    public bool IsSeated { get { return isSeated; } }

    private bool goingToSeat = false;
    private int id;
    public int ID { get { return id; } }
    private static int nextId = 0;
    
    private int panicModeIndex = 0;

    private bool transportedByPlayer = false;
    private bool firstInitTransportPatient = true;

    private int occupiedChairIndex;

    private bool firstInitHealthyPatient = true;
    
    private bool firstInitCollisionWithTreatment = true;

    // Use this for initialization
    void Start () {
        initialize();
        name = "Patient";

        if (player == null)
            player = GameObject.Find("Player").GetComponent<Player>();

        id = nextId;
        nextId++;

        Physics.queriesHitTriggers = true;

        transform.GetChild(0).position = new Vector3(-1000, -1000, -1000);

        GenerateStats();

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
        try
        {
            //If client is healthy, go to the door
            if (Wound == PatientWounds.Healthy)
            {
                if (firstInitHealthyPatient)
                {
                    clearTargets();
                    firstInitHealthyPatient = false;
                }

                addTarget(TreatmentFlags.EntrancePosition);
                MoveToPosition();
            }

            //Beginning of client not being transported
            if (!transportedByPlayer && Wound != PatientWounds.Dead)
            {
                switch (State)
                {
                    //Client is in treatment
                    case PatientState.GettingTreated:
                        // Wait for a bit of time
                        healthyCounter--;
                        if (healthyCounter == 0)
                        {
                            BecomeHealthy();
                        }
                        MoveToPosition();
                        break;

                    //Client tries to get a chair
                    case PatientState.WaitingForTreatment:
                        GoToAvailableChair();
                        break;
                }
            }

            //Patient is being transported
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

            //Visual update for possessed patients
            if (Wound == PatientWounds.Exorcism)
            {
                transform.Rotate(new Vector3(0, 0, 2 * turnSpeed));
            }
        }
        catch (Exception e)
        {
            //Because we hate exceptions
            print("Exception " + e.Message);
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

        if (reachedDestination && isInAChairPosition())
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
        //Patient cannot find seat
        if (panicMode)
        {
            return;
        }

        var nameOfCollided = coll.gameObject.name;
       
        //Patient collided with the player, and was seated
        if(nameOfCollided == "Player")
        {
            if(PlayerFlags.IdOfLastClickedPatient == id)
            {
                if (isSeated)
                {
                    if (!PlayerFlags.isPlayerBeingFollowed)
                    {
                        PlayerFlags.isPlayerBeingFollowed = true;
                        player.StartCarrying(this);

                        transportedByPlayer = true;
                        isSeated = false;
                        isChairOccupied[occupiedChairIndex] = false;
                        speed = 20.0f;
                    }
                }
            }            
        }
        else if(nameOfCollided == "Entrance")
        {
            if (Wound == PatientWounds.Healthy)
            {
                GameManager.RemovePatient(this);
                Destroy(this.gameObject);
            }

        }

        //***********************************************/
        //Check if the patient can be deposited someplace
        //***********************************************/
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

                    GameManager.RemovePatient(this);
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
        gameManager.PatientCured();
    }
    

    void OnLeftClick(object frontMostRayCast)
    {
        PlayerFlags.IdOfLastClickedPatient = id;
    }

    void OnRightClick(object frontMostRayCast)
    {
        PlayerFlags.IdOfLastClickedPatient = id;
    }

    public bool Equals(Patient other)
    {
        if (Equals(this, null) && Equals(other, null))
            return true;
        else if (Equals(this, null) | Equals(other, null))
            return false;
        else if (id == other.id)
            return true;
        else
            return false;
    }
    public override bool Equals(object obj)
    {
        return Equals(obj as Patient);
    }

    public override int GetHashCode()
    {
        return id;
    }

    private void GenerateStats()
    {
        switch (Wound)
        {
            case PatientWounds.Healthy:
                HP = 100;
                Calm = 100;
                break;
            case PatientWounds.Dead:
                HP = 0;
                Calm = 0;
                break;
            case PatientWounds.Hemorhagie:
                HP = UnityEngine.Random.Range(70,90);
                Calm = UnityEngine.Random.Range(55, 70);
                break;
            case PatientWounds.Psychology:
                HP = UnityEngine.Random.Range(90, 100);
                Calm = UnityEngine.Random.Range(40, 60);
                break;
            case PatientWounds.Vomitorium:
                HP = UnityEngine.Random.Range(70, 80);
                Calm = UnityEngine.Random.Range(55, 70);
                break;
            case PatientWounds.Surgery:
                HP = UnityEngine.Random.Range(35, 50);
                Calm = UnityEngine.Random.Range(40, 55);
                break;
            case PatientWounds.Exorcism:
                HP = UnityEngine.Random.Range(90, 100);
                Calm = UnityEngine.Random.Range(20, 40);
                break;
        }
    }

    public void UpdateStatsPatient(int amountOfCalmLost)
    {
        if(Wound == PatientWounds.Healthy || Wound == PatientWounds.Dead)
        {
            return;
        }
        
        //Patient is only updated at certain intervals
        timeExisted++;
        if (timeExisted % gameManager.secondsForUpdates != 0)
            return;

        Calm = Mathf.Clamp(Calm, 0, 100);

        HP -= 8 - Mathf.FloorToInt(Calm / 15f);
        Calm += amountOfCalmLost;
        Calm = Mathf.Clamp(Calm, 0, 100);

        if (HP <= 0)
        {
            Wound = PatientWounds.Dead;
            animationController.SetBool("Dead", true);
            gameManager.PatientDied();
        }
    }

    public void UpdateBarDisplay()
    {

    }

    private bool isInAChairPosition()
    {
        bool inAChair = false;

        foreach (Vector3 chair in chairPositions)
        {
            if (isCloseEnough(chair))
            {
                inAChair = true;
            }
        }

        return inAChair;
    }

    private bool isCloseEnough(Vector3 chair)
    {
        if (Mathf.Abs(chair.x - transform.position.x) < 0.2f)
            if (Mathf.Abs(chair.y - transform.position.y) < 0.2f)
                return true;
        return false;
    }

}

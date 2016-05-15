using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //Patients list
    static List<Patient> patients;

    //Game Metrics
    int money;
    public int Money { get { return money; } }

    int reputation;
    int deathCount;

    public int curedMoneyReward;
    public int curedRepReward;
    public int deathMoneyPenalty;

    public int hemorhagieCalmLost = -3;
    public int psychologyCalmLost = -5;
    public int surgeryCalmLost = -5;
    public int vomitoriumCalmLost = -2;
    public int exorcismCalmLost = -7;

    public Text moneyText;
    public Text deathText;
    public GameObject repBar;

    //IconLinks
    public GameObject[] items;

    //Time management - 60seconds = 1 round
    int playTime;
    public GameObject minTic;
    public GameObject hourTic;

	// Use this for initialization
	void Start () {

        //DEBUG***********************************/
        money = 12;
        reputation = 33;
        deathCount = 4;
        playTime = 0;
        UpdateMoneyDisplay();
        UpdateDeathDisplay();
        UpdateRepDisplay();
        UpdateItemDisplay();
        //****************************************/
        patients = new List<Patient>();
        InvokeRepeating("SecondPassed", 1f, 1f);
    }

    //Generates reputation bar color
    Color GetColor()
    {
        Color barColor;

        if (reputation > 50)
        {
            barColor = new Color((100 - reputation) / 50f, 1f, 0f);
        }
        else
            barColor = new Color(1f, reputation / 50f, 0f);

        return barColor;
        
    }

    //Changes Money and updates the display
	public void ChangeMoney(int amount)
    {
        money += amount;
        UpdateMoneyDisplay();
        UpdateItemDisplay();

        if(money < -100)
            LoseGame();
    }

    //Changes reputation and updates the display
    public void ChangeRep(int amount)
    {
        reputation += amount;

        if (reputation > 100)
            reputation = 100;

        //Lose condition
        else if (reputation < 0)
        {
            reputation = 0;
            UpdateRepDisplay();
            LoseGame();
        }

        UpdateRepDisplay();
    }

    //Called when a patient has healed. Increases money and reputation
    public void PatientCured()
    {
        ChangeMoney(curedMoneyReward);
        ChangeRep(curedRepReward);
    }

    //Changes death count and updates the display
    public void PatientDied()
    {
        deathCount++;
        UpdateDeathDisplay();
        ChangeMoney(-deathMoneyPenalty);
        ChangeRep(-deathCount);
    }
    
    //Display updates
    void UpdateMoneyDisplay()
    {
        //Update money display
        moneyText.text = money.ToString() + "$";
        if (money < 0)
            moneyText.color = Color.red;
        else
            moneyText.color = Color.black;

    }

    void UpdateRepDisplay()
    {
        //Update reputation
        float size = reputation / 100f;
        repBar.transform.localScale = new Vector3(size, 0.25f);
        repBar.GetComponent<Image>().color = GetColor();
    }

    void UpdateDeathDisplay()
    {
        //Update death text
        deathText.text = deathCount.ToString();
        int fontSize = deathCount * 2 + 12;
        if (fontSize > 36)
            fontSize = 36;
        deathText.fontSize = fontSize;
    }

    void UpdateItemDisplay()
    {
        for(int i = 0; i < items.Length; i++)
        { 
            if (items[i].GetComponent<Item>().price > money)
                items[i].GetComponent<Image>().color = Color.black;
            else
                items[i].GetComponent<Image>().color = Color.white;
        }
    }

    //Time management
    void SecondPassed()
    {
        playTime++;
        minTic.transform.Rotate(Vector3.back, 6f);
        hourTic.transform.Rotate(Vector3.back, 0.5f);

        int amountOfCalmLost = 0;
        int amountOfCorpses = 0;

        foreach (Patient patient in patients)
        {
            if(patient.Wound == PatientWounds.Dead)
            {
                amountOfCorpses++;
            }
        }

        //Update patients
        foreach (Patient patient in patients)
        {
            amountOfCalmLost -= 11 - Mathf.FloorToInt(patient.Calm / 10.0f);
            amountOfCalmLost -= amountOfCorpses;
            amountOfCalmLost -= 5 - Mathf.FloorToInt(reputation / 10.0f);
            patient.Calm = Mathf.Clamp(patient.Calm, 0, 100);
            switch (patient.Wound)
            {
                case PatientWounds.Healthy:
                    continue;
                case PatientWounds.Dead:
                    continue;
                case PatientWounds.Hemorhagie:
                    amountOfCalmLost -= hemorhagieCalmLost;
                    break;
                case PatientWounds.Psychology:
                    amountOfCalmLost -= psychologyCalmLost;
                    break;
                case PatientWounds.Vomitorium:
                    amountOfCalmLost -= vomitoriumCalmLost;
                    break;
                case PatientWounds.Surgery:
                    amountOfCalmLost -= surgeryCalmLost;
                    break;
                case PatientWounds.Exorcism:
                    amountOfCalmLost -= exorcismCalmLost;
                    break;
            }

            patient.UpdateStatsPatient(amountOfCalmLost);
        }

        //StateSwitch
        if(playTime%60 == 0)
        {
            GameFlowManager.GamePhase++;
        }
    }

    //Manage the static list of patients
    static public void AddPatient(Patient patient)
    {
        patients.Add(patient);
    }

    static public void RemovePatient(Patient patient)
    {
        patients.Remove(patient);
    }

    //Is called when a lose condition is met (reputation of 0, too much debt)
    void LoseGame()
    {

    }

    //Is called when the time is over
    void WinGame()
    {

    }
}

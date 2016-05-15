using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //Patients list
    static List<GameObject> patients;

    //Game Metrics
    int money;
    public int Money { get { return money; } }

    int reputation;
    int deathCount;

    public int curedMoneyReward;
    public int curedRepReward;
    public int deathMoneyPenalty;

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
	public void ChangeMoney(int ammount)
    {
        money += ammount;
        UpdateMoneyDisplay();
        UpdateItemDisplay();

        if(money < -100)
            LoseGame();
    }

    //Changes reputation and updates the display
    public void ChangeRep(int ammount)
    {
        reputation += ammount;

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

        //Update patients
        for(int i = 0; i < patients.Count; i++)
        {
            //TODO
        }

        //StateSwitch
        if(playTime%60 == 0)
        {
            //TODO
        }
    }

    //Manage the static list of patients
    static public void AddPatient(GameObject patient)
    {
        patients.Add(patient);
    }

    static public void RemovePatient(GameObject patient)
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

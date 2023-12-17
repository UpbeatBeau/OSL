using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float draftTime = 10;
    public bool timerOn = true;
    public Text timerText;
    public TeamManager man;
    public Team next;
    public Canvas onClockCan;
    public Canvas pickIsInCan;
    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (draftTime > 0)
        {
            draftTime -= Time.deltaTime;
            TimerTick(draftTime);
        }
        else
        {
            nextTeam();
            draftTime = 0;
            timerOn = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Onclockon();
            PickIn();
        }
 
        
    }

    void TimerTick(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void nextTeam()
    {
        man.GetComponent<TeamManager>().currentTeam = next;
        man.GetComponent<TeamManager>().UpdateTeam();
    }

    public void Onclockon()
    {
        if(onClockCan.enabled == false)
        {
            onClockCan.enabled = true;
        }
        else
        {
            onClockCan.enabled = false;
        }
    }
    
    public void PickIn()
    {
        if (pickIsInCan.enabled == false)
        {
            pickIsInCan.enabled = true;
        }
        else
        {
            pickIsInCan.enabled = false;
        }
    }
}

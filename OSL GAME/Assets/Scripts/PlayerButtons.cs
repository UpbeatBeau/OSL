using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerButtons : MonoBehaviour
{
    public GameManager gm;
    public TeamManager tm;
    public CSVread csv;
    private string targetplayername;
    private Canvas Drafttimer;
    private Canvas mycan;

    private void Start()
    {
        Drafttimer = GameObject.Find("Picks").GetComponent<Canvas>();
        mycan = this.GetComponent<Canvas>();
    }

    public void DraftMe()
    {
        tm = GameManager.instance.GetComponent<TeamManager>();
        csv = GameManager.instance.GetComponent<CSVread>();
        targetplayername = EventSystem.current.currentSelectedGameObject.transform.GetComponentInChildren<Text>().text;
        for(int i = 0;i < csv.clone.Length; i++)
        {
            if(csv.clone[i].playerName == targetplayername)
            {
                tm.currentTeam.draftedPlayers.Add(csv.clone[i]);
            }
        }
        Destroy(EventSystem.current.currentSelectedGameObject);
        
    }

    public void NextTeam()
    {
        Drafttimer.enabled = true;
        mycan.enabled = false;
        gm = GameManager.instance.GetComponent<GameManager>();
        gm.nextTeam();
        gm.draftTime = gm.maxTime;
        gm.isPicking = false;
        gm.timerOn = true;
    }
}

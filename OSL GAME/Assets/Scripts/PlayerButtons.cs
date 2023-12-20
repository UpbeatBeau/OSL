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
        gm = GameManager.instance.GetComponent<GameManager>();
        tm = GameManager.instance.GetComponent<TeamManager>();
        csv = GameManager.instance.GetComponent<CSVread>();
        string draftTeam = tm.currentTeam.teamName.Trim();
        Debug.Log(draftTeam);
        targetplayername = EventSystem.current.currentSelectedGameObject.transform.GetComponentInChildren<Text>().text;
        //Debug.Log("Target Player" + targetplayername);
        for(int i = 0;i < csv.clone.Length; i++)
        {
            if(csv.clone[i].playerName == targetplayername)
            {
                //Debug.Log("added");
                tm.currentTeam.draftedPlayers.Add(csv.clone[i].playerName);
                gm.lastDrafted.Clear();
                gm.lastDrafted.Add(csv.clone[i]);
                //tm.pick1.text = csv.clone[i].playerName;
            }
        }
        //Debug.Log(draftTeam);

        gm.lasttopick.Clear();
        gm.lasttopick.Add(tm.currentTeam);
        gm.UpdateDraftedPlayers(draftTeam);
        Destroy(EventSystem.current.currentSelectedGameObject);
        csv.Order.RemoveFirst();



    }

    public void NextTeam()
    {
        Drafttimer.enabled = true;
        mycan.enabled = false;
        gm = GameManager.instance.GetComponent<GameManager>();
        gm.nextTeam();
        StartCoroutine(timerPause(10f));
        gm.draftTime = gm.maxTime;
        gm.isPicking = false;
        
    }

  
    private IEnumerator timerPause(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            gm.timerOn = true;
        }
    }

    public void GoBack()
    {
        mycan.enabled = false;
        gm = GameManager.instance.GetComponent<GameManager>();
        gm.pickIsInCan.enabled = true;
    }
}

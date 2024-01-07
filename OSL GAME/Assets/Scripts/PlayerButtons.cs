using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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
        targetplayername = EventSystem.current.currentSelectedGameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text;
        //Debug.Log("Target Player" + targetplayername);
        for(int i = 0;i < csv.clone.Length; i++)
        {
            if(csv.clone[i].playerName == targetplayername)
            {
                //Debug.Log("added");
                tm.currentTeam.draftedPlayers.Add(csv.clone[i].playerName);
                gm.lastDrafted.Clear();
                gm.lastDrafted.Add(csv.clone[i]);
                int f = gm.choice;
                switch (f)
                {
                    case 1:
                        for (int x = 0; x < csv.osladcList.Count; x++)
                        {
                            if (csv.osladcList[x].name == targetplayername)
                            {
                                csv.osladcList.RemoveAt(x);
                            }
                        }
                        break;
                    case 2:
                        for (int x = 0; x < csv.oslsuppList.Count; x++)
                        {
                            if (csv.oslsuppList[x].name == targetplayername)
                            {
                                csv.oslsuppList.RemoveAt(x);
                            }
                        }
                        break;
                    case 3:
                        for (int x = 0; x < csv.oslmidList.Count; x++)
                        {
                            if (csv.oslmidList[x].name == targetplayername)
                            {
                                csv.oslmidList.RemoveAt(x);
                            }
                        }
                        break;
                    case 4:
                        for (int x = 0; x < csv.osljungList.Count; x++)
                        {
                            if (csv.osljungList[x].name == targetplayername)
                            {
                                csv.osljungList.RemoveAt(x);
                            }
                        }
                        break;

                }
            }

            gm.canfuck = true;
         
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
        gm.displaypick.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gm.lasttopick[0].teamName + "\nhave selected";
        gm.displaypick.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gm.lastDrafted[0].playerName;
        StartCoroutine(GameManager.instance.timerPause(10f));
        
        
    }

  
   

    public void GoBack()
    {
        mycan.enabled = false;
        gm = GameManager.instance.GetComponent<GameManager>();
        gm.pickIsInCan.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Globalization;
using System;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float draftTime;
    public bool timerOn = true;
    public bool isPicking = false;
    public float maxTime;
    public TextMeshProUGUI timerText;
    private TeamManager man;
    public TeamOBJ next;
    public Canvas onClockCan;
    public Canvas pickIsInCan;
    public UnityEngine.Object[] clearTeam;
    public CSVread csv;
    public Canvas draftTeam;
    public TextMeshProUGUI tmp;
    public List<PlayerObj> lastDrafted;
    public TextMeshProUGUI lastone;
    public List<TeamOBJ> lasttopick;
    public int choice;
    public bool canfuck = false;
    public PlayerButtons reset;

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
        csv = this.GetComponent<CSVread>();
        man = this.GetComponent<TeamManager>();
        timerOn = true;
        draftTime = maxTime;
        clearTeam = Resources.LoadAll<TeamOBJ>("Scriptable Obj/Teams");
        foreach(TeamOBJ t in clearTeam)
        {
            //Debug.Log(t.name);
            t.draftedPlayers.Clear();
        }
        File.SetAttributes(fileDraft, FileAttributes.Normal);
        File.Create(@fileDraft);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (draftTime > 0 && timerOn)
        {
            draftTime -= Time.deltaTime;
            TimerTick(draftTime);
        }else if(draftTime > 0 && !timerOn)
        {
            timerText.text = "Waiting!";
        }
        else if (!isPicking)
        {
            Onclockon();
            PickIn();
            draftTime = 0;
            timerOn = false;
            isPicking = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            draftTime = 0;
        }
        if (Input.GetKeyDown(KeyCode.D) && !isPicking)
        {
            draftTeam.enabled = true;
            onClockCan.enabled = false;
        }else if (Input.GetKeyUp(KeyCode.D) && !isPicking)
        {
            draftTeam.enabled = false;
            onClockCan.enabled = true;
        }else if (Input.GetKeyDown(KeyCode.Backspace) && canfuck)
        {
            FUCKEDIT();
        }else if (Input.GetKeyDown(KeyCode.P) && timerOn)
        {
            timerOn = false;
        }
        else if (Input.GetKeyDown(KeyCode.P) && !timerOn)
        {
            timerOn = true;
        }


    }

    public void UpdateDraftedPlayers(string TeamOBJ)
    {
        //Debug.Log("YOU DRAFTED");
        TextMeshProUGUI listofdrafted;
        List <string> teamList;
        listofdrafted = draftTeam.transform.Find(TeamOBJ).GetChild(0).GetComponent<TextMeshProUGUI>();
        teamList = man.currentTeam.draftedPlayers;
        listofdrafted.text = ListToPlayerText(teamList);
        Debug.Log(lastDrafted[0].playerName);
        lastone.text = lastDrafted[0].playerName;
    }

    private string ListToPlayerText(List<string> list)
    {
        string result = "";
        foreach(var listMember in list)
        {
            //Debug.Log(listMember.playerName);
            result += listMember + "\n";
        }
        return result;
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

        //Debug.Log(csv.Order.Peek());
        if (csv.Order.Count != 0)
        {
            
            string path = "Scriptable Obj/Teams/" + csv.Order.First.Value.Trim();
            
            Debug.Log(path);
            next = Resources.Load<TeamOBJ>(path);
            man.currentTeam = next;
            //Debug.Log(next.teamName);
            man.UpdateTeam();
            
        }
        else
        {
            EndDraft();
           //SceneManager.LoadScene("DraftOver");
        }
        MainTeam();
    }

    public void MainTeam()
    {
       
        tmp.text = man.currentTeam.teamName;
        tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ListToPlayerText(man.currentTeam.draftedPlayers);
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
        draftTeam.enabled = false;
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
        draftTeam.enabled = false;
        onClockCan.enabled = false;
    }

    List<string> finalTeams;
    string fileDraft = "/DraftResults.csv";
    
    
    
    public void EndDraft()
    {
         string path = Application.dataPath + "/Data/" + fileDraft;
        
        StreamWriter writer = new StreamWriter(path);
        //CsvWriter fileWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
        writer.WriteLine("OSLDRAFT\n");
        foreach (TeamOBJ t in clearTeam)
        {

            writer.WriteLine(t.teamName);
            //fileWriter.WriteRecords(t.draftedPlayers);
            for (int i = 0; i < t.draftedPlayers.Count; i++)
            {
               writer.WriteLine(t.draftedPlayers[i]);
            }
            writer.WriteLine("\n");
            writer.Flush();
            
        }
        writer.Close();

    }
   
    public void FUCKEDIT()
    {
        canfuck = false;
        csv.MessedUp();
        GameObject[] pbutts;
        pbutts = GameObject.FindGameObjectsWithTag("Pbutt");
        foreach (var p in pbutts)
        {
            Destroy(p);
        }
        lasttopick[0].draftedPlayers.RemoveAt(lasttopick[0].draftedPlayers.Count - 1);
        UpdateDraftedPlayers(lasttopick[0].teamName);
        lastone.text = "";
        csv.Order.AddFirst(lasttopick[0].name);
        reset.NextTeam();
        pickIsInCan.enabled = false;
        string rolecheck = lastDrafted[0].firstletter;
        if (String.Compare(rolecheck, "a", true) >= 0 && String.Compare(rolecheck, "f", true) <= 0)
        {
            csv.osladcList.Add(csv.lastosldrafted.player[0]);
        }
        else if (String.Compare(rolecheck, "g", true) >= 0 && String.Compare(rolecheck, "l", true) <= 0)
        {
            csv.oslsuppList.Add(csv.lastosldrafted.player[0]);
        }
        else if (String.Compare(rolecheck, "m", true) >= 0 && String.Compare(rolecheck, "s", true) <= 0)
        {
            csv.oslmidList.Add(csv.lastosldrafted.player[0]);

        }
        else if (String.Compare(rolecheck, "t", true) >= 0 && String.Compare(rolecheck, "z", true) <= 0)
        {
            csv.osljungList.Add(csv.lastosldrafted.player[0]);
        }
        else
        {
            csv.osljungList.Add(csv.lastosldrafted.player[0]);
        }
        GameObject[] spawners;
        spawners = GameObject.FindGameObjectsWithTag("Playerbutt");
        foreach (var p in spawners)
        {
            p.GetComponent<Canvas>().enabled = false;
            p.GetComponent<ButtonManager>().UpdateDisplay();
        }
        timerOn = false;
    }
    
}

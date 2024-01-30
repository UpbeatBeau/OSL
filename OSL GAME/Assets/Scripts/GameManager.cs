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
    public bool timerOn;
    public bool isPicking = false;
    public float maxTime;
    public TextMeshProUGUI timerText;
    private TeamManager man;
    public TeamOBJ next;
    public TeamOBJ nextupTeam;
    public Canvas onClockCan;
    public Canvas pickIsInCan;
    public Canvas displaypick;
    public UnityEngine.Object[] clearTeam;
    public CSVread csv;
    public Canvas draftTeam;
    public TextMeshProUGUI tmp;
    public List<PlayerObj> lastDrafted;
    public TextMeshProUGUI lastone;
    public Image lastteamlogo;
    public List<TeamOBJ> lasttopick;
    public int choice;
    public bool canfuck = false;
    public PlayerButtons reset;
    private bool endofDraft = false;
    public TextMeshProUGUI round;
    public TextMeshProUGUI pick;
    private int roundint = 1;
    private int pickint = 1;

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
        roundint = 1;
        pickint = 0;
        csv = this.GetComponent<CSVread>();
        man = this.GetComponent<TeamManager>();
        timerOn = true;
        draftTime = maxTime;
        clearTeam = Resources.LoadAll<TeamOBJ>("Scriptable Obj/Teams");
        foreach(TeamOBJ t in clearTeam)
        {
            //Debug.Log(t.name);
            if (t.teamName != "End of Draft")
            {
                t.draftedPlayers.Clear();
                TextMeshProUGUI listofdrafted;
                string captain;
                listofdrafted = draftTeam.transform.Find(t.teamName).GetChild(0).GetComponent<TextMeshProUGUI>();
                captain = t.Captain;
                listofdrafted.text = captain;
            }
        }
        File.SetAttributes(fileDraft, FileAttributes.Normal);
        //File.Create(@fileDraft);
        File.Create(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+fileDraft);

    }

    // Update is called once per frame
    void Update()
    {
        if (endofDraft)
        {
            man.overlay.color = new Color(0, 0, 0, .9f);
            draftTeam.enabled = true;
            onClockCan.enabled = false;
        }
        else
        {
            if (draftTime > 0 && timerOn)
            {
                draftTime -= Time.deltaTime;
                TimerTick(draftTime);
            }
            else if (draftTime > 0 && !timerOn)
            {
                timerText.text = "Waiting!";
            }
            else if (!isPicking)
            {
                onClockCan.enabled = false;
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
                man.overlay.color = new Color(0, 0, 0, .9f);
                draftTeam.enabled = true;
                onClockCan.enabled = false;
            }
            else if (Input.GetKeyUp(KeyCode.D) && !isPicking)
            {
                man.overlay.color = man.currentTeam.teamColor;
                draftTeam.enabled = false;
                onClockCan.enabled = true;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace) && canfuck)
            {
                FUCKEDIT();
            }
            else if (Input.GetKeyDown(KeyCode.P) && timerOn)
            {
                StopAllCoroutines();
                StopCoroutine("timerPause");
                timerOn = false;
            }
            else if (Input.GetKeyDown(KeyCode.P) && !timerOn)
            {
                timerOn = true;
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                EndDraft();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        round.text = "Round "+roundint;
        pick.text = "Pick " + pickint;

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void UpdateDraftedPlayers(string TeamOBJ)
    {
        //Debug.Log("YOU DRAFTED");
        TextMeshProUGUI listofdrafted;
        List <string> teamList;
        string captain;
        listofdrafted = draftTeam.transform.Find(TeamOBJ).GetChild(0).GetComponent<TextMeshProUGUI>();
        captain = man.currentTeam.Captain;
        teamList = man.currentTeam.draftedPlayers;
        listofdrafted.text = captain + "\n" + ListToPlayerText(teamList);
        Debug.Log(lastDrafted[0].playerName);
        lastone.text = lastDrafted[0].playerName;
        lastteamlogo.enabled = true;
        lastteamlogo.sprite = lasttopick[0].logo;
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
        if(pickint != 16)
        {
            pickint++;
        }
        else
        {
            pickint = 1;
            roundint++;
        }
        
        //Debug.Log(csv.Order.Peek());
        if (csv.Order.Count != 0)
        {
            
            string path = "Scriptable Obj/Teams/" + csv.Order.First.Value.Trim();
            
            //Debug.Log(path);
            next = Resources.Load<TeamOBJ>(path);
            man.currentTeam = next;
            if (csv.Order.Count > 1)
            {
                string nextuppath = "Scriptable Obj/Teams/" + csv.Order.First.Next.Value.Trim();
                nextupTeam = Resources.Load<TeamOBJ>(nextuppath);
            }
            else
            {
                nextupTeam = Resources.Load<TeamOBJ>("Scriptable Obj/Teams/xEndofDraft");
            }
            //Debug.Log(next.teamName);
            man.UpdateTeam();
            
        }
        else
        {
            endofDraft = true;
            EndDraft();            
        }
        MainTeam();
    }

    public void MainTeam()
    {
       
        tmp.text = man.currentTeam.teamName;
        tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "<color=#db9904>Captain</color>" + "\n" + man.currentTeam.Captain + "\n" + "<color=#db9904>Draft Picks</color>" + "\n" + ListToPlayerText(man.currentTeam.draftedPlayers);
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
        if (displaypick.enabled == false)
        {
            displaypick.enabled = true;
        }
        else
        {
            displaypick.enabled = false;
        }
        draftTeam.enabled = false;
        onClockCan.enabled = false;
        man.overlay.color = new Color(0, 0, 0, 0f);
    }

    List<string> finalTeams;
    string fileDraft = "/DraftResults.csv";
    
    
    
    public void EndDraft()
    {
         //string path = Application.dataPath + "/Data/" + fileDraft;
        string path = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + fileDraft;
        
        StreamWriter writer = new StreamWriter(path,false);
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
        man.overlay.color = new Color(0, 0, 0, .9f);
        displaypick.enabled = false;
        onClockCan.enabled = false;
        pickIsInCan.enabled = false;
        draftTeam.enabled = true;
        draftTime = 100000f;
        Destroy(this);

    }
   
    public void FUCKEDIT()
    {
        if (pickint != 1)
        {
            pickint--;
            pickint--;
        }
        else
        {
            pickint = 15;
            roundint--;
        }
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
        onClockCan.enabled = true;
        draftTime = maxTime;
        isPicking = false;
        displaypick.enabled = false;
        nextTeam();
        displaypick.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "The pick is in";
        displaypick.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = " ";
        timerOn = true;
        pickIsInCan.enabled = false;
        onClockCan.enabled = true;
        displaypick.enabled = false;
       
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
    public IEnumerator timerPause(float waitTime)
    {
        onClockCan.enabled = false;
        yield return new WaitForSeconds(waitTime);
        draftTime = maxTime;
        isPicking = false;
        onClockCan.enabled = true;
        displaypick.enabled = false;
        nextTeam();
        displaypick.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "The pick is in";
        displaypick.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = " ";
        yield return new WaitForSeconds(3f);
        timerOn = true;
        StopCoroutine("timerPause");
        StopAllCoroutines();
    }

}

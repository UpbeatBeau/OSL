using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System.Globalization;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float draftTime;
    public bool timerOn = true;
    public bool isPicking = false;
    public float maxTime;
    public Text timerText;
    private TeamManager man;
    public Team next;
    public Canvas onClockCan;
    public Canvas pickIsInCan;
    public Object[] clearTeam;
    public CSVread csv;
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
        clearTeam = Resources.LoadAll<Team>("Scriptable Obj/Teams");
        foreach(Team t in clearTeam)
        {
            //Debug.Log(t.name);
            t.draftedPlayers.Clear();
        }
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
 
        
    }

    public void UpdateDraftedPlayers(string team)
    {
        //Debug.Log("YOU DRAFTED");
        TextMeshProUGUI listofdrafted;
        List <string> teamList;
        listofdrafted = onClockCan.transform.Find(team).GetChild(0).GetComponent<TextMeshProUGUI>();
        teamList = man.currentTeam.draftedPlayers;
        listofdrafted.text = ListToPlayerText(teamList);
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
            string path = "Scriptable Obj/Teams/" + csv.Order.Dequeue().Trim();
            //Debug.Log(path);
            next = Resources.Load<Team>(path);
            man.currentTeam = next;
            //Debug.Log(next.teamName);
            man.UpdateTeam();
        }
        else
        {
            EndDraft();
           //SceneManager.LoadScene("DraftOver");
        }
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

    List<string> finalTeams;
    string fileDraft = "/DraftResults.csv";
    
    
    
    public void EndDraft()
    {
         string path = Application.dataPath + "/Data/" + fileDraft;
        
        StreamWriter writer = new StreamWriter(path);
        //CsvWriter fileWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
        writer.WriteLine("OSLDRAFT\n");
        foreach (Team t in clearTeam)
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
    
}

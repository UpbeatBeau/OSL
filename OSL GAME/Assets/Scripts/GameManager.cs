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
    public float maxTime = 60f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI draftedplayer;
    private TeamManager man;
    public TeamOBJ next;
    public TeamOBJ nextupTeam;
    public TeamOBJ holeTeam;
    public TeamOBJ fourthupTeam;
    public TeamOBJ fifthTeam;
    public Canvas onClockCan;
    public Canvas pickIsInCan;
    //public Canvas displaypick;
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
    public GameObject Backdrop;
    public GameObject TeamDrop;
    private string targetplayername;

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
        maxTime = 60f;
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
                //string captain;
                listofdrafted = draftTeam.gameObject.transform.Find(t.teamName.Trim()).GetComponent<TextMeshProUGUI>();
                //captain = t.Captain;
                listofdrafted.text = " ";
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
            man.overlay.color = new Color(0, 0, 0, 0f);
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
                TeamDrop.SetActive(false);
                Backdrop.SetActive(true);
                timerText.text = "Picking!";
                onClockCan.enabled = true;
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
                TeamDrop.SetActive(true);
                Backdrop.SetActive(false);
                //man.overlay.color = new Color(0, 0, 0, 0f);
                draftTeam.enabled = true;
                onClockCan.enabled = false;
            }
            else if (Input.GetKeyUp(KeyCode.D) && !isPicking)
            {
                TeamDrop.SetActive(false);
                Backdrop.SetActive(true);
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
            else if (Input.GetKeyDown(KeyCode.S))
            {
                string draftTeam = man.currentTeam.teamName.Trim();
                targetplayername = "Skipped";
                //Debug.Log("Target Player" + targetplayername);
                //Debug.Log("added");
                man.currentTeam.draftedPlayers.Add("Skipped");
                lastDrafted.Clear(); 
                canfuck = true;
                //Debug.Log(draftTeam);
                lasttopick.Clear();
                lasttopick.Add(man.currentTeam);
                lastDrafted.Add(Resources.Load<PlayerObj>("Scriptable OBJ/Players/Skipped"));
                UpdateDraftedPlayers(draftTeam);
                csv.Order.RemoveFirst();
                draftedplayer.text = lastDrafted[0].playerName;
                StartCoroutine(GameManager.instance.timerPause(5f));

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
        listofdrafted = draftTeam.transform.Find(TeamOBJ).GetComponent<TextMeshProUGUI>();
        captain = man.currentTeam.Captain;
        teamList = man.currentTeam.draftedPlayers;
        listofdrafted.text = ListToPlayerText(teamList);
        Debug.Log(lastDrafted[0].playerName);
       
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
        else if(roundint == 2 && pickint == 16)
        {
            maxTime = 120f;
            pickint = 1;
            roundint++;
        }else
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
            if (csv.Order.Count > 4)
            {
                string nextuppath = "Scriptable Obj/Teams/" + csv.Order.First.Next.Value.Trim();
                nextupTeam = Resources.Load<TeamOBJ>(nextuppath);
                string holepath = "Scriptable Obj/Teams/" + csv.Order.First.Next.Next.Value.Trim();
                holeTeam = Resources.Load<TeamOBJ>(holepath);
                string fourthpath = "Scriptable Obj/Teams/" + csv.Order.First.Next.Next.Next.Value.Trim();
                fourthupTeam = Resources.Load<TeamOBJ>(fourthpath);
                string fifthpath = "Scriptable Obj/Teams/" + csv.Order.First.Next.Next.Next.Next.Value.Trim();
                fifthTeam = Resources.Load<TeamOBJ>(fifthpath);
            }else if(csv.Order.Count > 3){
                string nextuppath = "Scriptable Obj/Teams/" + csv.Order.First.Next.Value.Trim();
                nextupTeam = Resources.Load<TeamOBJ>(nextuppath);
                string holepath = "Scriptable Obj/Teams/" + csv.Order.First.Next.Next.Value.Trim();
                holeTeam = Resources.Load<TeamOBJ>(holepath);
                string fourthpath = "Scriptable Obj/Teams/" + csv.Order.First.Next.Next.Next.Value.Trim();
                fourthupTeam = Resources.Load<TeamOBJ>(fourthpath);
            }
            else if (csv.Order.Count > 2) {
                string nextuppath = "Scriptable Obj/Teams/" + csv.Order.First.Next.Value.Trim();
                nextupTeam = Resources.Load<TeamOBJ>(nextuppath);
                string holepath = "Scriptable Obj/Teams/" + csv.Order.First.Next.Next.Value.Trim();
                holeTeam = Resources.Load<TeamOBJ>(holepath);
            }
            else if (csv.Order.Count > 1)
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
        /*if (displaypick.enabled == false)
        {
            displaypick.enabled = true;
        }
        else
        {
            displaypick.enabled = false;
        }*/
        draftTeam.enabled = false;
        //onClockCan.enabled = false;
        //man.overlay.color = new Color(0, 0, 0, 0f);
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
        TeamDrop.SetActive(true);
        Backdrop.SetActive(false);
        man.overlay.color = new Color(0, 0, 0, 0f);
        //displaypick.enabled = false;
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
        lastteamlogo.enabled = false;
        lastone.text = "";
        csv.Order.AddFirst(lasttopick[0].name);
        onClockCan.enabled = true;
        draftTime = maxTime;
        isPicking = false;
        //displaypick.enabled = false;
        nextTeam();
        //displaypick.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "The pick is in";
        //displaypick.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = " ";
        timerOn = true;
        pickIsInCan.enabled = false;
        onClockCan.enabled = true;
        //displaypick.enabled = false;
       
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
        //onClockCan.enabled = false;
        
        draftedplayer.transform.localScale = new Vector3(.25f, .25f, .25f);
        Vector3 draftedtargetscale = new Vector3(1.5f, 1.5f, 1.5f);
        Vector3 draftedmovescale = new Vector3(.64f, .64f, .64f);
        Vector2 draftedtargetlocation = new Vector2(-241f, 45f);
        Vector2 draftedstartlocation = new Vector2(11f, -186f);
        while (draftedplayer.transform.localScale != draftedtargetscale)
        {
            yield return null;
            draftedplayer.transform.localScale = Vector3.MoveTowards(draftedplayer.transform.localScale, draftedtargetscale, 1f * Time.deltaTime);

        }
        yield return new WaitForSeconds(waitTime);
        Vector2 teamlogotarget = new Vector3(-242.5532f, 180.7568f);
        Vector2 nextteamlogotarget = new Vector3(15f, 111f);
        Vector2 thirdteamlogotarget = new Vector3(272.3226f, 162.8841f);
        Vector2 fourthteamlogotarget = new Vector3(415.8289f, 162.8841f);
        Vector2 fifthteamlogotarget = new Vector3(556.6899f, 162.8841f);
        Vector2 ogfifthspotlogo = new Vector3(698.8735f, 162.8841f);
        Vector3 firstScaleog = new Vector3(2.65f, 2.65f, 2.65f);
        Vector3 secondScaleog = new Vector3(1.569f, 1.569f, 1.569f);
        Color firstalpha = new Color(255, 255, 255, 1);
        Color secondalpha = new Color(255, 255, 255, .75f);
        Color thirdalpha = new Color(255, 255, 255, .5f);
        Color fourthalpha = new Color(255, 255, 255, .25f);
        Color fifthalpha = new Color(255, 255, 255, .15f);
        Color nextalpha = new Color(255,255,255,.75f);
        while (man.teamlogo.rectTransform.anchoredPosition != teamlogotarget)
        {
            yield return null;
            lastteamlogo.color = Vector4.MoveTowards(lastteamlogo.color, new Color(0, 0, 0, 0), 100f * Time.deltaTime);
            lastone.color = Vector4.MoveTowards(lastteamlogo.color, new Color(0, 0, 0, 0), 50f * Time.deltaTime);
            man.teamlogo.color = Vector4.MoveTowards(man.teamlogo.color, nextalpha, 50f * Time.deltaTime);
            man.nextuplogo.color = Vector4.MoveTowards(man.nextuplogo.color, firstalpha, 50f * Time.deltaTime);
            man.intheholeteam.color = Vector4.MoveTowards(man.intheholeteam.color, secondalpha, 50f * Time.deltaTime);
            man.fourthtopickteam.color = Vector4.MoveTowards(man.fourthtopickteam.color, thirdalpha, 50f * Time.deltaTime);
            man.fifthpickteam.color = Vector4.MoveTowards(man.fifthpickteam.color, fourthalpha, 50f * Time.deltaTime);
            draftedplayer.transform.localScale = Vector3.MoveTowards(draftedplayer.rectTransform.localScale, draftedmovescale, .5f * Time.deltaTime);
            man.teamlogo.rectTransform.localScale = Vector3.MoveTowards(man.teamlogo.rectTransform.localScale, secondScaleog, .5f * Time.deltaTime);
            man.nextuplogo.rectTransform.localScale = Vector3.MoveTowards(man.nextuplogo.rectTransform.localScale, firstScaleog, .5f * Time.deltaTime);
            draftedplayer.rectTransform.anchoredPosition = Vector2.MoveTowards(draftedplayer.rectTransform.anchoredPosition, draftedtargetlocation, 80f * Time.deltaTime);
            man.teamlogo.rectTransform.anchoredPosition = Vector2.MoveTowards(man.teamlogo.rectTransform.anchoredPosition, teamlogotarget, 55f * Time.deltaTime);
            man.nextuplogo.rectTransform.anchoredPosition = Vector2.MoveTowards(man.nextuplogo.rectTransform.anchoredPosition, nextteamlogotarget, 55f * Time.deltaTime);
            man.intheholeteam.rectTransform.anchoredPosition = Vector2.MoveTowards(man.intheholeteam.rectTransform.anchoredPosition, thirdteamlogotarget, 35f * Time.deltaTime);
            man.fourthtopickteam.rectTransform.anchoredPosition = Vector2.MoveTowards(man.fourthtopickteam.rectTransform.anchoredPosition, fourthteamlogotarget, 35f * Time.deltaTime);
            man.fifthpickteam.rectTransform.anchoredPosition = Vector2.MoveTowards(man.fifthpickteam.rectTransform.anchoredPosition, fifthteamlogotarget, 35f * Time.deltaTime);
        }
        lastteamlogo.color = nextalpha;
        lastone.color = nextalpha;
        man.nextuplogo.color = secondalpha;
        man.teamlogo.color = firstalpha;
        man.intheholeteam.color = thirdalpha;
        man.fourthtopickteam.color = fourthalpha;
        man.fifthpickteam.color = fifthalpha;
        man.teamlogo.rectTransform.anchoredPosition = nextteamlogotarget;
        man.nextuplogo.rectTransform.anchoredPosition = thirdteamlogotarget;
        man.intheholeteam.rectTransform.anchoredPosition = fourthteamlogotarget;
        man.fourthtopickteam.rectTransform.anchoredPosition = fifthteamlogotarget;
        man.fifthpickteam.rectTransform.anchoredPosition = ogfifthspotlogo;
        man.teamlogo.rectTransform.localScale = firstScaleog;
        man.nextuplogo.rectTransform.localScale = secondScaleog;
        draftedplayer.text = " ";
        draftedplayer.rectTransform.anchoredPosition = draftedstartlocation;
        draftedplayer.transform.localScale = new Vector3(.25f, .25f, .25f);
        
        isPicking = false;
        onClockCan.enabled = true;
        lastone.text = lastDrafted[0].playerName;
        lastteamlogo.enabled = true;
        lastteamlogo.sprite = lasttopick[0].logo;
        //displaypick.enabled = false;
        nextTeam();
        draftTime = maxTime;

        //displaypick.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "The pick is in";
        //displaypick.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = " ";
        yield return new WaitForSeconds(2f);
        timerOn = true;
        StopCoroutine("timerPause");
        StopAllCoroutines();
    }

    
}

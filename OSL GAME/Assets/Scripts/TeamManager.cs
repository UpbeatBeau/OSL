using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamManager : MonoBehaviour
{
    public TeamOBJ currentTeam;
    public SpriteRenderer overlay;
    public TextMeshProUGUI teamName;
    public Image teamlogo;
    public Image nextuplogo;
    public TextMeshProUGUI nextupteamname;
    //public Text pick1;


    // Start is called before the first frame update
    void Start()
    {
       
        //UpdateTeam();
    }

   public void UpdateTeam()
    {
        overlay.color=currentTeam.teamColor;
        //teamName.color = currentTeam.teamColor;
        teamName.text = currentTeam.teamName;
        teamlogo.sprite = currentTeam.logo;
        nextupteamname.text = GameManager.instance.nextupTeam.teamName;
        nextuplogo.sprite = GameManager.instance.nextupTeam.logo;
    }
}

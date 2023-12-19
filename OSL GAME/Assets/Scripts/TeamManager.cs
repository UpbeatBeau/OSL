using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamManager : MonoBehaviour
{
    public Team currentTeam;
    public Camera cam;
    public TextMeshProUGUI teamName;
    public Image teamlogo;
    //public Text pick1;


    // Start is called before the first frame update
    void Start()
    {
       
        UpdateTeam();
    }

   public void UpdateTeam()
    {
        cam.backgroundColor=currentTeam.teamColor;
        //teamName.color = currentTeam.teamColor;
        teamName.text = currentTeam.teamName;
        teamlogo.sprite = currentTeam.logo;
    }
}

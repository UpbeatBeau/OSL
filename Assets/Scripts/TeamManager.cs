using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public Team currentTeam;

    public Text teamName;
    public Image teamlogo;


    // Start is called before the first frame update
    void Start()
    {
        UpdateTeam();
    }

   public void UpdateTeam()
    {
        teamName.text = currentTeam.teamName;
        teamlogo.sprite = currentTeam.logo;
    }
}

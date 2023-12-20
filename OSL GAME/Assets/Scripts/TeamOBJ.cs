using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "NewTeam", menuName = "ScriptableObjects/Teams", order = 0)]
public class TeamOBJ : ScriptableObject
{
    public string teamName = "Team Name";
    public Color teamColor;
    public Sprite logo;
    public bool onClock;
    public List<string> draftedPlayers;
}

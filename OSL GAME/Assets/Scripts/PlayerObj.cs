using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTeam", menuName = "ScriptableObjects/Players", order = 0)]
public class PlayerObj : ScriptableObject
{
    public string playerName;
    public string firstletter;
    public string playerPos;
    
}

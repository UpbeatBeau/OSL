using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RoleButtonManager : MonoBehaviour
{
    public Canvas roleCan;
    public GameManager gm;
    public ALPHABET goal;

    private void Start()
    {
        
    }

    public void ChooseRole()
    {
        gm = GameManager.instance.GetComponent<GameManager>();
        goal = EventSystem.current.currentSelectedGameObject.GetComponent<ALPHABET>();
            
        gm.choice = goal.rolechoice;
        //Debug.Log(goal);
        roleCan = GameObject.Find(goal.rolecanTarget).GetComponent<Canvas>();
        roleCan.enabled = true;
        gm.pickIsInCan.enabled = false;
    }


}

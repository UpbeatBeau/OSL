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
    public string goal;

    private void Start()
    {
        
    }

    public void ChooseRole()
    {
        gm = GameManager.instance.GetComponent<GameManager>();
        goal = EventSystem.current.currentSelectedGameObject.GetComponent<ALPHABET>().rolecanTarget;
        Debug.Log(goal);
        roleCan = GameObject.Find(goal).GetComponent<Canvas>();
        roleCan.enabled = true;
        gm.pickIsInCan.enabled = false;
    }

    public void BacktoPick()
    {

    }

}

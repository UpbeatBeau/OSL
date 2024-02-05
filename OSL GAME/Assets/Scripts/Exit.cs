using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public void EndThisBitch()
    {
        Application.Quit();
    }

    public void StartThisBitch()
    {
        GameManager.instance.timerOn = true;
        GameManager.instance.onClockCan.enabled = true;
        GameManager.instance.displaypick.enabled = false;
        GameManager.instance.Backdrop.SetActive(true);
        GameManager.instance.GetComponent<TeamManager>().overlay.enabled = true;
    }
}

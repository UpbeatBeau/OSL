using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public GameObject button;
    
    private CSVread gm;
    private Text btext;
    private GameObject newbut;
    public Transform WhereYouWantButtonsParented;
    public Vector3 offset;
    public string roleyouWant;
    public int buttonfixoff = 6;
    
    private void Awake()
    {
        WhereYouWantButtonsParented = this.transform;
        offset = new Vector3(-540,320, 0);
       
        //UpdateDisplay();
    }
    private void Start()
    {
        UpdateDisplay();
       
    }

    void MakePlayersButton(List<CSVread.Player> role)
    {
        for (int i = 0; i < role.Count; i++)
        {
            //Debug.Log(gm.clone[i].playerPos);
            //Debug.Log(role);
                if (i <= 5)
                {
                    Vector3 place = new Vector3(i * 200, 0) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<Text>();
                    btext.text = role[i].name;
                }
                else if (i > 5 && i <= 11)
                {
                    Vector3 place = new Vector3((i - buttonfixoff) * 200, -50) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<Text>();
                    btext.text = role[i].name;
                }
                else if (i > 11 && i <= 17)
                {
                    Vector3 place = new Vector3((i - 2*buttonfixoff) * 200, -100) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<Text>();
                    btext.text = role[i].name;
                }else if (i > 17 && i <= 23)
                {
                Vector3 place = new Vector3((i - 3 * buttonfixoff) * 200, -150) + offset;
                newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                RectTransform rect = (RectTransform)newbut.transform;
                rect.anchoredPosition = place;
                btext = newbut.transform.GetChild(0).GetComponent<Text>();
                btext.text = role[i].name;
                }else if (i > 23 && i <= 29)
                {
                Vector3 place = new Vector3((i - 4 * buttonfixoff) * 200,-200) + offset;
                newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                RectTransform rect = (RectTransform)newbut.transform;
                rect.anchoredPosition = place;
                btext = newbut.transform.GetChild(0).GetComponent<Text>();
                btext.text = role[i].name;
                }else if (i > 29 && i <= 35)
                {
                Vector3 place = new Vector3((i - 5 * buttonfixoff) * 200, -250) + offset;
                newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                RectTransform rect = (RectTransform)newbut.transform;
                rect.anchoredPosition = place;
                btext = newbut.transform.GetChild(0).GetComponent<Text>();
                btext.text = role[i].name;
                }
        }
    }


    public void UpdateDisplay()
    {
        
        gm = GameManager.instance.GetComponent<CSVread>();


        switch (roleyouWant)
        {
            case "adc":
                MakePlayersButton(gm.osladcList);
                break;
            case "support":
                MakePlayersButton(gm.oslsuppList);
                break;
            case "mid":
                MakePlayersButton(gm.oslmidList);
                break;
            case "jungle":
                MakePlayersButton(gm.osljungList);
                break;
            case "solo":
                MakePlayersButton(gm.oslsoloList);
                break;
        }
    }
}

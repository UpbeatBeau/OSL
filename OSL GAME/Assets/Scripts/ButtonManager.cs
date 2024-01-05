using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonManager : MonoBehaviour
{

    public GameObject button;
    
    private CSVread gm;
    private TextMeshProUGUI btext;
    private GameObject newbut;
    public Transform WhereYouWantButtonsParented;
    public Vector3 offset;
    public string roleyouWant;
    public int buttonfixoff = 6;
    
    private void Awake()
    {
        WhereYouWantButtonsParented = this.transform;
        offset = new Vector3(-500,320, 0);
       
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
                    Vector3 place = new Vector3(i * 200, -50) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    btext.text = role[i].name;
                }
                else if (i > 5 && i <= 11)
                {
                    Vector3 place = new Vector3((i - buttonfixoff) * 200, -100) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    btext.text = role[i].name;
                }
                else if (i > 11 && i <= 17)
                {
                    Vector3 place = new Vector3((i - 2*buttonfixoff) * 200, -150) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    btext.text = role[i].name;
                }else if (i > 17 && i <= 23)
                {
                    Vector3 place = new Vector3((i - 3 * buttonfixoff) * 200, -200) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    btext.text = role[i].name;
                }else if (i > 23 && i <= 29)
                {
                    Vector3 place = new Vector3((i - 4 * buttonfixoff) * 200,-250) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    btext.text = role[i].name;
                }else if (i > 29 && i <= 35)
                {
                    Vector3 place = new Vector3((i - 5 * buttonfixoff) * 200, -300) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    btext.text = role[i].name;
                }else if (i > 35 && i <= 41)
                {
                    Vector3 place = new Vector3((i - 6 * buttonfixoff) * 200, -350) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    btext.text = role[i].name;
                }else if (i > 41 && i <= 47)
                {
                    Vector3 place = new Vector3((i - 7 * buttonfixoff) * 200, -400) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    btext.text = role[i].name;
                }else if (i > 47 && i <= 53)
                {
                    Vector3 place = new Vector3((i - 8 * buttonfixoff) * 200, -450) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    btext.text = role[i].name;
                }else if (i > 53 && i <= 59)
                {
                    Vector3 place = new Vector3((i - 9 * buttonfixoff) * 200, -500) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    btext.text = role[i].name;
                }else if (i > 59 && i <= 65)
                {
                    Vector3 place = new Vector3((i - 10 * buttonfixoff) * 200, -550) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
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
                gm.osladcList.Sort(PlayerSort.Comparison);
                MakePlayersButton(gm.osladcList);
                break;
            case "support":
                gm.oslsuppList.Sort(PlayerSort.Comparison);
                MakePlayersButton(gm.oslsuppList);
                break;
            case "mid":
                gm.oslmidList.Sort(PlayerSort.Comparison);
                MakePlayersButton(gm.oslmidList);
                break;
            case "jungle":
                gm.osljungList.Sort(PlayerSort.Comparison);
                MakePlayersButton(gm.osljungList);
                break;
            case "solo":
                gm.oslsoloList.Sort(PlayerSort.Comparison);
                MakePlayersButton(gm.oslsoloList);
                break;
        }
    }
}

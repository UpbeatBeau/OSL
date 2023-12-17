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
    public string possitionForNow;

    public int width;
    public int height;

    private int[,] grid;

    private List<GameObject> spawnButton = new List<GameObject>();

    private void Awake()
    {
        WhereYouWantButtonsParented = this.transform;
        offset = new Vector3(-600, 0, 0);
        
        grid = new int[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                grid[x, y] = 0;
            }
        }
        //UpdateDisplay();
    }
    private void Start()
    {
        gm = GameManager.instance.GetComponent<CSVread>();

        MakePlayersButton(possitionForNow);
    }

    void MakePlayersButton(string role)
    {
        for (int i = 0; i < gm.tableSize; i++)
        {
            Debug.Log(gm.clone[i].playerPos);
            Debug.Log(role);
            if(role == gm.clone[i].playerPos.Trim())
            {
                Debug.Log("WOOOOOOOOOT");
            }

                if (i <= 6 && gm.clone[i].playerPos.Trim() == role)
                {
                    Vector3 place = new Vector3(i * 200, 350) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<Text>();
                    btext.text = gm.clone[i].playerName;
                }
                else if (i > 6 && i <= 12 && gm.clone[i].playerPos.Trim() == role)
                {
                    Vector3 place = new Vector3((i - 7) * 200, 300) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<Text>();
                    btext.text = gm.clone[i].playerName;
                }
                else if (i > 12 && i <= 18 && gm.clone[i].playerPos.Trim() == role)
                {
                    Vector3 place = new Vector3((i - 13) * 200, 250) + offset;
                    newbut = Instantiate<GameObject>(button.gameObject, WhereYouWantButtonsParented);
                    RectTransform rect = (RectTransform)newbut.transform;
                    rect.anchoredPosition = place;
                    btext = newbut.transform.GetChild(0).GetComponent<Text>();
                    btext.text = gm.clone[i].playerName;
                }
        }
    }


    /*void UpdateDisplay()
    {
        foreach (var button in spawnButton)
        {
            Destroy(button);
        }


        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (grid[x, y] == 0)
                {
                    var butt = Instantiate(button);
                    butt.transform.position = new Vector3(x, y);
                    butt.transform.SetParent(canvas.transform, true);
                    spawnButton.Add(butt);
                    GameObject life = butt.gameObject;
                    butt.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.ButtonPress(life));
                }
            }
        }
    }*/
}

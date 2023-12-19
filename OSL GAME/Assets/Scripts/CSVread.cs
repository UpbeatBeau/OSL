using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSVread : MonoBehaviour
{
    private TextAsset TextAssetData;
    public TextAsset DraftOrderData;
    public PlayerObj[] clone;
    public int tableSize;
    public int draftTableSize;
    public string[] draftOrder;
    

    [System.Serializable]
    public class Player
    {
        public string name;
        public string role;
    }
    [System.Serializable]
    public class PlayerList
    {
        public Player[] player;
    }


    public PlayerList oslList = new PlayerList();
    public List<Player> osladcList = new List<Player>(); //adc list
    public List<Player> oslsuppList = new List<Player>(); // supp list
    public List<Player> osljungList = new List<Player>(); // jung list
    public List<Player> oslmidList = new List<Player>(); // mid list
    public List<Player> oslsoloList = new List<Player>();
    public Queue<string> Order;//draft order
    // Start is called before the first frame update
    void Awake()
    {
        ReadCSV();
        ReadCSVTeams();
        //GameManager.instance.GetComponent<GameManager>().nextTeam();
    }
    private void Start()
    {
        GameManager.instance.GetComponent<GameManager>().nextTeam();
    }

    void ReadCSV()
    {
        TextAssetData = Resources.Load<TextAsset>("OSLDraft");
        string[] data = TextAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        tableSize = data.Length / 2 - 1;//data.lenght divided by column number minus the header row
        oslList.player = new Player[tableSize];//master array
      
        clone = new PlayerObj[tableSize];

        for (int i = 0; i < tableSize; i++)
        {

            oslList.player[i] = new Player();
            oslList.player[i].name = data[2 * (i + 1)];
            oslList.player[i].role = data[2 * (i + 1) + 1];
            clone[i] = ScriptableObject.CreateInstance<PlayerObj>();
            clone[i].playerName = oslList.player[i].name;
            clone[i].playerPos = oslList.player[i].role;
            string rolecheck = oslList.player[i].role.Trim();
            switch (rolecheck)
            {
                case "adc":
                    osladcList.Add(oslList.player[i]);
                    break;
                case "supp":
                    oslsuppList.Add(oslList.player[i]);
                    break;
                case "jung":
                    osljungList.Add(oslList.player[i]);
                    break;
                case "mid":
                    oslmidList.Add(oslList.player[i]);
                    break;
                case "solo":
                    oslsoloList.Add(oslList.player[i]);
                    break;

            }
        }
       
    }
    void ReadCSVTeams()
    {
        DraftOrderData = Resources.Load<TextAsset>("OSLOrder");
        draftOrder = DraftOrderData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        Order = new Queue<string>();


        draftTableSize = draftOrder.Length-1;
        Debug.Log(draftTableSize);
        for (int i = 0; i <= draftTableSize; i++)
        {
            //Debug.Log(draftOrder[i]);
            Order.Enqueue(draftOrder[i]);
            
        }
        //GameManager.instance.GetComponent<GameManager>().nextTeam();
    }
}

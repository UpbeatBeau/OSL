using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

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
    public PlayerList lastosldrafted = new PlayerList();
    public LinkedList<string> Order;//draft order
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
            clone[i].firstletter = oslList.player[i].name.Substring(0, 1);
            string rolecheck = clone[i].firstletter;
            if (String.Compare(rolecheck, "a",true) >= 0 && String.Compare(rolecheck, "f",true) <= 0)
            {
                osladcList.Add(oslList.player[i]);
            }else if (String.Compare(rolecheck, "g",true) >= 0 && String.Compare(rolecheck, "l",true)<= 0)
            {
                oslsuppList.Add(oslList.player[i]);
            }
            else if (String.Compare(rolecheck, "m", true) >= 0 && String.Compare(rolecheck, "s", true) <= 0)
            {
                oslmidList.Add(oslList.player[i]);
                
            }
            else if (String.Compare(rolecheck, "t", true) >= 0 && String.Compare(rolecheck, "z", true) <= 0)
            {
                osljungList.Add(oslList.player[i]);
            }
            else
            {
                osljungList.Add(oslList.player[i]);
            }
        }
       
    }
    void ReadCSVTeams()
    {
        DraftOrderData = Resources.Load<TextAsset>("OSLOrder");
        draftOrder = DraftOrderData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        Order = new LinkedList<string>();
       

        draftTableSize = draftOrder.Length-1;
        //Debug.Log(draftTableSize);
        for (int i = 0; i <= draftTableSize; i++)
        {
            
            Order.AddLast(draftOrder[i]);
            
        }
        //Display(Order, "test");
        //GameManager.instance.GetComponent<GameManager>().nextTeam();
    }

    private static void Display(LinkedList<string> words, string test)
    {
        Console.WriteLine(test);
        foreach (string word in words)
        {
            Console.Write(word + " ");
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    public void MessedUp()
    {
        lastosldrafted.player = new Player[1];
        lastosldrafted.player[0] = new Player();
        lastosldrafted.player[0].name = GameManager.instance.GetComponent<GameManager>().lastDrafted[0].playerName;
        lastosldrafted.player[0].role = GameManager.instance.GetComponent<GameManager>().lastDrafted[0].playerPos;
    }
}

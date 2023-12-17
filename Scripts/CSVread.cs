using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSVread : MonoBehaviour
{
    public TextAsset TextAssetData;
    public PlayerObj[] clone;
    public int tableSize;

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
    // Start is called before the first frame update
    void Awake()
    {
        ReadCSV();
    }

    void ReadCSV()
    {
        string[] data = TextAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        tableSize = data.Length / 2 - 1;
        oslList.player = new Player[tableSize];
        clone = new PlayerObj[tableSize];

        for(int i = 0; i < tableSize; i++)
        {
            oslList.player[i] = new Player();
            oslList.player[i].name = data[2 * (i + 1)];
            oslList.player[i].role = data[2 * (i + 1) + 1];
            clone[i] = ScriptableObject.CreateInstance<PlayerObj>();
            clone[i].playerName = oslList.player[i].name;
            clone[i].playerPos = oslList.player[i].role;
        }
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttoncreate : MonoBehaviour
{
    public PlayerButtons pb;
    private Button b;
    // Start is called before the first frame update

    void Start()
    {
        pb = this.transform.parent.GetComponent<PlayerButtons>();
        b = this.GetComponent<Button>();
        b.onClick.AddListener(pb.DraftMe);
        b.onClick.AddListener(pb.NextTeam);
    }

   
}

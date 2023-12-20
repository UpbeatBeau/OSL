using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSort : MonoBehaviour
{
   public static int Comparison(CSVread.Player p1, CSVread.Player p2)
    {
        return p1.name.CompareTo(p2.name);
    }
}

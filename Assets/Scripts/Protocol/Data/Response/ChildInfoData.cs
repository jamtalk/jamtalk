using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildInfoData
{
    public string mem_id;
    public string name;
    public int key;
    public string jumin;
    public int level;
    public int display;
    public string created_at;
    public int character_pick = 0;
    public int point;
    public int[] awards = new int[0];
    public char gender;
    public int day;
    public int alphabet;
    public int contents_title;
    public bool isDislplay => Convert.ToBoolean(display);
    public DateTime RegistedDate => DateParser.Parse(created_at);
}
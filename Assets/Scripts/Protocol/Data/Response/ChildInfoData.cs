using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildInfoData
{
    public string name;
    public string jumin;
    public int display;
    public string created_at;
    
    public string birth => jumin;
    public bool isDislplay => Convert.ToBoolean(display);
    public DateTime RegistedDate => DateParser.Parse(created_at);
}
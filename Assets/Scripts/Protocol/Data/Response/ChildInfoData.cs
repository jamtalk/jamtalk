using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildInfoData
{
    public string mem_id;
    public string name;
    public string child_key;
    public string key;
    public string jumin;
    public int level;
    public int display;
    public string created_at;
    public int character_pick = 0;
    public int point;
    public int[] awards = new int[0];
    public string gender;
    public int day;
    public int alphabet;
    public string contents_title;
    public eContents GetContents()
    {
        if (string.IsNullOrEmpty(contents_title))
            return eContents.JT_PL1_102;

        return (eContents)Enum.Parse(typeof(eContents), contents_title);
    }
    public bool isDislplay => Convert.ToBoolean(display);
    public DateTime RegistedDate => DateParser.Parse(created_at);
    public int age => DateTime.Now.Year - int.Parse(jumin.ToString().Substring(0, 4));
    public bool Selected
    {
        get => PlayerPrefs.HasKey("CHILD") && PlayerPrefs.GetString("CHILD") == child_key;
        set
        {
            PlayerPrefs.SetString("CHILD", child_key);
            PlayerPrefs.Save();
        }
    }
}
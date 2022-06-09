using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(LocalDBElement),true)]
public class LocalDBEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var target = (LocalDBElement)serializedObject.targetObject;
        if (target.Loadable)
        {
            if (GUILayout.Button("Load"))
            {
                var path = EditorUtility.OpenFilePanel("Select original data", Application.dataPath, "csv");
                if (!string.IsNullOrEmpty(path))
                {
                    target.Load(ParsingData(path));
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
        base.OnInspectorGUI();
    }
    private List<Hashtable> ParsingData(string path)
    {
        var data = new List<Hashtable>();
        using (var sr = new StreamReader(path))
        {
            var keys = sr.ReadLine().Split(',');
            Debug.Log(string.Join("\n", keys));
            var line = sr.ReadLine();
            while (!string.IsNullOrEmpty(line)) 
            {
                var table = new Hashtable();
                var values = line.Split(',');
                for (int i = 0; i < keys.Length; i++)
                    table.Add(keys[i], values[i]);
                data.Add(table);
                line = sr.ReadLine();
            }
        }
        return data;
    }
}
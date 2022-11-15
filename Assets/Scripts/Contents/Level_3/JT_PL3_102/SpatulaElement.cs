using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatulaElement : MonoBehaviour
{
    [HideInInspector]
    public bool isGuide = true;

    private void Update()
    {
        if (!isGuide)
        {
            transform.position = GameManager.Instance.GetMousePosition();
        }
    }
}

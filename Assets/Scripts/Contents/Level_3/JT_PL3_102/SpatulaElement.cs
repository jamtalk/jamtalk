using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatulaElement : MonoBehaviour
{
    private void Update()
    {
        transform.position = GameManager.Instance.GetMousePosition();
    }
}

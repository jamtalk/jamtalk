using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DropField<T> : MonoBehaviour where T : MonoBehaviour
{
    public T Parent;
}
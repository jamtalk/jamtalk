using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public abstract class Pooling<T> : MonoBehaviour
    where T : PoolingElement
{
    public List<PoolingElement> pool = new List<PoolingElement>();
    public virtual T GetObject(T orizinal, Transform parent)
    {
        var targets = pool.Where(x => !x.gameObject.activeSelf);
        T result;
        if (targets.Count() > 0)
        {
            result = targets.First().GetComponent<T>();
            result.transform.SetParent(parent);
        }
        else
        {
            result = Instantiate(orizinal, parent);
            result.name = orizinal.name;
            pool.Add(result);
        }
        result.gameObject.SetActive(true);
        SetElement(result);

        return result;
    }

    public abstract void SetElement(T element);
}

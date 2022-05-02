using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolation
{
    public static Vector2[] GetInterpolation(Vector2 current, Vector2 target, float pixel)
    {
        var mousePos = target;
        var direction = (mousePos - current).normalized * pixel;
        var targetPos = current;
        var dis = Vector2.Distance(targetPos, mousePos);
        var count = 0;
        var list = new List<Vector2>();
        //while (dis > pixel)
        //{
        //    Debug.Log(dis);
        //    targetPos += direction;
        //    count += 1;
        //    dis = Vector2.Distance(targetPos, mousePos);
        //    list.Add(targetPos);
        //}
        for (var pos = current + direction; Vector2.Distance(pos, target) > pixel; pos += direction)
        {
            list.Add(direction);
        }
        return list.ToArray();
    }
}

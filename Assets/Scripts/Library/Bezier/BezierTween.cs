using UnityEngine;
using DG.Tweening;
namespace GJGameLibrary.Util.Bezier.DoTween
{
    public class BezierTween : Bezier
    {
        public static Sequence Curve(Transform target,float duration, int count, params Vector3[] points)
        {
            var pos = Curve(count, points);
            var seq = DOTween.Sequence();
            duration /= pos.Length;
            for(int i = 0;i <pos.Length; i++)
            {
                var tween = target.DOMove(pos[i], duration);
                tween.SetEase(Ease.Linear);
                seq.Append(tween);
            }
            return seq;
        }
    }
}

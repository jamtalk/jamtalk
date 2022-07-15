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
            target.position = pos[0];
            float dis = 0f;
            for (int i = 0; i < pos.Length; i++)
            {
                if (i == 0)
                    dis += Vector3.Distance(target.position, pos[i]);
                else
                    dis += Vector3.Distance(pos[i-1], pos[i]);
            }

            for(int i = 0;i <pos.Length; i++)
            {
                float _duration = 0f;
                float _dis;
                if(i==0)
                {
                    _dis = Vector3.Distance(target.position, pos[i]);
                }
                else
                {
                    _dis = Vector3.Distance(pos[i - 1], pos[i]);
                }
                _duration = _dis / dis * duration;
                var tween = target.DOMove(pos[i], _duration);
                tween.SetEase(Ease.Linear);
                seq.Append(tween);
            }
            return seq;
        }
    }
}

using UnityEngine;

public abstract class AnimationScript : MonoBehaviour
{
    public bool playOnAwake;
    [SerializeField]
    [Range(1f, 100f)]
    protected float speed;
    public virtual float duration => 100 / speed;
    public abstract void Play();
    public abstract void Stop();
    protected virtual void Awake()
    {
        if (playOnAwake)
            Play();
    }
    protected virtual void OnDestroy()
    {
        Stop();
    }
}

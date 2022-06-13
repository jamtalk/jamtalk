using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSources : MonoBehaviour
{
    public ParticleSystem particle;
    public void Play()
    {
        particle.Play();
    }
}

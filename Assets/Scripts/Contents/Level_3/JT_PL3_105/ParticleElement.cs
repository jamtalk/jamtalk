using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleElement : MonoBehaviour
{
    public ParticleSystem particle;
    public Image image;

    private void OnEnable()
    {
        particle.Play();
    }
}

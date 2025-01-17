using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehavior to be accessed by animation events to play a given audio clip on an audio source.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private bool doRandomizePitch = false;
    private AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlaySFX(AudioClip clip)
    {
        if (doRandomizePitch)
            source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(clip);
    }
}

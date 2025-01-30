using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for playing one-shot sounds from game objects that shouldn't have their own AudioSource. <br />
/// (Basically all prefabs that can be destroyed, such as projectiles, minions, and obstacles.)
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SFXPool : MonoBehaviour
{
    [SerializeField] private AudioSource miscAudioSource;
    [SerializeField] private AudioSource uiAudioSource;
    // Sources for other mix groups will go here.

    public static SFXPool instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogWarning("Multiple SFXPool instances detected. Destroying " + this);
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public enum MixGroup //For mixing, won't be used until the end of the audio production cycle.
    {
        misc,
        ui
    }
    /// <summary>
    /// Plays a given sound effect on an AudioSource assigned to no Mix Group.
    /// </summary>
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            miscAudioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Plays a given sound effect on a source assigned to a given Mix Group.
    /// </summary>
    public void PlaySound(AudioClip clip, MixGroup mixGroup)
    {
        switch(mixGroup)
        {
            case MixGroup.misc:
                miscAudioSource.PlayOneShot(clip);
                break;
            case MixGroup.ui:
                uiAudioSource.PlayOneShot(clip);
                break;
            default:
                Debug.LogWarning("Unknown mix group assigned to " + clip);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip takeDamageClip;

    public void PlayTakeDamageClip()
    {
        audioSource.PlayOneShot(takeDamageClip);
    }
}

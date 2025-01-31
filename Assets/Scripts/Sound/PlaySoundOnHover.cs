using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnHover : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioClip sfx;
    [SerializeField] private AudioSource source;
    public void OnPointerEnter(PointerEventData eventData)
    {
        source.PlayOneShot(sfx);
    }
}

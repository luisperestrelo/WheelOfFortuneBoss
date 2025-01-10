using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnStart : MonoBehaviour
{
    [SerializeField]
    private SoundProfile profile;
    private void Start()
    {
        MusicPlayer.instance.LoadProfile(profile);
        StartCoroutine(MusicPlayer.instance.StartSection(MusicPlayer.MusicSection.fight));
    }
}
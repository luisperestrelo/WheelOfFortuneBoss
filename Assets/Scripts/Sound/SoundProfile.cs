using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound Profile")]
public class SoundProfile : ScriptableObject
{
    public AudioClip ambienceLoop;

    [Space]
    [Header("Pre Fight Music")]
    public AudioClip preFightLoop;
    public float preFightLoopStartTime;
    public float preFightLoopEndTime;

    [Space]
    [Header("Fight Music")]
    public AudioClip fightLoop;
    public float fightLoopStartTime;
    public float fightLoopEndTime;

    [Space]
    public AudioClip fightEnd;

    [Space]
    [Header("Profile data")]
    public int bpm;
}

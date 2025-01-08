using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioLowPassFilter))]
public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource preFightSource,
        fightSource,
        ambienceSource;

    [SerializeField]
    private AnimationCurve freqRecoveryCurve;

    private AudioLowPassFilter filter;

    private int preFightLoopStartSamples;
    private int preFightLoopEndSamples;
    private int preFightLoopLengthSamples;

    private int fightLoopStartSamples;
    private int fightLoopEndSamples;
    private int fightLoopLengthSamples;

    private SoundProfile loadedProfile;

    [SerializeField]
    private float musicVolume = 0.5f;

    /// <summary>Set to true for one frame at the start of each measure.</summary>
    private bool measureFlag = false;

    public enum MusicSection
    {
        ambience,
        prefight,
        fight,
        endFight
    }

    public static MusicPlayer instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple Music Player instances detected. Destroying " + this);
            Destroy(gameObject);
        } else
            instance = this;

        filter = GetComponent<AudioLowPassFilter>();
    }

    public void LoadProfile(SoundProfile profile)
    {
        //There is a world where this code is readable
        //Alas, such dreams are of a man asleep.

        loadedProfile = profile;

        preFightSource.clip = profile.preFightLoop;
        //Convert loop start and end timestamps of fight music from seconds to samples.
        preFightLoopStartSamples = (int)(profile.preFightLoopStartTime * preFightSource.clip.frequency);
        preFightLoopEndSamples = (int)(profile.preFightLoopEndTime * preFightSource.clip.frequency);
        preFightLoopLengthSamples = preFightLoopEndSamples - preFightLoopStartSamples;

        fightSource.clip = profile.fightLoop;
        //Same thing for the pre-fight music.
        fightLoopStartSamples = (int)(profile.fightLoopStartTime * fightSource.clip.frequency);
        fightLoopEndSamples = (int)(profile.fightLoopEndTime * fightSource.clip.frequency);
        fightLoopLengthSamples = fightLoopEndSamples - fightLoopStartSamples;

        //Reset the measure cycle to realign with the BPM of the new music profile
        StopCoroutine(MeasureFlagCycle());
        StartCoroutine(MeasureFlagCycle());
    }

    /// <summary>
    /// Crossfades into a new section of the music profile. Should be called only after loading a music profile.
    /// </summary>
    /// <param name="section">The section of the music to crossfade into.</param>
    public IEnumerator StartSection(MusicSection section)
    {
        yield return new WaitUntil(() => measureFlag == true);
        switch(section)
        {
            case MusicSection.prefight:
                Debug.Log("Playing pre fight");
                preFightSource.Play();
                StartCoroutine(FadeSourceVolumeRoutine(source: preFightSource, targetVolume: 1, time: 3));
                StartCoroutine(FadeSourceVolumeRoutine(source: fightSource, targetVolume: 0, time: 1.5f));
                StartCoroutine(FadeSourceVolumeRoutine(source: ambienceSource, targetVolume: 0, time: 3f));
                break;
            case MusicSection.fight:
                fightSource.Play();
                StartCoroutine(FadeSourceVolumeRoutine(source: preFightSource, targetVolume: 0, time: loadedProfile.fightLoopStartTime));
                StartCoroutine(FadeSourceVolumeRoutine(source: fightSource, targetVolume: 1, time: loadedProfile.fightLoopStartTime));
                StartCoroutine(FadeSourceVolumeRoutine(source: ambienceSource, targetVolume: 0, time: loadedProfile.fightLoopStartTime));
                break;
            default:
                Debug.LogWarning("Unknown music section loaded.");
                break;
        }
    }

    private IEnumerator MeasureFlagCycle()
    {
        while (loadedProfile != null)
        {
            StartCoroutine(SetMeasureFlag());
            yield return new WaitForSeconds((60f / loadedProfile.bpm) * 4f);
        }
    }

    //This is its own coroutine so that the cycle isn't offset by one frame every time it runs.
    private IEnumerator SetMeasureFlag()
    {
        measureFlag = true;
        Debug.Log(measureFlag);
        yield return null;
        measureFlag = false;
    }

    /// <param name="source">The audio source to fade</param>
    /// <param name="targetVolume">The relative volume to fade to (0 for muted, 1 for normal music volume as determined by settings)</param>
    /// <param name="time">The time over which to fade from the current volume to the target volume</param>
    private IEnumerator FadeSourceVolumeRoutine(AudioSource source, float targetVolume, float time)
    {
        if (source.volume == targetVolume) yield break;

        float originalVolume = source.volume;
        float timeElapsed = 0;
        while(timeElapsed < time)
        {
            timeElapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(originalVolume, targetVolume * musicVolume, timeElapsed / time);
            yield return null;
        }
    }

    /// <summary>
    /// Applies a resonant low-pass filter to the music (for when the player takes damage)
    /// </summary>
    /// <param name="intensity">How intense the filter should be (0 does nothing, 1 is the max)</param>
    public void FilterMusic(float intensity)
    {
        const float maxFreq = 10000f;
        float freq = maxFreq - (intensity * maxFreq);
        float time = intensity * 2f;
        StartCoroutine(FilterMusicRoutine(freq, time));
    }

    private IEnumerator FilterMusicRoutine(float freq, float time)
    {
        float timeElapsed = 0;
        const float moveFreqDownTime = 0.1f;
        while(timeElapsed < moveFreqDownTime)
        {
            timeElapsed += Time.deltaTime;
            filter.cutoffFrequency = Mathf.Lerp(22000, freq, timeElapsed / moveFreqDownTime);
        }

        timeElapsed = 0;
        while(timeElapsed < time)
        {
            timeElapsed += Time.deltaTime;
            filter.cutoffFrequency = Mathf.Lerp(freq, 22000, freqRecoveryCurve.Evaluate(timeElapsed / time));
            yield return null;
        }
    }

    private void Update()
    {
        //If the pre-fight music has gone past the end of its loop point, rewind it by the duration of the loop.
        if (preFightSource.timeSamples >= preFightLoopEndSamples) { preFightSource.timeSamples -= preFightLoopLengthSamples;}

        //Same thing for the fight music
        if (fightSource.timeSamples >= fightLoopEndSamples) { fightSource.timeSamples -= fightLoopLengthSamples; }
    }
}

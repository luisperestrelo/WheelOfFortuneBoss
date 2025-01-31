using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioLowPassFilter))]
public class MusicPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource preFightSource,
        fightSource,
        ambienceSource;

    [SerializeField]
    private AnimationCurve freqRecoveryCurve;

    [SerializeField] private AudioLowPassFilter lpFilter;
    [SerializeField] private AudioHighPassFilter hpFilter;
    [SerializeField] private AudioMixer mixer;

    private int preFightLoopStartSamples;
    private int preFightLoopEndSamples;
    private int preFightLoopLengthSamples;

    private int fightLoopStartSamples;
    private int fightLoopEndSamples;
    private int fightLoopLengthSamples;

    private SoundProfile loadedProfile;

    [SerializeField]
    private float musicVolume = 0.5f;

    private Event onStartNewSection = new Event();

    [Tooltip("Set to true for one frame at the start of each measure.")]
    public bool measureFlag = false;
    private bool fadeOverrideFlag = false;

    [SerializeField] private AnimationCurve reverbFadeInCurve;

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

        DontDestroyOnLoad(gameObject);
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

        if (ambienceSource.clip != profile.ambienceLoop)
        {
            ambienceSource.clip = profile.ambienceLoop;
            ambienceSource.Play();
        }

        //Reset the measure cycle to realign with the BPM of the new music profile
        StopCoroutine(MeasureFlagCycle());
        StartCoroutine(MeasureFlagCycle());
        StartCoroutine(TransitionToNewSongRoutine(profile));
    }

    /// <summary>
    /// Crossfades into a new section of the music profile. Should be called only after loading a music profile. <br />
    /// </summary>
    /// <param name="section">The section of the music to crossfade into.</param>
    private IEnumerator StartSectionRoutine(MusicSection section)
    {
        yield return new WaitUntil(() => measureFlag == true);
        yield return SetFadeOverrideFlag();
        switch(section)
        {
            case MusicSection.prefight:
                preFightSource.Play();
                StartCoroutine(FadeSourceVolumeRoutine(source: preFightSource, targetVolume: 1, time: 3));
                StartCoroutine(FadeSourceVolumeRoutine(source: fightSource, targetVolume: 0, time: 1.5f));
                StartCoroutine(FadeSourceVolumeRoutine(source: ambienceSource, targetVolume: 0, time: 3f));
                break;
            case MusicSection.fight:
                fightSource.Play();
                StartCoroutine(FadeSourceVolumeRoutine(source: preFightSource, targetVolume: 0, time: 5));
                StartCoroutine(FadeSourceVolumeRoutine(source: fightSource, targetVolume: 1, time: 2));
                StartCoroutine(FadeSourceVolumeRoutine(source: ambienceSource, targetVolume: 0, time: 3));
                break;
            case MusicSection.ambience:
                StartCoroutine(FadeSourceVolumeRoutine(source: preFightSource, targetVolume: 0, time: 3));
                StartCoroutine(FadeSourceVolumeRoutine(source: fightSource, targetVolume: 0, time: 3));
                StartCoroutine(FadeSourceVolumeRoutine(source: ambienceSource, targetVolume: 1, time: 3));
                break;
            default:
                Debug.LogWarning("Unknown music section loaded.");
                break;
        }
    }

    private IEnumerator TransitionToNewSongRoutine(SoundProfile profile)
    {
        StartCoroutine(FadeInPrefightAfterDelay());
        float timeElapsed = 0;
        if (fightSource.volume > 0)
        {
            //fade out fight
            
            const float fadeOutTime = 3f;
            StartCoroutine(FadeSourceVolumeRoutine(fightSource, 0, fadeOutTime));
            while (timeElapsed < fadeOutTime)
            {
                timeElapsed += Time.deltaTime;
                SetReverbIntensity(Mathf.Lerp(0, 1, timeElapsed / fadeOutTime));
                yield return new WaitForSecondsRealtime(0);
            }
            fightSource.clip = profile.fightLoop;
            //Same thing for the pre-fight music.
            fightLoopStartSamples = (int)(profile.fightLoopStartTime * fightSource.clip.frequency);
            fightLoopEndSamples = (int)(profile.fightLoopEndTime * fightSource.clip.frequency);
            fightLoopLengthSamples = fightLoopEndSamples - fightLoopStartSamples;
            fightSource.Stop();
            fightSource.volume = 1;
        }
    }

    //This is its own method so that it doesnt have to be activated in the middle of a while loop
    private IEnumerator FadeInPrefightAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2);
        float timeElapsed = 0;
        const float fadeInTime = 5;
        preFightSource.Play();
        while (timeElapsed < fadeInTime)
        {
            timeElapsed += Time.deltaTime;
            SetReverbIntensity(Mathf.Lerp(1, 0, timeElapsed / fadeInTime));
            preFightSource.volume = Mathf.Lerp(0, 1, timeElapsed / fadeInTime);
            yield return new WaitForSecondsRealtime(0);
        }
    }

    /// <summary>
    /// Crossfades into a new section of the music profile. Should be called only after loading a music profile.
    /// </summary>
    /// <param name="section">The section of the music to crossfade into.</param>
    public void StartSection(MusicSection section)
    {
        StartCoroutine(StartSectionRoutine(section));
    }

    private IEnumerator MeasureFlagCycle()
    {
        while (loadedProfile != null)
        {
            StartCoroutine(SetMeasureFlag());
            yield return new WaitForSecondsRealtime((60f / loadedProfile.bpm) * 4f);
        }
    }

    //This is its own coroutine so that the cycle isn't offset by one frame every time it runs.
    private IEnumerator SetMeasureFlag()
    {
        measureFlag = true;
        yield return new WaitForSecondsRealtime(0);
        measureFlag = false;
    }

    private IEnumerator SetFadeOverrideFlag()
    {
        fadeOverrideFlag = true;
        yield return new WaitForSecondsRealtime(0);
        fadeOverrideFlag = false;
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

            if (fadeOverrideFlag)
                break;

            yield return new WaitForSecondsRealtime(0);
        }
    }

    /// <summary>
    /// Snaps the reverb mix on the music insert to a given percentage.
    /// </summary>
    /// <param name="intensity">0 is no reverb, 1 is 100% reverb.</param>
    private void SetReverbIntensity(float intensity)
    {
        mixer.SetFloat("MusicReverbMix", Mathf.Lerp(-80, 0, reverbFadeInCurve.Evaluate(intensity)));
    }

    #region Filter
    /// <summary>
    /// Applies a resonant low-pass and high-pass filter to the music temporarily (for when the player takes damage)
    /// </summary>
    /// <param name="intensity">How intense the filter should be (0 does nothing, 1 is the max)</param>
    public void FilterMusic(float intensity)
    {
        const float maxFreq = 10000f;
        float freq = maxFreq - (intensity * maxFreq);
        float time = intensity * 2f;
        StartCoroutine(TempLowPassMusicRoutine(freq, time));
        StartCoroutine(TempHighPassMusicRoutine(freq, time));
    }

    /// <summary>
    /// Sets the frequency cutoff of a low-pass and high-pass filter to a given intensity.
    /// </summary>
    /// <param name="intensity">How intense the filter should be (0 does nothing, 1 filters the music to nothing.</param>
    public void SetFilterIntensity(float intensity)
    {
        float cutoffAtMaxIntensity = 1000;
        const float time = 0.2f;
        StartCoroutine(MoveLowPassToFreqRoutine(Mathf.Lerp(22000, cutoffAtMaxIntensity, intensity), time));
        StartCoroutine(MoveHighPassToFreqRoutine(Mathf.Lerp(0, cutoffAtMaxIntensity, intensity), time));
    }

    //Low Pass and High Pass filters share no parent class, so we can't just pass in which filter we want.
    //So now we have two methods that are basically identical controlled by two other methods that are basically identical.
    //I am all ears for better solutions.

    /// <summary>
    /// Temporarily low-passes the music at a given frequency for a given amount of time.
    /// </summary>
    private IEnumerator TempLowPassMusicRoutine(float freq, float time)
    {
        yield return MoveLowPassToFreqRoutine(freq, 0.1f);
        yield return MoveLowPassToFreqRoutine(22000, time-0.1f);
    }

    /// <summary>
    /// Temporarily high-passes the music at a given frequency for a given amount of time.
    /// </summary>
    private IEnumerator TempHighPassMusicRoutine(float freq, float time)
    {
        yield return MoveHighPassToFreqRoutine(freq, 0.1f);
        yield return MoveHighPassToFreqRoutine(0, time - 0.1f);
    }

    private IEnumerator MoveLowPassToFreqRoutine(float freq, float time)
    {
        float startFreq = lpFilter.cutoffFrequency;
        float timeElapsed = 0;
        while (timeElapsed < time)
        {
            timeElapsed += Time.unscaledDeltaTime;
            lpFilter.cutoffFrequency = Mathf.Lerp(startFreq, freq, timeElapsed / time);
            yield return new WaitForSecondsRealtime(0);
        }
    }
    private IEnumerator MoveHighPassToFreqRoutine(float freq, float time)
    {
        float startFreq = hpFilter.cutoffFrequency;
        float timeElapsed = 0;
        while (timeElapsed < time)
        {
            timeElapsed += Time.unscaledDeltaTime;
            hpFilter.cutoffFrequency = Mathf.Lerp(startFreq, freq, timeElapsed / time);
            yield return new WaitForSecondsRealtime(0);
        }
    }
    #endregion

    private void Update()
    {
        //Same thing for the fight music
        float correctedTimeSamples = fightSource.timeSamples * (44100 / AudioSettings.outputSampleRate);
        if (correctedTimeSamples >= fightLoopEndSamples) { fightSource.timeSamples -= fightLoopLengthSamples; }
    }
}

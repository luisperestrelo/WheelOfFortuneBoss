using UnityEngine;
using UnityEngine.Audio;

public abstract class FieldEffectHandler : MonoBehaviour
{
    public Field FieldData { get; private set; }
    protected WheelSegment Segment { get; private set; }
    protected PlayerStats playerStats;
    protected AudioSource audioSource;
    //protected SkillStats skillStats;

    public virtual void Initialize(Field fieldData)
    {
        FieldData = fieldData;
        playerStats = FindObjectOfType<PlayerStats>();
        audioSource = CreateAudioSource(false);
    }

    public abstract void OnEnter(Player player);
    public abstract void OnStay(Player player, float deltaTime);
    public abstract void OnExit(Player player);
    public virtual void SetSegment(WheelSegment segment)
    {
        Segment = segment;
    }

    protected AudioSource CreateAudioSource(bool isChargeSource)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        AudioMixer mixer = Resources.Load("Master Mixer") as AudioMixer;
        if (isChargeSource)
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("Field Charge")[0];
        else
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("Wheel")[0];
        return source;
    }
} 
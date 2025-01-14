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
        audioSource = gameObject.AddComponent<AudioSource>();
        AudioMixer mixer = Resources.Load("Master Mixer") as AudioMixer;
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Wheel")[0];
    }

    public abstract void OnEnter(Player player);
    public abstract void OnStay(Player player, float deltaTime);
    public abstract void OnExit(Player player);
    public virtual void SetSegment(WheelSegment segment)
    {
        Segment = segment;
    }
} 
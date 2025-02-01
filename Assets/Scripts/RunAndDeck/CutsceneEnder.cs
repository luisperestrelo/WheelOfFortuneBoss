using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnder : MonoBehaviour
{
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private AudioClip endSfx;
    private float fadeTime = 1;
    public void EndCutscene()
    {
        SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.LoadScene("CardSelectionScene"));
        StartCoroutine(FadeOutAudioSourceRoutine(ambienceSource));
        StartCoroutine(FadeOutAudioSourceRoutine(voiceSource));
        SFXPool.instance.PlaySound(endSfx, SFXPool.MixGroup.ui);
    }

    private IEnumerator FadeOutAudioSourceRoutine(AudioSource source)
    {
        float timeElapsed = 0;
        float originalVolume = source.volume;
        while(timeElapsed < fadeTime)
        {
            timeElapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(originalVolume, 0, timeElapsed / fadeTime);
            yield return null;
        }
    }
}

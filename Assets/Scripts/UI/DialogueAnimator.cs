using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueAnimator : MonoBehaviour
{
    [SerializeField] private CutsceneEnder ender;
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;

    [SerializeField] private float textFadeTime = 1.5f;
    [SerializeField] private float timeBetweenText = 1.5f;
    [SerializeField] private float clearTextTime = 1.2f;

    [SerializeField] private List<AudioClip> voiceSfx;

    [SerializeField] private AnimationCurve textFadeCurve;

    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(CutsceneRoutine());
    }
    private IEnumerator DisplayLine(TextMeshProUGUI text, string s)
    {
        float timeElapsed = 0;
        text.text = s;
        while (timeElapsed < textFadeTime)
        {
            timeElapsed += Time.deltaTime;
            text.alpha = Mathf.Lerp(0, 1, textFadeCurve.Evaluate(timeElapsed / textFadeTime));
            yield return null;
        }
    }

    private IEnumerator SetNewDialogue(string s1, string s2, AudioClip clip1, AudioClip clip2)
    {
        source.PlayOneShot(clip1);
        yield return DisplayLine(text1, s1);
        yield return new WaitForSeconds(timeBetweenText);

        source.PlayOneShot(clip2);
        yield return DisplayLine(text2, s2);
        yield return new WaitForSeconds(timeBetweenText * 1.5f);

        yield return ClearTextRoutine();
    }

    private IEnumerator CutsceneRoutine()
    {
        yield return new WaitForSeconds(2);
        yield return SetNewDialogue("Oh, little wisp...", "What has my wretched brother made of you?", voiceSfx[0], voiceSfx[1]);

        yield return SetNewDialogue("I am so sorry.", "This is not how the thread of your fate was spun.", voiceSfx[2], voiceSfx[1]);

        yield return SetNewDialogue("This can still be fixed.", "I will spin your fate anew.", voiceSfx[0], voiceSfx[1]);

        yield return SetNewDialogue("Let it be so; You shall go unto my two siblings.", "You shall slay them each.", voiceSfx[0], voiceSfx[1]);

        yield return SetNewDialogue("Know you may not turn away from your fate...", "You may not move freely until it is done.", voiceSfx[0], voiceSfx[1]);

        yield return SetNewDialogue("Go, now, little wisp...", "Reclaim your fate.", voiceSfx[0], voiceSfx[1]);
        yield return new WaitForSeconds(timeBetweenText / 2);

        ender.EndCutscene();
    }

    private IEnumerator ClearTextRoutine()
    {
        float timeElapsed = 0;
        while(timeElapsed < clearTextTime)
        {
            timeElapsed += Time.deltaTime;
            text1.alpha = Mathf.Lerp(1, 0, timeElapsed / clearTextTime);
            text2.alpha = Mathf.Lerp(1, 0, timeElapsed / clearTextTime);
            yield return null;
        }
    }
}

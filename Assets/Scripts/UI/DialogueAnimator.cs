using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueAnimator : MonoBehaviour
{
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

    private IEnumerator CutsceneRoutine()
    {
        yield return new WaitForSeconds(2);

        source.PlayOneShot(voiceSfx[0]);
        yield return DisplayLine(text1, "Oh, little wisp...");
        yield return new WaitForSeconds(timeBetweenText);

        source.PlayOneShot(voiceSfx[1]);
        yield return DisplayLine(text2, "What has my brother made of you?");
        yield return new WaitForSeconds(timeBetweenText * 1.5f);

        yield return ClearTextRoutine();
        yield return new WaitForSeconds(timeBetweenText / 2);

        //Ctrl + V Supremacy. (I did not feel like making a new method for this);
        source.PlayOneShot(voiceSfx[2]);
        yield return DisplayLine(text1, "I am so sorry.");
        yield return new WaitForSeconds(timeBetweenText);

        source.PlayOneShot(voiceSfx[0]);
        yield return DisplayLine(text2, "This is not how the thread of your fate was spun.");
        yield return new WaitForSeconds(timeBetweenText * 1.5f);
        yield return ClearTextRoutine();
        yield return new WaitForSeconds(timeBetweenText / 2);

        source.PlayOneShot(voiceSfx[1]);
        yield return DisplayLine(text1, "This can still be fixed.");
        yield return new WaitForSeconds(timeBetweenText);

        source.PlayOneShot(voiceSfx[2]);
        yield return DisplayLine(text2, "I will spin your fate anew.");
        yield return new WaitForSeconds(timeBetweenText * 1.5f);
        yield return ClearTextRoutine();
        yield return new WaitForSeconds(timeBetweenText / 2);

        yield return DisplayLine(text1, "Let it be so; You will go unto my siblings.");
        yield return new WaitForSeconds(timeBetweenText);
        yield return DisplayLine(text2, "You will slay them each.");
        yield return new WaitForSeconds(timeBetweenText * 1.5f);
        yield return ClearTextRoutine();
        yield return new WaitForSeconds(timeBetweenText / 2);

        yield return DisplayLine(text1, "You are now bound to your adversaries.");
        yield return new WaitForSeconds(timeBetweenText);
        yield return DisplayLine(text2, "I bestow unto you the runes of old.");
        yield return new WaitForSeconds(timeBetweenText * 1.5f);
        yield return ClearTextRoutine();
        yield return new WaitForSeconds(timeBetweenText / 2);

        yield return DisplayLine(text1, "Go now, little wisp.");
        yield return new WaitForSeconds(timeBetweenText);
        yield return ClearTextRoutine();
        yield return new WaitForSeconds(timeBetweenText / 2);
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

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
    private int dialogueIndex = 0;

    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
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
        Debug.Log("Showing dialogue ");
        source.PlayOneShot(clip1);
        yield return DisplayLine(text1, s1);
        yield return new WaitForSeconds(timeBetweenText);

        source.PlayOneShot(clip2);
        yield return DisplayLine(text2, s2);
        yield return new WaitForSeconds(timeBetweenText * 1.5f);

        yield return ClearTextRoutine();
    }

    public IEnumerator CutsceneRoutine()
    {
        yield return new WaitForSeconds(2);

        while(dialogueIndex < 4)
        {
            IterateDialogue();
            yield return new WaitForSeconds(9);
        }

        yield return new WaitForSeconds(timeBetweenText / 2);

        ender.EndCutscene();
    }

    public void IterateDialogue()
    {
        dialogueIndex++;
        switch (dialogueIndex)
        {
            case 1:
                StartCoroutine(SetNewDialogue("Hail, fallen mortal. I am Clotho, spinner of fate and life.", "I wove the threads that shaped the world.", voiceSfx[0], voiceSfx[1]));
                break;
            case 2:
                StartCoroutine(SetNewDialogue("You've met an untimely demise to my siblings, one that I did not spin.", "I will grant you life anew...", voiceSfx[2], voiceSfx[3]));
                break;
            case 3:
                StartCoroutine(SetNewDialogue("But first, to break your chains, you must slay my two siblings each.", "Go now to the heavens and reclaim your fate.", voiceSfx[1], voiceSfx[4]));
                break;
            case 4:
                ender.EndCutscene();
                break;
            default:
                Debug.LogWarning("Invalid dialogue index");
                break;
        }
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

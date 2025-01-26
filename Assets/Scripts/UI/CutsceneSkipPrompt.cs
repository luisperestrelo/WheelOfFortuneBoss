using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSkipPrompt : MonoBehaviour
{
    float displayTime = 0;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed = 0.5f;
    [SerializeField] private CutsceneEnder ender;

    // Update is called once per frame
    void Update()
    {
        if (displayTime <= 0)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, Time.deltaTime * fadeSpeed);
        }
        else
        {
            displayTime -= Time.deltaTime;
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1, Time.deltaTime * fadeSpeed);
            if (Input.GetKeyDown(KeyCode.Escape))
                ender.EndCutscene();
        }

        if (Input.anyKey)
        {
            displayTime = 2;
        }
    }
}

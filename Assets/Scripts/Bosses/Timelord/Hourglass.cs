using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hourglass : MonoBehaviour
{
    [SerializeField] private Color color = Color.white;
    [SerializeField] private float flipDuration = 3f;

    [SerializeField] private GameObject topMask;
    [SerializeField] private GameObject bottomMask;

    [SerializeField] private GameObject topSandSprite;
    [SerializeField] private GameObject bottomSandSprite;
    [SerializeField] private GameObject sandRaySprite;

    [SerializeField] private ParticleSystem emphasize;

    private Vector3 maxTopMaskScale;
    private Vector3 maxBottomMaskScale;
    private Vector3 maxSandRayScale;

    public Color defaultColor;

    private bool isRunning;

    void Start()
    {
        maxTopMaskScale = topMask.transform.localScale;
        maxBottomMaskScale = bottomMask.transform.localScale;
        maxSandRayScale = sandRaySprite.transform.localScale;

        topSandSprite.GetComponent<SpriteRenderer>().color = color;
        bottomSandSprite.GetComponent<SpriteRenderer>().color = color;
        sandRaySprite.GetComponent<SpriteRenderer>().color = color;

        

        SetProgress(1f);
    }

    public void SetDefaultValues()
    {
        maxTopMaskScale = topMask.transform.localScale;
        maxBottomMaskScale = bottomMask.transform.localScale;
        maxSandRayScale = sandRaySprite.transform.localScale;

        SetHourglassColor(defaultColor); // it's not exactly the same as it spawns, because the sand and the effects are different
                                        // but here we're setting them both to the same color. Just did this quickly can refine later



        SetProgress(1f);
    }

    /// <summary>
    /// Flips the hourglass and starts the timer afterward.
    /// </summary>
    /// <param name="seconds">Duration of the timer.</param>

    /// <param name="onComplete">Called after timer has completed.</param>
    /// <returns>True, if timer has successfully started. False, if timer is already running. </returns>
    public bool StartTimer(float seconds, Action onComplete = null)
    {
        if (isRunning)
            return false;

        StartCoroutine(StartProgress(seconds, onComplete));
        return true;
    }

    private IEnumerator StartProgress(float duration, Action onComplete)
    {
        isRunning = true;
        SetProgress(1f);

        yield return StartCoroutine(Flip());
        yield return StartCoroutine(ScaleSandRay());
        yield return StartCoroutine(FlowSand(duration));

        isRunning = false;
        onComplete?.Invoke();
    }
    public void SetProgress(float value)
    {
        topMask.transform.localScale = new Vector3(maxTopMaskScale.x, (1 - value) * maxTopMaskScale.y, maxTopMaskScale.z);
        bottomMask.transform.localScale = new Vector3(maxBottomMaskScale.x, value * maxBottomMaskScale.y, maxBottomMaskScale.z);
    }

    private void Reset()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
        SetProgress(0f);
    }

    private IEnumerator Flip()
    {
        var elapsedTime = 0f;
        while (elapsedTime < flipDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, (elapsedTime / flipDuration) * 180f);
            yield return null;
        }
        Reset();
    }

    private IEnumerator ScaleSandRay()
    {
        var duration = 1f;
        var elapsedTime = 0f;
        sandRaySprite.transform.localScale = new Vector3(maxSandRayScale.x, 0, maxSandRayScale.z);
        sandRaySprite.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            sandRaySprite.transform.localScale = new Vector3(maxSandRayScale.x, (elapsedTime / duration) * maxSandRayScale.y, maxSandRayScale.z);
            yield return null;
        }
        sandRaySprite.transform.localScale = new Vector3(maxSandRayScale.x, maxSandRayScale.y, maxSandRayScale.z);
        Reset();
    }

    private IEnumerator FlowSand(float duration)
    {
        var elapsedTime = 0f;
        sandRaySprite.gameObject.SetActive(true);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            SetProgress(elapsedTime / duration);
            yield return null;
        }

        sandRaySprite.gameObject.SetActive(false);
        SetProgress(1f);
    }

    public void SetHourglassColor(Color newColor)
    {
        color = newColor;

        if (topSandSprite) topSandSprite.GetComponent<SpriteRenderer>().color = color;
        if (bottomSandSprite) bottomSandSprite.GetComponent<SpriteRenderer>().color = color;
        if (sandRaySprite) sandRaySprite.GetComponent<SpriteRenderer>().color = color;

        if (emphasize)
        {
            var main = emphasize.main;
            main.startColor = color;
        }
    }
}

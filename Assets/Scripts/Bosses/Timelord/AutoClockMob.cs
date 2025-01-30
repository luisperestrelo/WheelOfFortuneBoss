using System;
using System.Collections;
using UnityEngine;

public class AutoClockMob : ClockMob
{
    [Header("Auto Clock Mob")]
    [SerializeField] private float durationUntilPunishment = 10f;


    [Header("Slow Zone")]
    [SerializeField] private GameObject slowZonePrefab;
    [SerializeField] private float slowZoneDuration = 15f;

    private void Start()
    {
        StartTimer(durationUntilPunishment, () =>
        {
            SpawnSlowZone();
        });
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

        OnComplete += onComplete;
        StartCoroutine(StartProgress(seconds));
        return true;
    }


    IEnumerator StartProgress(float duration)
    {
        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            var degree = -elapsedTime / duration * 360f;
            SetProgress(degree);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        OnComplete?.Invoke();
        //SpawnSlowZone();
        StartCoroutine(StartProgress(duration));
    }

    private void SetProgress(float value)
    {
        progressMat?.SetFloat("_Arc2", 350 + value);
        pointer.localRotation = Quaternion.Euler(pointer.localRotation.eulerAngles.x,
            pointer.localRotation.eulerAngles.y, value);
    }

    private void SpawnSlowZone()
    {
        GameObject slowZone = Instantiate(slowZonePrefab, transform.position, Quaternion.identity);
        Destroy(slowZone, 10f);
    }
}

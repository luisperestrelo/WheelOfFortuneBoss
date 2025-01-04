using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircularSweepAttack : MonoBehaviour
{
    [SerializeField] private GameObject rotatingLaserPrefab;
    [SerializeField] private float defaultLaserDuration = 5f;
    [SerializeField] private float defaultLaserRotationSpeed = 100f;
    [SerializeField] private float telegraphDuration = 1f;

    private List<GameObject> _activeLasers = new List<GameObject>();

    public void StartCircularSweep(float startAngle = 0f, bool isClockwise = true, float duration = -1f, float speed = -1f)
    {
        if (duration == -1f)
        {
            duration = defaultLaserDuration;
        }
        if (speed == -1f)
        {
            speed = defaultLaserRotationSpeed;
        }
        StartCoroutine(CircularSweepRoutine(startAngle, isClockwise, duration, speed));
    }

    private IEnumerator CircularSweepRoutine(float startAngle, bool isClockwise, float duration, float speed)
    {
        GameObject newLaser = SetupTelegraphPhase(startAngle, isClockwise, speed);

        yield return new WaitForSeconds(telegraphDuration);

        ActivateLaser(newLaser);

        StartCoroutine(StopCircularSweepAfterDelay(newLaser, duration));
    }

    private GameObject SetupTelegraphPhase(float startAngle, bool isClockwise, float speed)
    {
        GameObject newLaser = Instantiate(rotatingLaserPrefab, transform.position, Quaternion.identity, transform);
        newLaser.transform.Rotate(Vector3.forward, startAngle);

        SetLaserRotationSpeed(newLaser, speed, isClockwise);
        SetLaserAlpha(newLaser, 0.2f);
        DisableLaserCollider(newLaser);

        _activeLasers.Add(newLaser);

        return newLaser;
    }

    private void ActivateLaser(GameObject laser)
    {
        SetLaserAlpha(laser, 1f);
        EnableLaserCollider(laser);
    }

    private void SetLaserRotationSpeed(GameObject laser, float speed, bool isClockwise)
    {
        LaserRotation laserRotation = laser.GetComponent<LaserRotation>();
        laserRotation.SetRotationSpeed(speed);
        laserRotation.SetRotationDirection(isClockwise);
    }

    private void SetLaserAlpha(GameObject laser, float alpha)
    {
        SpriteRenderer laserSprite = laser.GetComponentInChildren<SpriteRenderer>();
        Color laserColor = laserSprite.color;
        laserColor.a = alpha;
        laserSprite.color = laserColor;
    }

    private void DisableLaserCollider(GameObject laser)
    {
        Collider2D laserCollider = laser.GetComponentInChildren<Collider2D>();
        laserCollider.enabled = false;
    }

    private void EnableLaserCollider(GameObject laser)
    {
        Collider2D laserCollider = laser.GetComponentInChildren<Collider2D>();
        laserCollider.enabled = true;
    }

    private IEnumerator StopCircularSweepAfterDelay(GameObject laser, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (laser != null)
        {
            _activeLasers.Remove(laser);
            Destroy(laser);
        }
    }

    // Method to change the rotation direction of an existing laser
    public void ChangeLaserRotationDirection(GameObject laser, bool isClockwise)
    {
        if (_activeLasers.Contains(laser))
        {
            LaserRotation laserRotation = laser.GetComponent<LaserRotation>();
            laserRotation.SetRotationDirection(isClockwise);
        }
    }

    public float GetDefaultLaserDuration()
    {
        return defaultLaserDuration;
    }

    public float GetDefaultLaserRotationSpeed()
    {
        return defaultLaserRotationSpeed;
    }
} 
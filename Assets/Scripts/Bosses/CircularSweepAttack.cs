using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircularSweepAttack : MonoBehaviour
{
    [SerializeField] private GameObject rotatingLaserPrefab;
    [SerializeField] private float laserDuration = 5f;
    [SerializeField] private float laserRotationSpeed = 100f;
    [SerializeField] private float telegraphDuration = 1f;

    private List<GameObject> _activeLasers = new List<GameObject>();

    public void StartCircularSweep(float startAngle = 0f, bool isClockwise = true)
    {
        StartCoroutine(CircularSweepRoutine(startAngle, isClockwise));
    }

    private IEnumerator CircularSweepRoutine(float startAngle, bool isClockwise)
    {
        GameObject newLaser = SetupTelegraphPhase(startAngle, isClockwise);

        yield return new WaitForSeconds(telegraphDuration);

        ActivateLaser(newLaser);

        StartCoroutine(StopCircularSweepAfterDelay(newLaser, laserDuration));
    }

    private GameObject SetupTelegraphPhase(float startAngle, bool isClockwise)
    {
        GameObject newLaser = Instantiate(rotatingLaserPrefab, transform.position, Quaternion.identity, transform);
        newLaser.transform.Rotate(Vector3.forward, startAngle);

        SetLaserRotationSpeed(newLaser, laserRotationSpeed, isClockwise);
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
} 
using UnityEngine;
using System.Collections.Generic;

public class Shockwave : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private int numberOfSegments = 50;
    [SerializeField] private List<int> gapSegments = new List<int>();
    [SerializeField] private float shockwaveExpansionSpeed = 5f;
    [SerializeField] private float shockwaveDamage = 10f;

    private void Start()
    {
        GenerateSegments();
        Destroy(gameObject, lifetime);
    }

    private void GenerateSegments()
    {
        float angleStep = 360f / numberOfSegments;

        for (int i = 0; i < numberOfSegments; i++)
        {
            if (gapSegments.Contains(i)) continue;

            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up; // Calculate direction based on angle

            GameObject segment = Instantiate(segmentPrefab, transform.position, Quaternion.identity);
            ShockwaveSegment segmentScript = segment.GetComponent<ShockwaveSegment>();
            if (segmentScript != null)
            {
                segmentScript.Initialize(shockwaveExpansionSpeed, direction, shockwaveDamage);
            }
        }
    }

    public void SetNumberOfSegments(int numSegments)
    {
        numberOfSegments = numSegments;
    }

    public void SetGapSegments(List<int> gaps)
    {
        gapSegments = gaps;
    }

    public void SetShockwaveExpansionSpeed(float speed)
    {
        shockwaveExpansionSpeed = speed;
    }

    public void SetShockwaveDamage(float damage)
    {
        shockwaveDamage = damage;
    }
} 
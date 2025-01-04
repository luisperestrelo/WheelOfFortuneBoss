using UnityEngine;
using System.Collections.Generic;

//Need to make this only damage the player once, rather than every segment applying damage.
public class Shockwave : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private int numberOfSegments = 50;
    [SerializeField] private List<int> gapSegments = new List<int>();
    [SerializeField] private float shockwaveExpansionSpeed = 5f;
    [SerializeField] private float shockwaveDamage = 10f;
    [SerializeField] private float startRadius = 0f;
    [SerializeField] private bool expandOutward = true;

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
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;
            //for startRadius = 0, this results in the segment being created at the center of the circle, as it should for expandOutward

            Vector3 segmentPosition = direction * startRadius;

            //
            GameObject segment = Instantiate(segmentPrefab, transform);
            segment.transform.localPosition = segmentPosition;

            ShockwaveSegment segmentScript = segment.GetComponent<ShockwaveSegment>();
            if (segmentScript != null)
            {
                Vector3 segmentDirection = expandOutward ? direction : -direction;
                segmentScript.Initialize(shockwaveExpansionSpeed, segmentDirection, shockwaveDamage, transform, expandOutward);
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

    public void SetStartRadius(float radius)
    {
        startRadius = radius;
    }

    public void SetExpandOutward(bool expand)
    {
        expandOutward = expand;
    }
}
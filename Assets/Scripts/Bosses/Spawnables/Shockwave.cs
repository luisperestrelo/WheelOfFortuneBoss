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

    private bool hasDealtDamage = false; // Since each segment has its own damage, we need a flag to track if damage has been dealt 

    // Constructor to initialize values
    public void Initialize(int numberOfSegments, float shockwaveExpansionSpeed, float shockwaveDamage, float startRadius, bool expandOutward, float lifetime, List<int> gapSegments)
    {
        this.numberOfSegments = numberOfSegments;
        this.shockwaveExpansionSpeed = shockwaveExpansionSpeed;
        this.shockwaveDamage = shockwaveDamage;
        this.startRadius = startRadius;
        this.expandOutward = expandOutward;
        this.lifetime = lifetime;
        this.gapSegments = gapSegments;
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

            //TODO: use an object pool, or have a pre-existing inactive object that we just activate    
            GameObject segment = Instantiate(segmentPrefab, transform);
            segment.transform.localPosition = segmentPosition;
            ShockwaveSegment segmentScript = segment.GetComponent<ShockwaveSegment>();

            if (segmentScript != null)
            {
                Vector3 segmentDirection = expandOutward ? direction : -direction;
                segmentScript.Initialize(shockwaveExpansionSpeed, segmentDirection, shockwaveDamage, this, expandOutward);
            }
        }
    }

    // Method to reset the damage flag (no usefulness for now, but might in the future (eg reusing same shockwave))
    public void ResetDamageFlag()
    {
        hasDealtDamage = false;
    }

    public bool HasDealtDamage()
    {
        return hasDealtDamage;
    }

    public void MarkAsDealtDamage()
    {
        hasDealtDamage = true;
    }
}
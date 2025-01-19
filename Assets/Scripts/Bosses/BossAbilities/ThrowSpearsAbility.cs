using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSpearsAbility : MonoBehaviour
{

    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private GameObject spearTelegraphPrefab;
    [SerializeField] private int numberOfSpears = 2;
    [SerializeField] private float telegraphDuration = 1f;
    [SerializeField] private float spearWidth = 0.5f;
    [SerializeField] private float spearLength = 5f;
    [SerializeField] private float spawnDistance = 15f;
    [SerializeField] private bool useRandomDirections = false;
    [SerializeField] private float minAngleBetweenSpears = 30f;
    [SerializeField] private float angleOffset = 0f;

    [SerializeField] private AudioClip indicatorSfx;

    [Header("Spear Stats")]
    [SerializeField] private float spearSpeed = 20f;
    [SerializeField] private float spearDamage = 15f;
    [SerializeField] private float spearLifeTime = 5f;

    
    public void ThrowSpears(int numSpears, bool randomDirections, float angleOffset)
    {
        this.numberOfSpears = numSpears;
        this.useRandomDirections = randomDirections;
        this.angleOffset = angleOffset;

        List<float> spearAngles = new List<float>();

        if (useRandomDirections)
        {
            spearAngles = GenerateRandomSpearAngles();
        }
        else
        {
            spearAngles = GenerateEvenlySpacedSpearAngles(angleOffset);
        }

        SFXPool.instance.PlaySound(indicatorSfx);

        foreach (float angle in spearAngles)
        {
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;
            Vector3 spawnPosition = transform.position + direction * spawnDistance;

            GameObject telegraph = Instantiate(spearTelegraphPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));

            // Start a coroutine to throw the spear after the telegraph duration
            //StartCoroutine(ThrowSpearAfterDelay(direction, angle, telegraphDuration, spawnPosition));
            
            //They spawn from the boss's center with this one, use the one above to make them spawn ahead of the boss a bit
            StartCoroutine(ThrowSpearAfterDelay(direction, angle, telegraphDuration, transform.position)); 
        }
    }

    private List<float> GenerateRandomSpearAngles()
    {
        List<float> angles = new List<float>();
        for (int i = 0; i < numberOfSpears; i++)
        {
            float randomAngle;
            bool tooClose;
            do
            {
                tooClose = false;
                randomAngle = Random.Range(0f, 360f);
                foreach (float existingAngle in angles)
                {
                    if (Mathf.Abs(Mathf.DeltaAngle(randomAngle, existingAngle)) < minAngleBetweenSpears)
                    {
                        tooClose = true;
                        break;
                    }
                }
            } while (tooClose);
            angles.Add(randomAngle);
        }
        return angles;
    }

    private List<float> GenerateEvenlySpacedSpearAngles(float angleOffset)
    {
        List<float> angles = new List<float>();
        float angleStep = 360f / numberOfSpears;
        for (int i = 0; i < numberOfSpears; i++)
        {
            angles.Add(i * angleStep + angleOffset);
        }
        return angles;
    }

    private IEnumerator ThrowSpearAfterDelay(Vector3 direction, float angle, float delay, Vector3 spawnPosition)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Throwing spear after delay of " + delay);

        GameObject spear = Instantiate(spearPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
        spear.transform.localScale = new Vector3(spearWidth, spearLength, 1f);

        Spear spearComponent = spear.GetComponent<Spear>();
        spearComponent.Initialize(spearSpeed, spearDamage, spearLifeTime);
    }


}

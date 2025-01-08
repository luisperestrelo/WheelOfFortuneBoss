using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentricShockwavesAttack : MonoBehaviour
{
    [SerializeField] private GameObject shockwavePrefab;
    [SerializeField] private float shockwaveInterval = 1f;
    [SerializeField] private int numberOfShockwaves = 3;
    [SerializeField] private float shockwaveExpansionSpeed = 5f;
    [SerializeField] private float shockwaveLifetime = 3f;
    [SerializeField] private float shockwaveDamage = 10f;
    [SerializeField] private int numberOfSegments = 360;
    [SerializeField] private bool useRandomGaps = true;
    [SerializeField] private int numberOfGaps = 2;
    [SerializeField] private int gapSize = 15;
    [SerializeField] private List<int> gapSegments = new List<int>() { 0, 1 };
    [SerializeField] private float startRadius = 0f;
    [SerializeField] private bool expandOutward = true;

    public void StartConcentricShockwaves(float startRadius = -1f, bool expandOutward = true)
    {
        if (startRadius == -1f)
        {
            startRadius = this.startRadius;
        }
        StartCoroutine(EmitShockwaves(startRadius, expandOutward));
    }

    private IEnumerator EmitShockwaves(float startRadius, bool expandOutward)
    {
        for (int i = 0; i < numberOfShockwaves; i++)
        {
            GameObject shockwave = Instantiate(shockwavePrefab, transform.position, Quaternion.identity);
            Shockwave shockwaveScript = shockwave.GetComponent<Shockwave>();

            shockwaveScript.SetNumberOfSegments(numberOfSegments);
            shockwaveScript.SetShockwaveExpansionSpeed(shockwaveExpansionSpeed);
            shockwaveScript.SetShockwaveDamage(shockwaveDamage);
            shockwaveScript.SetStartRadius(startRadius);
            shockwaveScript.SetExpandOutward(expandOutward);
            shockwaveScript.SetShockwaveLifetime(shockwaveLifetime);

            if (useRandomGaps)
            {
                List<int> randomGaps = GenerateRandomGaps();
                shockwaveScript.SetGapSegments(randomGaps);
            }
            else
            {
                shockwaveScript.SetGapSegments(gapSegments);
            }

            yield return new WaitForSeconds(shockwaveInterval);
        }
    }

    private List<int> GenerateRandomGaps()
    {
        List<int> randomGaps = new List<int>();
        for (int i = 0; i < numberOfGaps; i++)
        {
            int startSegment = Random.Range(0, numberOfSegments);
            for (int j = 0; j < gapSize; j++)
            {
                randomGaps.Add((startSegment + j) % numberOfSegments);
            }
        }
        return randomGaps;
    }

    public void SetShockwaveInterval(float interval)
    {
        shockwaveInterval = interval;
    }

    public void SetNumberOfShockwaves(int number)
    {
        numberOfShockwaves = number;
    }

    public void SetShockwaveExpansionSpeed(float speed)
    {
        shockwaveExpansionSpeed = speed;
    }

    public void SetShockwaveLifetime(float lifetime)
    {
        shockwaveLifetime = lifetime;
    }

    public void SetShockwaveDamage(float damage)
    {
        shockwaveDamage = damage;
    }

    public float GetDefaultShockwaveInterval()
    {
        return shockwaveInterval;
    }

    public int GetDefaultNumberOfShockwaves()
    {
        return numberOfShockwaves;
    }

    public float GetDefaultShockwaveExpansionSpeed()
    {
        return shockwaveExpansionSpeed;
    }

    public float GetDefaultShockwaveLifetime()
    {
        return shockwaveLifetime;
    }

    public float GetDefaultShockwaveDamage()
    {
        return shockwaveDamage;
    }

    public void SetNumberOfSegments(int numSegments)
    {
        numberOfSegments = numSegments;
    }

    public int GetDefaultNumberOfSegments()
    {
        return numberOfSegments;
    }

    public void SetGapSegments(List<int> gaps)
    {
        gapSegments = gaps;
    }

    public List<int> GetDefaultGapSegments()
    {
        return gapSegments;
    }

    public void SetUseRandomGaps(bool useRandom)
    {
        useRandomGaps = useRandom;
    }

    public bool GetUseRandomGaps()
    {
        return useRandomGaps;
    }

    public void SetNumberOfGaps(int numGaps)
    {
        numberOfGaps = numGaps;
    }

    public int GetDefaultNumberOfGaps()
    {
        return numberOfGaps;
    }

    public void SetGapSize(int size)
    {
        gapSize = size;
    }

    public int GetDefaultGapSize()
    {
        return gapSize;
    }

    public void SetStartRadius(float radius)
    {
        startRadius = radius;
    }

    public float GetDefaultStartRadius()
    {
        return startRadius;
    }

    public void SetExpandOutward(bool expand)
    {
        expandOutward = expand;
    }

    public bool GetExpandOutward()
    {
        return expandOutward;
    }
}
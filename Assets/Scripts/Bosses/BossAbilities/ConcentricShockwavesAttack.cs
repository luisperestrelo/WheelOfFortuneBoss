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

            List<int> gaps = useRandomGaps ? GenerateRandomGaps() : gapSegments;
            shockwaveScript.Initialize(numberOfSegments, shockwaveExpansionSpeed, shockwaveDamage, startRadius, expandOutward, shockwaveLifetime, gaps);

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

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictTheFutureAbility : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Hourglass hourglass;
    [SerializeField] private GameObject telegraph;
    [SerializeField] private GameObject telegraphFake;

    [SerializeField] private Vector3 spawnPosition;
    [Header("Visual & Damaging Effects")]
    [SerializeField] private GameObject realFutureDamageEffect;
    [SerializeField] private GameObject fakeFutureDamageEffect;

    [Header("Offsets & Radius")]
    [Tooltip("How far from the center do we want the effect to be")]
    [SerializeField] private float halfSliceRadius = 2f;
    [Tooltip("Offset for 'Real Future' effect, added on top of the radius-based positioning.")]
    [SerializeField] private Vector3 realDamageEffectOffset = Vector3.zero;
    [Tooltip("Offset for 'Fake Future' effect, added on top of the mirrored radius-based positioning.")]
    [SerializeField] private Vector3 fakeDamageEffectOffset = Vector3.zero;

    [Header("Future Colors")]
    [SerializeField] private Color realFutureColor = Color.blue;
    [SerializeField] private Color fakeFutureColor = Color.red;

    [Header("Telegraph Settings")]
    [SerializeField] private float telegraphDuration = 3f;


    private void Start()
    {
        spawnPosition = new Vector3(0, 2.49f, 0);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F4))
        {
            RealFuture();
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            FakeFuture();
        }
#endif
    }

    // ---------------------------------------------------------------
    //  TELEGRAPH SPAWNING METHODS
    // ---------------------------------------------------------------
    // Return the spawned telegraph so we can later destroy it.
    private GameObject SpawnTelegraphAtZ(float zRotation, bool isFake = false)
    {
        if (isFake)
        {
            if (telegraphFake == null) return null;
            return Instantiate(telegraphFake, spawnPosition, Quaternion.Euler(0f, 0f, zRotation));
        }
        else
        {
            if (telegraph == null) return null;

            return Instantiate(
                telegraph,
                spawnPosition,
                Quaternion.Euler(0f, 0f, zRotation)
            );
        }
    }



    private GameObject SpawnTelegraphRandomZ()
    {
        float randomZ = Random.Range(0f, 360f);
        return SpawnTelegraphAtZ(randomZ);
    }

    private GameObject SpawnTelegraph0Or180()
    {
        bool flip = Random.value < 0.5f;
        float zRotation = flip ? 0f : 180f;
        return SpawnTelegraphAtZ(zRotation);
    }

    // ---------------------------------------------------------------
    //  FUTURE SCENARIOS
    // ---------------------------------------------------------------
    //If we visit this later, the logic is a bit all over the place because it was a quick fix to the rework of this ability.
    // SO ignore the usage of names like "Fake" and such, this ability doesn't work like that anymore
    public void RealFuture()
    {
        hourglass.SetHourglassColor(realFutureColor);

        /* GameObject spawnedTelegraph = SpawnTelegraphRandomZ();
        GameObject spawnedTelegraphFake = SpawnTelegraphAtZ(spawnedTelegraph.transform.eulerAngles.z + 180f, true); */

        GameObject spawnedTelegraphFake = SpawnTelegraphRandomZ();
        GameObject spawnedTelegraph = SpawnTelegraphAtZ(spawnedTelegraphFake.transform.eulerAngles.z + 180f, true);


        // Hourglass runs for telegraphDuration. After that time, spawn effect & remove telegraph.
        hourglass.StartTimer(telegraphDuration, () =>
        {
            if (spawnedTelegraph != null)
            {

                float angleDeg = spawnedTelegraph.transform.eulerAngles.z;
                float bandaidOffset = 180f;
                angleDeg = angleDeg + bandaidOffset;

                // We "think in terms of a circle": center = spawnPosition, radius = halfSliceRadius at angleDeg.
                Vector3 effectCenter = GetCirclePosition(spawnPosition, halfSliceRadius, angleDeg)
                                       + realDamageEffectOffset;

                // Might want to make this not apply to the visuals, but we need to rotate the collider
                Quaternion effectRotation = spawnedTelegraph.transform.rotation;

                Instantiate(realFutureDamageEffect, effectCenter, effectRotation);
                //Instantiate(realFutureDamageEffect, effectCenter, Quaternion.identity);
                // The effect prefab handles the damage
                Destroy(spawnedTelegraph);
                Destroy(spawnedTelegraphFake);
            }

        });
    }

    public void FakeFuture()
    {
        hourglass.SetHourglassColor(fakeFutureColor);

/*         GameObject spawnedTelegraph = SpawnTelegraphRandomZ();
        GameObject spawnedTelegraphFake = SpawnTelegraphAtZ(spawnedTelegraph.transform.eulerAngles.z + 180f, true); */

        GameObject spawnedTelegraphFake = SpawnTelegraphRandomZ();
        GameObject spawnedTelegraph = SpawnTelegraphAtZ(spawnedTelegraphFake.transform.eulerAngles.z + 180f, true);


        // Hourglass runs for telegraphDuration. After that time, spawn effect & remove telegraph.
        hourglass.StartTimer(telegraphDuration, () =>
        {
            if (spawnedTelegraph != null)
            {
                // Grab the telegraph's Z rotation.
                float angleDeg = spawnedTelegraph.transform.eulerAngles.z;

                // "Fake" scenario: Mirror the angle by adding 180°, so the damage event is actually on the opposite side of the telegraph
                float bandaidOffset = 180f;
                float mirroredAngle = angleDeg + 180f + bandaidOffset;

                Vector3 effectCenter = GetCirclePosition(spawnPosition, halfSliceRadius, mirroredAngle)
                                       + fakeDamageEffectOffset;

                Quaternion effectRotation = Quaternion.Euler(0f, 0f, mirroredAngle - bandaidOffset);

                Instantiate(fakeFutureDamageEffect, effectCenter, effectRotation);

                //Instantiate(fakeFutureDamageEffect, effectCenter, Quaternion.identity);
                Destroy(spawnedTelegraph);
                Destroy(spawnedTelegraphFake);

                // The effect prefab handles the damage

            }

        });
    }


    private Vector3 GetCirclePosition(Vector3 center, float radius, float angleDeg)
    {
        float angleRad = angleDeg * Mathf.Deg2Rad;
        float x = center.x + radius * Mathf.Cos(angleRad);
        float y = center.y + radius * Mathf.Sin(angleRad);
        float z = center.z; // Keep the same Z as the center
        return new Vector3(x, y, z);
    }
}

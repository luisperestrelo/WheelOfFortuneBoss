using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepFatTentaclesAbility : MonoBehaviour
{
    [SerializeField] private Transform sweepCenter;
    [SerializeField] private float angularVelocity = 45f;
    [SerializeField] private float sweepDuration = 2f;
    [SerializeField] private float angleOffset = 180f;

    [SerializeField] private AudioClip sweepSfx;

    private bool isSweeping = false;
    private float elapsedTime = 0f;
    private List<GameObject> fatTentacles = new List<GameObject>();
    private Dictionary<GameObject, float> initialAngles = new Dictionary<GameObject, float>();

    private CircularPath circularPath;

    private void Start()
    {
        circularPath = FindObjectOfType<CircularPath>();
        //sweepCenter = circularPath.transform;
    }

    public void StartSweep()
    {
        isSweeping = true;
        elapsedTime = 0f;
        fatTentacles.Clear();
        initialAngles.Clear();
        bool hasPlayedSfx = false;

        foreach (GameObject obj in AbilityObjectManager.Instance.GetActiveAbilityObjects())
        {
            if (obj.GetComponentInChildren<FatTentacle>() != null)
            {
                //Only play the SFX if there is a tentacle that is moving, and only play one sfx.
                if (!hasPlayedSfx)
                    obj.GetComponentInChildren<SFXPlayer>().PlaySFX(sweepSfx);

                fatTentacles.Add(obj);
                Vector3 direction = obj.transform.position - sweepCenter.position;
                float initialAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Incorporate angleOffset
                initialAngle -= angleOffset;

                initialAngles.Add(obj, initialAngle);
            }
        }
    }

    private void Update()
    {
        if (isSweeping)
        {
            elapsedTime += Time.deltaTime;
            float currentSweepAngle = angularVelocity * elapsedTime;

            foreach (GameObject tentacle in fatTentacles)
            {
                if (tentacle == null) continue;

                float newAngle = initialAngles[tentacle] + currentSweepAngle;

                // Update the tentacle's position and rotation
                Vector3 newDirection = Quaternion.Euler(0, 0, newAngle) * Vector3.left;
                tentacle.transform.position = sweepCenter.position + newDirection * (tentacle.transform.position - sweepCenter.position).magnitude;
                tentacle.transform.rotation = Quaternion.Euler(0, 0, newAngle);

                // Update wall angles based on the new tentacle angle
                Wall wall = tentacle.GetComponentInChildren<Wall>();
                if (wall != null)
                {
                    // Calculate new wall angles based on the tentacle's forward direction
                    float tentacleAngle = newAngle + angleOffset;
                    wall.UpdateAngles(tentacleAngle - 20f, tentacleAngle + 20f);
                }
            }

            if (elapsedTime >= sweepDuration)
            {
                isSweeping = false;
            }
        }
    }
}
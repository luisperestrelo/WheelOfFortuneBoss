using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    [Tooltip("Time in seconds before this portal can be used again once triggered")]
    [SerializeField] private float teleportCooldown = 0.5f;

    [Tooltip("Reference to the other portal GameObject. They should always come in a pair")]
    [SerializeField] private Portal otherPortal;
    public float angle = 0f;

    private float lastTeleportTime = Mathf.NegativeInfinity;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //need a short cooldown so that the player doesn't get stuck in a loop
            if (Time.time >= lastTeleportTime + teleportCooldown)
            {
                other.transform.position = otherPortal.transform.position;
                other.GetComponent<PlayerSpinMovement>()?.SetCurrentAngle(otherPortal.angle);

                lastTeleportTime = Time.time;
                otherPortal.lastTeleportTime = Time.time;
            }
        }
    }
}

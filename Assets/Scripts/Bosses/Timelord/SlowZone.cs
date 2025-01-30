using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZone : MonoBehaviour
{
    [SerializeField] private float speedReductionFactor = 0.5f; // Adjust this to control how much the speed is reduced
    private PlayerSpinMovement playerMovement;
    private float originalMaxSpeed;

    //TODO: put references here
    void Start()
    {
        
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerMovement == null && other.GetComponent<PlayerSpinMovement>() != null)
        {
            playerMovement = other.GetComponent<PlayerSpinMovement>();
            originalMaxSpeed = playerMovement.MaxRotationSpeed; 
            playerMovement.SetMaxRotationSpeed(originalMaxSpeed * speedReductionFactor); 
            Debug.Log("Player entered Slow Zone. Speed reduced.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerSpinMovement>() == playerMovement)
        {
            playerMovement.SetMaxRotationSpeed(originalMaxSpeed); 
            playerMovement = null; 
            Debug.Log("Player exited Slow Zone. Speed restored.");
        }
    }
}

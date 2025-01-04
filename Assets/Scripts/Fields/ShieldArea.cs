using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldArea : WheelArea
{
    [Header("Shield Area Settings")]
    [SerializeField] private GameObject shieldPrefab;         
    [SerializeField] private GameObject onCooldownTextObject; 
    [SerializeField] private float cooldownDuration = 5f;     

    private bool _isOnCooldown = false;
    private float _cooldownTimer = 0f;

    protected override void Awake()
    {
        base.Awake();

        if (onCooldownTextObject != null)
        {
            onCooldownTextObject.SetActive(false);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (_isOnCooldown)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0f)
            {
                _isOnCooldown = false;
                if (onCooldownTextObject != null)
                {
                    onCooldownTextObject.SetActive(false);
                }
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (!other.CompareTag("Player")) return;

        // If we're on cooldown, no new shield
        if (_isOnCooldown)
        {
            return;
        }

        // Otherwise, give the player a shield
        PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
        if (playerCombat != null && !playerCombat.HasShield)
        {
            playerCombat.ActivateShield(shieldPrefab, this);
        }

        // Start cooldown immediately upon entering, could change in the future
        // as of now player only gains shield on entering, so if the cooldown expires while he is inside the area, he will not get a shield 
        _isOnCooldown = true;
        _cooldownTimer = cooldownDuration;

        if (onCooldownTextObject != null)
        {
            onCooldownTextObject.SetActive(true);
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);

        if (!other.CompareTag("Player")) return;

        // If the player leaves and still has this area’s shield, remove it
        PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
        if (playerCombat != null && playerCombat.HasShield)
        {
            playerCombat.RemoveShield();
        }
    }
}

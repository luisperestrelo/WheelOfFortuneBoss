using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeRift : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("Damage per second inflicted if the player remains in the rift.")]
    [SerializeField] private float damagePerSecond = 10f;

    [Tooltip("Interval in seconds between consecutive damage ticks while standing inside the rift.")]
    [SerializeField] private float damageInterval = 0.4f;



    private bool _isGrown = false;
    private bool _isPlayerInRift = false;
    private float _damageTimer = 0f;

    private Collider2D _collider2D;
    private PlayerHealth _playerHealth;

    private void Awake()
    {
        // Grab the collider and disable it until animation says we've fully grown.
        _collider2D = GetComponent<Collider2D>();
        if (_collider2D != null)
        {
            _collider2D.enabled = false;
        }
    }

    private void Start()
    {

        _playerHealth = FindObjectOfType<PlayerHealth>(); // fallback
    }

    private void Update()
    {
        // Only do damage if we've grown.
        if (!_isGrown) return;

        if (_isPlayerInRift)
        {
            _damageTimer += Time.deltaTime;
            if (_damageTimer >= damageInterval)
            {
                if (_playerHealth != null)
                {
                    // Damage over time
                    float damageAmount = damagePerSecond * damageInterval;
                    _playerHealth.TakeDamage(damageAmount, false);
                }
                _damageTimer = 0f;
            }
        }
    }

    /// <summary>
    /// Called from the animation event in TimeRiftAnimationEvents
    /// to enable collision (& thereby damage) once the TimeRift is "fully grown."
    /// </summary>
    public void EnableDamage()
    {
        _isGrown = true;
        if (_collider2D != null)
        {
            _collider2D.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isGrown) return; 

        if (other.CompareTag("Player"))
        {
            
            _playerHealth = other.GetComponent<PlayerHealth>();
            if (_playerHealth)
            {
                float immediateDamage = damagePerSecond * damageInterval;
                _playerHealth.TakeDamage(immediateDamage, isDamageOverTime: false);
            }

            _isPlayerInRift = true;
            _damageTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_isGrown) return; 

        if (other.CompareTag("Player"))
        {
            _isPlayerInRift = false;
            _damageTimer = 0f;
        }
    }
}

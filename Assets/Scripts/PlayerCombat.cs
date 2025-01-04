using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float defaultDamage = 10f;
    [SerializeField] private GameObject _fireballPrefab;
    

    private float currentDamage;
    private Coroutine damageCoroutine;

    public bool HasShield { get; private set; }
    private GameObject _activeShield;
    private ShieldArea _currentShieldArea;

    private void Start()
    {
        currentDamage = defaultDamage;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ShootFireBall();
        }
    }

    public void SetDamageMultiplierForDuration(float multiplier, float duration)
    {
        // If there's already a damage-coroutine running, stop it so durations don't overlap unpredictably.
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }

        damageCoroutine = StartCoroutine(DamageMultiplierCoroutine(multiplier, duration));
    }

    private IEnumerator DamageMultiplierCoroutine(float multiplier, float duration)
    {
        currentDamage = defaultDamage;
        currentDamage *= multiplier;       // Apply multiplier
        Debug.Log("Damage set to: " + currentDamage + " for " + duration + " seconds");
        yield return new WaitForSeconds(duration);  
        currentDamage = defaultDamage;     
        Debug.Log("Damage reset to: " + currentDamage);
        damageCoroutine = null;
    }

    public void ShootFireBall()
    {
        GameObject fireball = Instantiate(_fireballPrefab, transform.position, Quaternion.identity);
        fireball.GetComponent<Fireball>().SetDamage(currentDamage);
        fireball.GetComponent<Rigidbody2D>().velocity = transform.up * -10f;

        
    }

    public void ActivateShield(GameObject shieldPrefab, ShieldArea source)
    {
        if (shieldPrefab == null) return;

        HasShield = true;
        _currentShieldArea = source;
        _activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
    }

    public void RemoveShield()
    {
        HasShield = false;

        if (_activeShield != null)
        {
            Destroy(_activeShield);
            _activeShield = null;
        }

        _currentShieldArea = null;
    }
}

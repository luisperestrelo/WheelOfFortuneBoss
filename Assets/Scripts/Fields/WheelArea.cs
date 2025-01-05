using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WheelArea : MonoBehaviour, IPointerClickHandler
{
    protected bool _isOnFire = false;
    [SerializeField] private ParticleSystem _onFireParticles;

    [SerializeField] private SpriteRenderer _iconRend;
    public WheelEffect effect;
    protected virtual void Start()
    {
        _onFireParticles = GetComponentInChildren<ParticleSystem>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (WheelBuilder.effectBeingPlaced != null)
        {
            SetEffect(WheelBuilder.effectBeingPlaced);
            WheelBuilder.effectBeingPlaced = null;
        }
    }

    public void SetEffect(WheelEffect effect)
    {
        _iconRend.sprite = effect.icon;
        this.effect = effect;
    }

    /// <summary>
    /// Called each frame the player is in this area.
    /// </summary>
    public void OnUpdate(PlayerCombat player)
    {
        if (_isOnFire) player.health.TakeDamage(20 * Time.deltaTime);
        if (effect != null)
            effect.OnUpdate(player);
    }

    public void SetOnFire()
    {
        _onFireParticles.Play();
        _isOnFire = true;
    }

    public void StopOnFire()
    {
        _onFireParticles.Stop();
        _isOnFire = false;
    }
}

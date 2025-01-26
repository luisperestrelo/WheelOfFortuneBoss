using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(BuffManager))]
[RequireComponent(typeof(Stats))]
public class CritUpgrades : MonoBehaviour
{
    private PlayerCombat _playerCombat;
    private BuffManager _buffManager;
    private Stats _stats;

    // All the effects the player currently owns
    // Passive Upgrade Cards that give effects basically add to this List when the player selects them
    private List<IOnCritEffect> _onCritEffects = new List<IOnCritEffect>();
    public List<string> _onCritEffectNames = new List<string>(); //just so we can see it in the inspector

    private void Awake()
    {
        _playerCombat = GetComponent<PlayerCombat>();
        _buffManager = GetComponent<BuffManager>();
        _stats = GetComponent<Stats>();
    }

    private void OnEnable()
    {
        _playerCombat.OnCrit.AddListener(OnCritTriggered);
    }

    private void OnDisable()
    {
        _playerCombat.OnCrit.RemoveListener(OnCritTriggered);
    }

    private void OnCritTriggered()
    {
        // Something critted! Invoke all effects
        foreach(var effect in _onCritEffects)
        {
            effect.HandleCrit(_playerCombat, _buffManager, _stats);
        }
    }

    // Called when the player picks up a new effect, e.g. from an upgrade
    public void AddCritEffect(IOnCritEffect effect)
    {
        _onCritEffects.Add(effect);
        _onCritEffectNames.Add(effect.GetType().Name);
    }

    public void RemoveCritEffect(IOnCritEffect effect)
    {
        _onCritEffects.Remove(effect);
        _onCritEffectNames.Remove(effect.GetType().Name);
    }
} 
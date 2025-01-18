using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    [SerializeField] private RadialProgressBar chargeProgressBar;
    [SerializeField] private Color color;
    [SerializeField] private Sprite icon;
    [SerializeField] private RuneProgress inactiveProgress;
    [SerializeField] private RuneProgress activeProgress;
    private float _chargeProgress;

    public float ChargeProgress
    {
        get => _chargeProgress;
        set
        {
            _chargeProgress = value;
            chargeProgressBar.SetProgress(value);
        }
    }


    private float _cooldownProgress
    ;
    public float CooldownProgress
    {
        get { return _cooldownProgress; }
        set
        {
            _cooldownProgress = value;
            inactiveProgress.Progress = 1 - value;
        }
    }

    public float ActiveProgress
    {
        get { return activeProgress.Progress; }
        set { activeProgress.Progress = value; }
    }


}

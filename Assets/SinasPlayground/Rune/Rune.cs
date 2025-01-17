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
        set { 
            _chargeProgress = value; 
            chargeProgressBar.SetProgress(value); }
    }
    // Start is called before the first frame update

}

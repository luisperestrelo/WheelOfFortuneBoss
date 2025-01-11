using UnityEngine;

[CreateAssetMenu(fileName = "New Chargeable Field", menuName = "Fields/New Chargeable Field")]
public class ChargeableField : Field
{
    [SerializeField] private bool resetsOnExit;
    public bool ResetsOnExit => resetsOnExit;

    [SerializeField] private float decayRate;
    public float DecayRate => decayRate;

    [SerializeField] private float chargeTime;
    public float ChargeTime => chargeTime;
} 
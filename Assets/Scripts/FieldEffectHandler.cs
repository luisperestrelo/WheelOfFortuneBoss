using UnityEngine;

public abstract class FieldEffectHandler : MonoBehaviour
{
    public Field FieldData { get; private set; }
    protected WheelSegment Segment { get; private set; }

    public virtual void Initialize(Field fieldData)
    {
        FieldData = fieldData;
    }

    public abstract void OnEnter(Player player);
    public abstract void OnStay(Player player, float deltaTime);
    public abstract void OnExit(Player player);
    public virtual void SetSegment(WheelSegment segment)
    {
        Segment = segment;
    }
} 
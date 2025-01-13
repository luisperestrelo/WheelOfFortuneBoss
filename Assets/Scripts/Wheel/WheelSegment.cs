using UnityEngine;

public class WheelSegment
{
    public Field Field { get;  set; }
    public FieldEffectHandler EffectHandler { get;  set; }
    public float StartAngle { get;  set; }
    public float EndAngle { get;  set; }
    public bool IsActive { get; set; }
    public float CooldownTimer { get; set; }
    public bool IsOnCooldown { get; private set; }
    private float cooldownTimer;
    public TextMesh CooldownText { get; set; }

    public WheelSegment(Field field, float startAngle, float endAngle)
    {
        Field = field;
        StartAngle = startAngle;
        EndAngle = endAngle;
        IsActive = true;
        CooldownTimer = 0f;
        EffectHandler = FieldEffectHandlerFactory.CreateEffectHandler(field);
        if (EffectHandler != null)
        {
            EffectHandler.Initialize(field);
        }
        IsOnCooldown = false;
        cooldownTimer = 0f;
    }

    public void StartCooldown()
    {
        PlayerStats playerStats = Object.FindObjectOfType<PlayerStats>();

        if (Field.Cooldown > 0)
        {
            IsOnCooldown = true;
            cooldownTimer = Field.Cooldown * playerStats.FieldsCooldownMultiplier;
        }
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (IsOnCooldown)
        {
            cooldownTimer -= deltaTime;
            if (cooldownTimer <= 0f)
            {
                IsOnCooldown = false;
            }
        }
    }
} 
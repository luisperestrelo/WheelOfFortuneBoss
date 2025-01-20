using UnityEngine;

public class WheelSegment : MonoBehaviour
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
    public SpriteRenderer SegmentRenderer { get; private set; }
    public Collider2D SegmentCollider { get; private set; }
    public CircularPath CircularPath { get; private set; }

    public void Initialize(Field field, float startAngle, float endAngle)
    {
        Field = field;
        StartAngle = startAngle;
        EndAngle = endAngle;
        IsActive = true;
        CooldownTimer = 0f;
        IsOnCooldown = false;
        cooldownTimer = 0f;

        CircularPath = FindObjectOfType<CircularPath>();

        // Add a SpriteRenderer component for the segment's visual representation if it doesn't exist
        SegmentRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (SegmentRenderer == null)
        {
            SegmentRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        SegmentRenderer.sprite = Field.Icon; // Use the field's icon as the sprite
        SegmentRenderer.color = Field.Color;

        // Add a Collider2D component for interaction if it doesn't exist
        SegmentCollider = gameObject.GetComponent<PolygonCollider2D>();
        if (SegmentCollider == null)
        {
            SegmentCollider = gameObject.AddComponent<PolygonCollider2D>();
        }
        SegmentCollider.isTrigger = true;
        UpdateColliderShape();

        // Create and initialize the EffectHandler
        EffectHandler = FieldEffectHandlerFactory.CreateEffectHandler(field);
        if (EffectHandler != null)
        {
            EffectHandler.Initialize(field);
            EffectHandler.SetSegment(this);
        }
    }

    public void UpdateColliderShape()
    {
        if (SegmentCollider is PolygonCollider2D polygonCollider)
        {
            // Define the vertices of the pizza slice collider
            Vector2[] points = new Vector2[4];
            points[0] = CircularPath.GetCenter(); // Center of the wheel
            points[1] = Quaternion.Euler(0, 0, StartAngle - 90) * Vector2.up * CircularPath.GetRadius(); // Outer point at start angle
            points[2] = Quaternion.Euler(0, 0, EndAngle - 90) * Vector2.up * CircularPath.GetRadius(); // Outer point at end angle
            points[3] = points[1] * 0.1f; // Inner point at start angle (adjust for desired thickness)

            polygonCollider.points = points;
        }
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
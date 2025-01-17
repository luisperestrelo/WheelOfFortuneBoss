using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: make the radius come from a CircularPath object attached to the boss
public class PlayerSpinMovement : MonoBehaviour
{
    [SerializeField] private Transform anchorPoint;
    //[SerializeField] private float defaultRotationSpeed = 180f; // degrees per second
    [Header("Don't use this to change radius, use PlayerPath GameObject")]
    [SerializeField] private float radius = 5f;


    [SerializeField] private bool drawLine = true;
    [SerializeField] private CircularPath circularPath;

    [Header("Movement Settings")]
    [SerializeField] private float maxRotationSpeed = 180f; // degrees per second 
    [SerializeField] private float accelerationRate = 1080f; // degrees per second squared (high value for quick acceleration)
    [Tooltip("NOT USED IN MOST SCHEMES, MOST SCHEMES USE THE SAME ACCELERATION VALUE. This is the rate at which the player will decelerate when changing direction.")]
    [SerializeField] private float decelerationRate = 720f; // didnt do much with this, but was testing different speeds for accelerating and decelerating  
    [SerializeField] private bool usesAcceleration = true; 
    public bool UsesAcceleration
    {
        get { return usesAcceleration; }
    }

    [Header("Movement Scheme")]
    [SerializeField] private IMovementScheme currentMovementScheme;
    [SerializeField] private MovementSchemeType movementSchemeType;

    private enum MovementSchemeType
    {
        TapToChangeDirection, // Space to change direction
        HoldToMove, // E and Q to move
        HoldToMoveAndDash, // E and Q to move, W/M2/Space to dash
        TwoInputSpaceAndBoost, // Space to change Direction, W/M2 to boost
        TwoInputSpaceAndSlow, // Space to change Direction, W/M2 to slow
        TwoInputSpaceAndDash, // Space to change Direction, W/M2 to dash
        TwoInputSpaceAndStop, // Space to change Direction, W/M2 to stop
        TwoInputSpaceAndMove, // Space to change Direction, W/M2 to move
        TwoInputSpaceAndToggleMove, // Space to change Direction, W/M2 to toggle move
        DifferentAccelerationandDecelerationvalues, // Kinda ignore this one, but its same as TapToChangeDirection basically
        HoldToMoveWithDashBoostAndSlow, // E and Q to move, W/M2/Space to dash, F to boost, G to slow    
        TapToChangeFireToSlow //Space to change direction, shoot to slow.
    }

    private float _currentAngle = 50f;
    public float CurrentAngle
    {
        get { return _currentAngle; }
        set { _currentAngle = value; }
    }

    private float _direction = 1f;
    public float Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    private float _currentRotationSpeed;
    public float CurrentRotationSpeed
    {
        get { return _currentRotationSpeed; }
        set { _currentRotationSpeed = value; }
    }

    public float MaxRotationSpeed
    {
        get { return maxRotationSpeed; }
    }

    public float AccelerationRate
    {
        get { return accelerationRate; }
    }

    public float DecelerationRate
    {
        get { return decelerationRate; }
    }
    

    private LineRenderer _lineRenderer;

    [Header("Wall Collision")]
    [SerializeField] private bool enableWallCollisions = true;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        radius = circularPath.GetRadius();

        InitializeMovementScheme();
    }

    private void InitializeMovementScheme()
    {
        switch (movementSchemeType)
        {
            case MovementSchemeType.TapToChangeDirection:
                currentMovementScheme = new TapToChangeDirection();
                break;
            case MovementSchemeType.DifferentAccelerationandDecelerationvalues:
                currentMovementScheme = new DifferentAccelerationandDecelerationvalues();
                break;
            case MovementSchemeType.HoldToMove:
                currentMovementScheme = new HoldToMove();
                break;
            case MovementSchemeType.TwoInputSpaceAndBoost:
                currentMovementScheme = new TwoInputSpaceAndBoost();
                break;
            case MovementSchemeType.TwoInputSpaceAndSlow:
                currentMovementScheme = new TwoInputSpaceAndSlow();
                break;
            case MovementSchemeType.TwoInputSpaceAndDash:
                currentMovementScheme = new TwoInputSpaceAndDash();
                break;
            case MovementSchemeType.TwoInputSpaceAndStop:
                currentMovementScheme = new TwoInputSpaceAndStop();
                break;
            case MovementSchemeType.TwoInputSpaceAndMove:
                currentMovementScheme = new TwoInputSpaceAndMove();
                break;
            case MovementSchemeType.TwoInputSpaceAndToggleMove:
                currentMovementScheme = new TwoInputSpaceAndToggleMove();
                break;
            case MovementSchemeType.HoldToMoveAndDash:
                currentMovementScheme = new HoldToMoveAndDash();
                break;
            case MovementSchemeType.HoldToMoveWithDashBoostAndSlow:
                currentMovementScheme = new HoldToMoveWithDashBoostAndSlow();
                break;
            case MovementSchemeType.TapToChangeFireToSlow:
                currentMovementScheme = new TapToChangeFireToSlow();
                break;
            default:
                Debug.LogError("Invalid movement scheme type selected.");
                break;
        }

        currentMovementScheme.Initialize(this);
    }

    private void Update()
    {
        if (circularPath != null)
        {
            radius = circularPath.GetRadius();
        }

        currentMovementScheme.UpdateMovement();
        /*
               if (Input.GetKeyDown(KeyCode.W) && Time.time > nextBoostTime)
               {
                   StartCoroutine(SpeedBoostRoutine());
               }
               else if (Input.GetKeyDown(KeyCode.S) && Time.time > nextSlowTime)
               {
                   StartCoroutine(SpeedSlowRoutine());
               } */

        DrawLine();

        if (anchorPoint == null)
        {
            return;
        }

        // Wall collision check (after movement update)
        if (enableWallCollisions)
        {
            CheckForWallCollisions();
        }

        float x = anchorPoint.position.x + radius * Mathf.Cos(_currentAngle * Mathf.Deg2Rad);
        float y = anchorPoint.position.y + radius * Mathf.Sin(_currentAngle * Mathf.Deg2Rad);

        transform.position = new Vector3(x, y, transform.position.z);
    }

    /*     private IEnumerator SpeedBoostRoutine()
        {
            float oldSpeed = _currentRotationSpeed; // might be useful in a later version
            _currentRotationSpeed = boostAmount;

            nextBoostTime = Time.time + boostCooldown;

            yield return new WaitForSeconds(boostDuration);

            //_currentRotationSpeed = oldSpeed;
            _currentRotationSpeed = maxRotationSpeed;
        }

        private IEnumerator SpeedSlowRoutine()
        {
            float oldSpeed = _currentRotationSpeed;
            _currentRotationSpeed = slowAmount;

            nextSlowTime = Time.time + slowCooldown;

            yield return new WaitForSeconds(slowDuration);

            //_currentRotationSpeed = oldSpeed;
            _currentRotationSpeed = maxRotationSpeed;
        } */

    // TODO: this is a very lazy implementation, but the idea would be to use this for attacks that are aimed at "in front" of the player
    public Vector3 GetFuturePosition(float time)
    {
        float futureAngle = _currentAngle + _direction * _currentRotationSpeed * time;
        float x = anchorPoint.position.x + radius * Mathf.Cos(futureAngle * Mathf.Deg2Rad);
        float y = anchorPoint.position.y + radius * Mathf.Sin(futureAngle * Mathf.Deg2Rad);
        return new Vector3(x, y, transform.position.z);
    }
//
    //Draw line from player to anchor point using linerenderer
    private void DrawLine()
    {

        if (!drawLine)
        {
            return;
        }



        if (_lineRenderer == null)
        {
            return;
        }

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, anchorPoint.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, anchorPoint.position);
    }

    private void CheckForWallCollisions()
    {
        Wall[] walls = FindObjectsOfType<Wall>();
        foreach (Wall wall in walls)
        {
            if (IsAngleWithinRange(_currentAngle, wall.WallStartAngle, wall.WallEndAngle))
            {
                // Determine the closest edge of the wall and adjust the player's angle
                float distanceToStart = Mathf.DeltaAngle(_currentAngle, wall.WallStartAngle);
                float distanceToEnd = Mathf.DeltaAngle(_currentAngle, wall.WallEndAngle);

                if (Mathf.Abs(distanceToStart) < Mathf.Abs(distanceToEnd))
                {
                    _currentAngle = wall.WallStartAngle;
                }
                else
                {
                    _currentAngle = wall.WallEndAngle;
                }

                // Stop the player's rotation upon collision
                _currentRotationSpeed = 0f;
            }
        }
    }

    private bool IsAngleWithinRange(float angle, float start, float end)
    {
        // Handle cases where the wall spans the 0/360 boundary
        if (start > end)
        {
            return angle >= start || angle <= end;
        }
        else
        {
            return angle >= start && angle <= end;
        }
    }

}

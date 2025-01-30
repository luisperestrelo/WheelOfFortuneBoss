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

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = .8f;


    public bool isCollidingWithWall = false;

    public enum MovementSchemeType
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

        float previousAngle = _currentAngle;

        // Update the movement scheme (handles input and direction changes)
        currentMovementScheme.UpdateMovement();

        if (IsThereAWallInFront())
        {
            // If a wall is detected, prevent movement by resetting the angle
            // later we can draw a tangent from the wall to the circumference and set the angle to the angle of the tangent 
            _currentAngle = previousAngle;
            _currentRotationSpeed = 0f;
        }

        DrawLine();

        if (anchorPoint == null)
        {
            return;
        }

        // Wall collision check (after movement update)
        /*         if (enableWallCollisions)
                {
                    CheckForWallCollisions();
                } */

        if (IsThereAWallInFront())
        {
            _currentAngle = previousAngle;
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
        futureAngle = (futureAngle % 360f + 360f) % 360f;
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
        var scaleX = Mathf.Lerp(0.8f, 1f, Mathf.PingPong(Time.time * 0.5f, 1));
        _lineRenderer.textureScale = new Vector2(scaleX, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, anchorPoint.position);

        // Calculate the movement direction based on the current angle, direction, and a 90-degree offset
        float movementAngle = _currentAngle + 90f * _direction;
        Vector2 movementDirection = new Vector2(Mathf.Cos(movementAngle * Mathf.Deg2Rad), Mathf.Sin(movementAngle * Mathf.Deg2Rad));

        // Normalize the movement direction
        movementDirection.Normalize();

        // Calculate the endpoint of the line for wall checking
        Vector2 wallCheckEnd = (Vector2)wallCheck.position + movementDirection * wallCheckDistance;

        // Draw the line from the wallCheck position to the calculated endpoint
        Gizmos.DrawLine(wallCheck.position, wallCheckEnd);
    }

    //deprecateds
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

                _currentRotationSpeed = 0f;
            }
        }
    }
    /* 
        private bool isThereAWallInFront()
        {
            Wall[] walls = FindObjectsOfType<Wall>();
            foreach (Wall wall in walls)
            {
                if (IsAngleWithinRange(_currentAngle, wall.WallStartAngle, wall.WallEndAngle))
                {


                    // Stop the player's rotation upon collision
                    _currentRotationSpeed = 0f;

                    return true;
                }
            }
            return false;
        } */

    private bool IsThereAWallInFront()
    {
        // Calculate the movement direction based on the current angle, direction, and a 90-degree offset
        float movementAngle = _currentAngle + 90f * _direction;
        Vector2 movementDirection = new Vector2(Mathf.Cos(movementAngle * Mathf.Deg2Rad), Mathf.Sin(movementAngle * Mathf.Deg2Rad));

        movementDirection.Normalize();

        // Cast a ray in the direction of movement to check for walls
        
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, movementDirection, wallCheckDistance, whatIsGround);

        //behaving werid, works sometimes, but we do it again OnDrawGizmos
        Debug.DrawRay(wallCheck.position, movementDirection * wallCheckDistance, hit.collider != null ? Color.red : Color.green);

        if (hit.collider != null)
        {
            return true; // Wall detected
        }

        return false; // No wall detected
    }





    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            isCollidingWithWall = true;
            Debug.Log("Wall collision detected");
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            isCollidingWithWall = false;
            Debug.Log("Wall collision exited");
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

    public void ChangeMovementScheme(MovementSchemeType newMovementSchemeType)
    {
        movementSchemeType = newMovementSchemeType;
        InitializeMovementScheme();
    }

    public MovementSchemeType GetCurrentMovementScheme()
    {
        return movementSchemeType;
    }

}

using UnityEngine;

public class TwoInputSpaceAndBoostAccel : IMovementScheme
{
    private PlayerSpinMovement _player;

    public void Initialize(PlayerSpinMovement player)
    {
        _player = player;
    }

    public void UpdateMovement()
    {
        // Handle input for changing direction
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Direction *= -1f;
        }

        // Handle input for speed boost
        float targetSpeed = _player.Direction * _player.MaxRotationSpeed;
        if (Input.GetKey(KeyCode.W) || Input.GetMouseButton(1)) // Right-click
        {
            targetSpeed *= 2f; // Apply boost multiplier
        }

        // Use ternary operator for conditional acceleration/deceleration
        _player.CurrentRotationSpeed = Mathf.MoveTowards(
            _player.CurrentRotationSpeed,
            targetSpeed,
            Mathf.Abs(_player.CurrentRotationSpeed) < Mathf.Abs(targetSpeed)
                ? _player.AccelerationRate * Time.deltaTime
                : _player.DecelerationRate * Time.deltaTime
        );

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;
    }
} 
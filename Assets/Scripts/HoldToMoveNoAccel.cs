using UnityEngine;

public class HoldToMoveNoAccel : IMovementScheme
{
    private PlayerSpinMovement _player;

    public void Initialize(PlayerSpinMovement player)
    {
        _player = player;
    }

    public void UpdateMovement()
    {
        // Handle input
        if (Input.GetKey(KeyCode.E))
        {
            _player.Direction = -1f; // Counter-clockwise
            _player.CurrentRotationSpeed = _player.Direction * _player.MaxRotationSpeed; // Instant speed
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            _player.Direction = 1f; // Clockwise
            _player.CurrentRotationSpeed = _player.Direction * _player.MaxRotationSpeed; // Instant speed
        }
        else
        {
            // No input, stop instantly
            _player.CurrentRotationSpeed = 0f;
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;
    }
} 
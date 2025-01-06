using UnityEngine;

public class HoldToMove : IMovementScheme
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
            _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, _player.Direction * _player.MaxRotationSpeed, _player.AccelerationRate * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            _player.Direction = 1f; // Clockwise
            _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, _player.Direction * _player.MaxRotationSpeed, _player.AccelerationRate * Time.deltaTime);
        }
        else
        {
            // No input, decelerate to a stop
            _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, 0f, _player.DecelerationRate * Time.deltaTime);
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;
    }
} 
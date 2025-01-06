using UnityEngine;

public class TwoInputSpaceAndToggleMove : IMovementScheme
{
    private PlayerSpinMovement _player;
    private bool _isMoving = false;

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

        // Handle input for toggling movement
        if (Input.GetKeyDown(KeyCode.W) || Input.GetMouseButtonDown(1)) // Right-click
        {
            _isMoving = !_isMoving;
        }

        // Move or stop based on _isMoving flag
        if (_isMoving)
        {
            // Accelerate to normal speed
            _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, _player.Direction * _player.MaxRotationSpeed, _player.AccelerationRate * Time.deltaTime);
        }
        else
        {
            // Decelerate to a stop
            _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, 0f, _player.DecelerationRate * Time.deltaTime);
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;
    }
}

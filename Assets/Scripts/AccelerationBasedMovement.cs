using UnityEngine;

public class AccelerationBasedMovement : IMovementScheme
{
    private PlayerSpinMovement _player;

    public void Initialize(PlayerSpinMovement player)
    {
        _player = player;
    }

    public void UpdateMovement()
    {
        // Handle input
        if (Input.GetKeyDown(KeyCode.V))
        {
            _player.Direction *= -1f;
        }

        // Acceleration/Deceleration
        float targetSpeed = _player.Direction * _player.MaxRotationSpeed;

        // Calculate the new rotation speed
        float newRotationSpeed = Mathf.MoveTowards(
            _player.CurrentRotationSpeed,
            targetSpeed,
            Mathf.Abs(_player.CurrentRotationSpeed) < Mathf.Abs(targetSpeed)
                ? _player.AccelerationRate * Time.deltaTime
                : _player.DecelerationRate * Time.deltaTime
        );

        // Update the angle based on the new rotation speed
        _player.CurrentAngle += newRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;

        // Set the current rotation speed to the new value
        _player.CurrentRotationSpeed = newRotationSpeed;
    }
}
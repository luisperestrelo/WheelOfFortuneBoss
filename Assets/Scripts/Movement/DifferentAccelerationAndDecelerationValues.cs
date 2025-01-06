using UnityEngine;

//experimenting
public class DifferentAccelerationandDecelerationvalues : IMovementScheme
{
    private PlayerSpinMovement _player;

    public void Initialize(PlayerSpinMovement player)
    {
        _player = player;
    }

    public void UpdateMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Direction *= -1f;
        }

        float targetSpeed = _player.Direction * _player.MaxRotationSpeed;

        _player.CurrentRotationSpeed = Mathf.MoveTowards(
            _player.CurrentRotationSpeed,
            targetSpeed,
            _player.UsesAcceleration ?
            (Mathf.Abs(_player.CurrentRotationSpeed) < Mathf.Abs(targetSpeed)
                ? _player.AccelerationRate * Time.deltaTime
                : _player.DecelerationRate * Time.deltaTime) :
            Mathf.Infinity
        );

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;
    }
}
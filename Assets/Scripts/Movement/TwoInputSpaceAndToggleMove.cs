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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Direction *= -1f;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetMouseButtonDown(1)) 
        {
            _isMoving = !_isMoving;
        }

        if (_isMoving)
        {
            if (_player.UsesAcceleration)
            {
                _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, _player.Direction * _player.MaxRotationSpeed, _player.AccelerationRate * Time.deltaTime);
            }
            else
            {
                _player.CurrentRotationSpeed = _player.Direction * _player.MaxRotationSpeed;
            }
        }
        else
        {
            if (_player.UsesAcceleration)
            {
                _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, 0f, _player.DecelerationRate * Time.deltaTime);
            }
            else
            {
                _player.CurrentRotationSpeed = 0f;
            }
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;
    }
}

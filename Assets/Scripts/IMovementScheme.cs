using UnityEngine;

public interface IMovementScheme
{
    void Initialize(PlayerSpinMovement player);
    void UpdateMovement();
} 
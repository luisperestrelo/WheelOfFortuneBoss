using UnityEngine;

public interface IMovementScheme
{
    string Description { get; }
    void Initialize(PlayerSpinMovement player);
    void UpdateMovement();
} 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class PlayerControlModule : MonoBehaviour
{
    public PlayerType playerType;
    protected Player _owner;

    public virtual void Initialize(Player owner)
    {
        _owner = owner;
    }
    public abstract void HandleRotation(Vector3 axis, float angle);
    public abstract void HandleMovement(Vector2 vel);
    public abstract void HandleCollisions(Player owner);

    public enum PlayerType { DRONE, QUADRUPED, TANK, Count }
}

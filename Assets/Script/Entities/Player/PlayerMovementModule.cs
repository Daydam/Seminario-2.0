using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class PlayerControlModule : MonoBehaviour
{
    public PlayerType playerType;

    public abstract void HandleRotation(Vector3 axis, float angle);
    public abstract void HandleMovement(Vector2 vel);

    public enum PlayerType { DRONE, QUADRUPED, TANK, Count }
}

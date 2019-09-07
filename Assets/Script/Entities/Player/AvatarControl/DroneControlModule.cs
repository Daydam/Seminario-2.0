using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DroneControlModule : PlayerControlModule
{
    public override void HandleRotation(Vector3 axis, float angle)
    {
        transform.Rotate(axis, angle);
    }

    public override void HandleMovement(Vector2 vel)
    {
        //CUANDO SE ME CANTE EL OJETE LO HAGO v2
    }
}

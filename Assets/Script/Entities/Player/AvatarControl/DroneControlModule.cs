using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DroneControlModule : PlayerControlModule
{
    public override void Initialize(Player _owner)
    {
        
    }

    public override void HandleRotation(Vector3 axis, float angle)
    {
        transform.Rotate(axis, angle);
    }

    public override void HandleMovement(Vector2 vel)
    {
        //CUANDO SE ME CANTE EL OJETE LO HAGO v2
    }

    public override void HandleCollisions(Player owner)
    {
        var playerCol = owner.GetComponent<Collider>();
        //harcodeado, fixear
        print("FIXEAME PORFA, ESTOY HARDCODEADO");
        var stage = GameObject.FindObjectOfType<CrystalPyramidStage>();

        if (stage)
        {
            var lowObstacles = stage.crystalRomboids.SelectMany(x => x.GetStructuresColliders(ObstacleHeight.LOW));
            foreach (var obstacleCollider in lowObstacles)
            {
                Physics.IgnoreCollision(playerCol, obstacleCollider);
            }
        }
    }
}

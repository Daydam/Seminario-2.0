using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    bool running;
    public Vector3 positionOffset;
    public Vector3 aimOffset;
    [Range(0, 1)]
    public float movementSpeed;
    [Range(0, 1)]
    public float aimSpeed;

    Player target;
    Vector3 targetSight;

    public void AssignTarget(Player target)
    {
        this.target = target;
        transform.position = target.transform.position;
        transform.forward = target.transform.forward;
        targetSight = transform.forward;
        running = true;
    }

    private void FixedUpdate()
    {
        if(running)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position + target.transform.right * positionOffset.x + target.transform.up * positionOffset.y + target.transform.forward * positionOffset.z, movementSpeed);
            targetSight = Vector3.Lerp(targetSight, target.transform.position + target.transform.right * aimOffset.x + target.transform.up * aimOffset.y + target.transform.forward * aimOffset.z, aimSpeed);
            transform.LookAt(targetSight);
        }
    }
}

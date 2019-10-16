using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuadrupedControlModule : PlayerControlModule
{
    public string boneName;
    public string headName;
    Transform _headBone;
    Transform _headMesh;
    Transform _robotBody;
    Player _owner;
    private Transform hardcodeForCameraForward;

    public Transform Head
    {
        get
        {
            if (_headBone == null)
            {
                _headBone = GetComponentsInChildren<Transform>().Where(x => x.name == headName).First();
            }
            return _headBone;
        }
    }

    public Transform HardcodeForCameraForward
    {
        get
        {
            if (hardcodeForCameraForward == null)
            {
                hardcodeForCameraForward = GetComponentsInChildren<Transform>().Where(x => x.name == "DIOS MIO").First();
            }
            return hardcodeForCameraForward;
        }
    }

    void Start()
    {
        hardcodeForCameraForward = GetComponentsInChildren<Transform>().Where(x => x.name == "DIOS MIO").First();

        _headBone = GetComponentsInChildren<Transform>().Where(x => x.name == boneName).First();
        _robotBody = GetComponentsInChildren<Transform>().Where(x => x.name == "Quadro").First();
        _headMesh = GetComponentsInChildren<Transform>().Where(x => x.name == headName).First();
        _owner = GetComponent<Player>();
    }

    public override void HandleRotation(Vector3 axis, float angle)
    {
        //GRACIAS SANTIAGO POR HACER EL HUESO COMO EL OJETE
        //TO DO BORRAR ESTO CUANDO SE FIXEE EL HUESO
        var fixedDirection = Quaternion.Euler(90, 0, 90) * axis;
        transform.Rotate(axis, angle);
        _robotBody.Rotate(axis, -angle);
        _headBone.Rotate(fixedDirection, angle);
    }

    public override void HandleMovement(Vector2 vel)
    {
        _owner.AnimationController.SetMovement(vel != Vector2.zero);
    }

    public override void HandleCollisions(Player owner)
    {

    }
}


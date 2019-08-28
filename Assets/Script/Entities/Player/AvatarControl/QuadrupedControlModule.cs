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
    Player _owner;

    public Transform Head
    {
        get
        {
            if (_headMesh == null)
            {
                _headMesh = GetComponentsInChildren<Transform>().Where(x => x.name == headName).First();
            }
            return _headMesh;
        }
    }

    void Start()
    {
        _headBone = GetComponentsInChildren<Transform>().Where(x => x.name == boneName).First();
        _headMesh = GetComponentsInChildren<Transform>().Where(x => x.name == headName).First();
        _owner = GetComponent<Player>();
    }

    public override void HandleRotation(Vector3 axis, float angle)
    {
        //GRACIAS SANTIAGO POR HACER EL HUESO COMO EL OJETE
        //TO DO BORRAR ESTO CUANDO SE FIXEE EL HUESO
        var fixedDirection = Quaternion.Euler(90, 0, 90) * axis;
        _headBone.Rotate(fixedDirection, angle);
    }

    public override void HandleMovement(Vector3 axis, float angle)
    {
        //CUANDO SE ME CANTE EL OJETE LO HAGO
    }
}

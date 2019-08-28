using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuadrupedControlModule : PlayerControlModule
{
    public string boneName;
    Transform _headBone;
    Player _owner;

    public Transform HeadBone
    {
        get
        {
            if (_headBone == null)
            {
                _headBone = GetComponentsInChildren<Transform>().Where(x => x.name == boneName).First();
            }
            return _headBone;
        }

        private set
        {
            _headBone = value;
        }
    }

    void Start()
    {
        _headBone = GetComponentsInChildren<Transform>().Where(x => x.name == boneName).First();
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

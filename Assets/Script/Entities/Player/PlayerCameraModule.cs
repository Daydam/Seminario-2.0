using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerCameraModule : MonoBehaviour
{
    [SerializeField] Vector3 _offset;
    public Vector3 Offset
    {
        get { return _offset; }
    }
}

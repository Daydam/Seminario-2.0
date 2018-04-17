using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillTrail : MonoBehaviour
{
    TrailRenderer[] _trails;
    public float width;
    public float time;
    public float minVertextDistance;

    void Start()
    {
        _trails = GetComponentsInChildren<TrailRenderer>();
    }

    public void ShowTrails()
    {
        foreach (var tr in _trails)
        {
            tr.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void ReactToMovement(bool activate)
    {
        foreach (var tr in _trails)
        {
            tr.startWidth = activate ? width : 0;
            tr.endWidth = activate ? width : 0;
        }
    }


    public void StopShowing()
    {
        foreach(var tr in _trails)
        {
            tr.gameObject.layer = LayerMask.NameToLayer("Unrenderizable");
        }
    }
}

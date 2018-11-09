using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerSightingHandler : MonoBehaviour
{
    bool _bestPlayer;
    bool _castingVortex;
    Camera _cam;
    Player _me;
    GameObject _particle;
    readonly string particleName = "OutOfSight";

    public void Init()
    {
        _me = GetComponent<Player>();
        _cam = _me.Cam.GetComponent<Camera>();
        _particle = transform.GetComponentsInChildren<Transform>().Where(x => x.name == particleName).First().gameObject;
        var layer = _me.myID + 1;
        _particle.layer = LayerMask.NameToLayer("P" + layer + "ONLY");

        EventManager.Instance.AddEventListener(Events.SkillEvents.VortexStart, OnVortexStart);
        EventManager.Instance.AddEventListener(Events.SkillEvents.VortexEnd, OnVortexEnd);
    }

    void OnVortexStart(object[] paramsContainer)
    {
        var play = (Player)paramsContainer[0];
        if (play.Equals(_me))
        {
            SetVortexUsage(true);
        }
    }

    void OnVortexEnd(object[] paramsContainer)
    {
        var play = (Player)paramsContainer[0];
        if (play.Equals(_me))
        {
            SetVortexUsage(false);
        }
    }

    public void SetBestPlayer(bool active)
    {
        _bestPlayer = active;
    }

    public void SetVortexUsage(bool active)
    {
        _castingVortex = active;
    }

    void LateUpdate()
    {
        if (!_particle) return;

        if (_bestPlayer || _castingVortex)
        {
            _particle.SetActive(false);
            return;
        }

        var ray = new Ray(_cam.transform.position, (_me.transform.position - _cam.transform.position).normalized);
        RaycastHit rch;
        var dist = Vector3.Distance(_me.transform.position, _cam.transform.position) + .3f;

        var didHit = !Physics.Raycast(ray, out rch, dist, LayerMask.GetMask("DestructibleWall", "DestructibleStructure", "Structure"));
        if (didHit) _particle.SetActive(false);
        else _particle.SetActive(!rch.collider.gameObject.Equals(_me.gameObject));

    }

}

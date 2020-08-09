using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Firepower.Events;

public class PlayerSightingHandler : MonoBehaviour
{
    bool _bestPlayer;
    bool _castingVortex;
    Camera _cam;
    Player _owner;
    Renderer[] _rim;
    readonly string particleName = "OutOfSight";
    int layer;

    public void Init()
    {
        _owner = GetComponent<Player>();
        _cam = _owner.Cam.GetComponent<Camera>();
        _rim = transform.Find(particleName).GetComponentsInChildren<Renderer>();
        layer = GameManager.Instance.Players.IndexOf(_owner) + 1;

        foreach (var item in _rim)
        {
            item.gameObject.layer = LayerMask.NameToLayer("P" + layer + "ONLY");
            item.material.SetColor("_Tint", _owner.LightsModule.GetPlayerColor());

            item.material.SetVector("_CollapsePosition", new Vector3(55555, 55555, 55555));
        }

        EventManager.Instance.AddEventListener(SkillEvents.VortexStart, OnVortexStart);
        EventManager.Instance.AddEventListener(SkillEvents.VortexEnd, OnVortexEnd);
    }

    void OnVortexStart(object[] paramsContainer)
    {
        var play = (Player)paramsContainer[0];
        if (play.Equals(_owner))
        {
            SetVortexUsage(true);
        }
    }

    void OnVortexEnd(object[] paramsContainer)
    {
        var play = (Player)paramsContainer[0];
        if (play.Equals(_owner))
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

        foreach (var item in _rim)
        {
            if (_castingVortex) item.material.SetFloat("_Active", 1);
            else item.material.SetFloat("_Active", 0);
        }
    }

    void LateUpdate()
    {
        if (/*THIS IS A SAFEGUARD, I WANT THE GAME TO BE FUCKING PLAYABLE. FUTURE TINCHO, FIXEAME ESTA*/ _rim == null ||
            !_rim.Any()) return;

        if (_bestPlayer)
        {
            foreach (var item in _rim)
            {
                item.material.SetFloat("_Active", 1);

                item.gameObject.layer = 1;
            }
        }
        if (!_bestPlayer)
        {
            foreach (var item in _rim)
            {
                item.gameObject.layer = LayerMask.NameToLayer("P" + layer + "ONLY");
            }
            var ray = new Ray(_cam.transform.position, (_owner.transform.position - _cam.transform.position).normalized);
            RaycastHit rch;
            var dist = Vector3.Distance(_owner.transform.position, _cam.transform.position) + .3f;

            var didHit = Physics.Raycast(ray, out rch, dist, LayerMask.GetMask("DestructibleWall", "DestructibleStructure", "Structure"));

            foreach (var item in _rim)
            {
                if (didHit) item.material.SetFloat("_Active", 1);
                else item.material.SetFloat("_Active", 0);
            }
        }

    }

}

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
    Renderer _rim;
    readonly string particleName = "OutOfSight";
    int layer;

    public void Init()
    {
        _me = GetComponent<Player>();
        _cam = _me.Cam.GetComponent<Camera>();
        _rim = transform.GetComponentsInChildren<Transform>().Where(x => x.name == particleName).First().GetComponent<Renderer>();
        layer = GameManager.Instance.Players.IndexOf(_me) + 1;
        _rim.gameObject.layer = LayerMask.NameToLayer("P" + layer + "ONLY");
        _rim.material.SetColor("_Tint", _me.Rend.material.GetColor("_PlayerColor"));

        _rim.material.SetVector("_CollapsePosition", new Vector3(55555, 55555, 55555));

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
        //_rim.gameObject.SetActive(_castingVortex);
        if(_castingVortex) _rim.material.SetFloat("_Active", 1);
        else _rim.material.SetFloat("_Active", 0);
    }

    void LateUpdate()
    {
        if (!_rim) return;

        if (_bestPlayer)
        {
            //_rim.SetActive(true);
            _rim.material.SetFloat("_Active", 1);

            _rim.gameObject.layer = 1;
        }
        if(!_bestPlayer)
        {
            _rim.gameObject.layer = LayerMask.NameToLayer("P" + layer + "ONLY");
            var ray = new Ray(_cam.transform.position, (_me.transform.position - _cam.transform.position).normalized);
            RaycastHit rch;
            var dist = Vector3.Distance(_me.transform.position, _cam.transform.position) + .3f;

            var didHit = Physics.Raycast(ray, out rch, dist, LayerMask.GetMask("DestructibleWall", "DestructibleStructure", "Structure"));
            //_rim.SetActive(didHit);
            if(didHit)_rim.material.SetFloat("_Active", 1);
            else _rim.material.SetFloat("_Active", 0);
        }

    }

}

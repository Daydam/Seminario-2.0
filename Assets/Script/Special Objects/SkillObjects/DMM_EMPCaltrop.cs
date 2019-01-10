using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DMM_EMPCaltrop : MonoBehaviour
{
    Rigidbody _rb;
    Collider _coll;
    Action<DMM_EMPCaltrop> _activationCallback;

    public SO_EMPCaltrop skillData;

    float _lifeTime;

    public float ElapsedLifeTime
    {
        get
        {
            return _lifeTime;
        }
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _coll = GetComponent<Collider>();
    }

    public DMM_EMPCaltrop Spawn(Vector3 spawnPos, Vector3 fwd, string emmitter, Action<DMM_EMPCaltrop> activationCallback, SO_EMPCaltrop data)
    {
        skillData = data;

        transform.position = spawnPos;
        transform.forward = fwd;
        gameObject.tag = emmitter;
        _activationCallback = activationCallback;
        _lifeTime = 0;

        //Test
        //_coll.isTrigger = true;

        return this;
    }

    void Update()
    {
        _lifeTime += Time.deltaTime;
    }

    void OnTriggerEnter(Collider col)
    {
        var colTag = col.gameObject.tag;
        var activationCondition = colTag == "Player 1" || colTag == "Player 2" || colTag == "Player 3" || colTag == "Player 4";
        var filterLauncher = colTag != gameObject.tag;

        if (activationCondition && filterLauncher)
        {
            ActivateAOE();
        }
    }

    void ActivateAOE()
    {
        var players = Physics.OverlapSphere(transform.position, skillData.radius).Select(x => x.GetComponent<Player>()).Where(x => x != null);

        foreach (var pl in players)
        {
            pl.ApplySlowMovement(skillData.duration, skillData.amount);
        }

        _activationCallback(this);
    }

    public void ForceActivation()
    {
        ActivateAOE();
    }
}
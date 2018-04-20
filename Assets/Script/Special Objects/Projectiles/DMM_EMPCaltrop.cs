﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DMM_EMPCaltrop : MonoBehaviour
{
    Rigidbody _rb;
    Collider _coll;
    public float damage, activationDelay;
    float _duration, _radius, _amount;

    bool _planted;
    bool _activated;

    bool _showGizmos;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _coll = GetComponent<Collider>();
    }

    public DMM_EMPCaltrop Spawn(Vector3 spawnPos, Vector3 fwd, float slowDuration, float slowAmount, float radius, string emmitter)
    {
        transform.position = spawnPos;
        transform.forward = fwd;
        transform.parent = null;
        gameObject.tag = emmitter;
        _activated = false;

        //Test
        //_coll.isTrigger = true;

        _duration = slowDuration;
        _radius = radius;
        _amount = slowAmount;

        Plant();

        return this;
    }

    public static void Initialize(DMM_EMPCaltrop obj)
    {
        obj.gameObject.SetActive(true);
    }

    public static void Dispose(DMM_EMPCaltrop ojb)
    {
        ojb.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (_planted && _activated)
        {
            var colTag = col.gameObject.tag;
            var activationCondition = colTag == "Player 1" || colTag == "Player 2" || colTag == "Player 3" || colTag == "Player 4";
            var filterLauncher = colTag != gameObject.tag;

            if (activationCondition && filterLauncher)
            {
                ActivateAOE();
            }
        }
        else if (!_planted)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Shield"))
            {
                if (col.gameObject.GetComponentInParent<Player>().gameObject.tag == this.gameObject.tag) return;
                ReturnToPool();
            }
            else if (col.gameObject.tag != this.gameObject.tag)
            {
                Plant();
            }
        }
    }

    void Plant()
    {
        //Test
        //_coll.isTrigger = true;

        _planted = true;
        StartCoroutine(DelayedActivation(activationDelay));
    }

    IEnumerator DelayedActivation(float time)
    {
        yield return new WaitForSeconds(time);
        _activated = true;
    }

    void ActivateAOE()
    {
        _showGizmos = true;

        var players = Physics.OverlapSphere(transform.position, _radius).Select(x => x.GetComponent<Player>()).Where(x => x != null);

        foreach (var pl in players)
        {
            pl.ApplySlowMovement(_duration, _amount);
        }

        ReturnToPool();
    }

    void ReturnToPool()
    {
        _showGizmos = false;
        EMPCaltropSpawner.Instance.ReturnToPool(this);
    }

    private void OnDrawGizmos()
    {
        if (_showGizmos)
        {
            DrawDebugAoE(.67f, 1.4f, 2);
        }
    }

    void DrawDebugAoE(float min, float med, float max)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, max);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, med);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, min);
    }
}

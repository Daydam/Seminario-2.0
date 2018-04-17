﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 100% de Efecto en AoE: hasta 33.5% del área máxima.
///60% de Efecto en AoE: hasta 70% del área máxima.
///15% de Efecto en AoE: hasta 100% del área máxima.
/// </summary>
public class DMM_GrenadeImpactFrag : MonoBehaviour
{
    AnimationCurve _AOEDecay;
    Rigidbody _rb;
    public float maximumRadius;
    float minAoE, medAoE, maxAoE;
    public float damage, knockback, speed;
    float _travelledDistance;
    float _maximumDistance;

    bool _stopMoving;

    bool _showGizmos;

    void Awake()
    {
        SetAoEValues();
        SetCurveValues();
        _rb = GetComponent<Rigidbody>();
    }

    void SetAoEValues()
    {
        minAoE = maximumRadius * 0.335f;
        medAoE = maximumRadius * 0.7f;
        maxAoE = maximumRadius * 1f;
    }

    void SetCurveValues()
    {
        _AOEDecay = new AnimationCurve();
        var initialKey = new Keyframe(minAoE, 1, 0, 0);
        _AOEDecay.AddKey(initialKey);
        var startFalloff = new Keyframe(medAoE, .6f, 0, 0);
        _AOEDecay.AddKey(startFalloff);
        var endFalloff = new Keyframe(maxAoE, .15f, 0, 0);
        _AOEDecay.AddKey(endFalloff);
        var zero = new Keyframe(maxAoE + 0.1f, 0, 0, 0);
        _AOEDecay.AddKey(zero);
    }

    public DMM_GrenadeImpactFrag SpawnGrenade(Vector3 spawnPos, Vector3 fwd, float maximumDistance, string emmitter)
    {
        transform.position = spawnPos;
        transform.forward = fwd;
        transform.parent = null;
        _maximumDistance = maximumDistance;
        _travelledDistance = 0;
        gameObject.tag = emmitter;
        _stopMoving = false;

        return this;
    }

    void Update()
    {
        if (_travelledDistance >= _maximumDistance) ActivateAOE();
    }

    void FixedUpdate()
    {
        if (!_stopMoving)
        {
            var distance = speed * Time.fixedDeltaTime;
            _travelledDistance += distance;
            _rb.MovePosition(_rb.position + transform.forward * distance);
        }
    }

    public static void InitializeBullet(DMM_GrenadeImpactFrag bulletObj)
    {
        bulletObj.gameObject.SetActive(true);
    }

    public static void DisposeBullet(DMM_GrenadeImpactFrag bulletObj)
    {
        bulletObj.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            if (col.gameObject.GetComponentInParent<Player>().gameObject.tag == this.gameObject.tag) return;
            ReturnToPool();
        }
        else if (col.gameObject.tag != this.gameObject.tag)
        {
            ActivateAOE();
        }
    }

    void ActivateAOE()
    {
        _showGizmos = true;
        _stopMoving = true;

        var cols = Physics.OverlapSphere(transform.position, maxAoE);
        var players = cols.Select(x => x.GetComponent<Player>()).Where(x => x != null);

        var allDestruct = cols.Select(x => x.gameObject)
                            .Where(x => x.layer == LayerMask.NameToLayer("DestructibleStructure") ||
                                        x.layer == LayerMask.NameToLayer("DestructibleWall"));

        foreach (var item in allDestruct)
        {
            item.SetActive(false);
        }

        var inMinAoe = players.Where(x => Vector3.Distance(x.gameObject.transform.position, transform.position) <= minAoE);

        foreach (var play in inMinAoe)
        {
            var multiplier = _AOEDecay.Evaluate(minAoE);
            play.TakeDamage(damage * multiplier);
            var forceDir = (play.transform.position - transform.position) * -1;
            play.ApplyKnockback(knockback * multiplier, forceDir.normalized);
        }

        var inMedAoe = players.Where(x => 
                       Vector3.Distance(x.gameObject.transform.position, transform.position) > minAoE
                       &&
                       Vector3.Distance(x.gameObject.transform.position, transform.position) <= medAoE);

        foreach (var play in inMedAoe)
        {
            var multiplier = _AOEDecay.Evaluate(medAoE);
            play.TakeDamage(damage * multiplier);
            var forceDir = transform.position - play.transform.position;
            play.ApplyKnockback(knockback * multiplier, forceDir.normalized);
        }

        var inMaxAoe = players.Where(x => Vector3.Distance(x.gameObject.transform.position, transform.position) > medAoE);

        foreach (var play in inMaxAoe)
        {
            var multiplier = _AOEDecay.Evaluate(maxAoE);
            play.TakeDamage(damage * multiplier);
            var forceDir = transform.position - play.transform.position;
            play.ApplyKnockback(knockback * multiplier, forceDir.normalized);
        }

        ReturnToPool();
    }

    void ReturnToPool()
    {
        _showGizmos = false;
        GrenadeImpactFragSpawner.Instance.ReturnToPool(this);
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

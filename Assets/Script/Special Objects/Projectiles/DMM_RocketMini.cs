using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using PhoenixDevelopment;

public class DMM_RocketMini : MonoBehaviour
{
    public SO_RocketSalvo skillData;

    Action<DMM_RocketMini> _activationCallback;

    Rigidbody _rb;
    Player _owner;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public DMM_RocketMini Spawn(Vector3 spawnPos, Vector3 landingPoint, string emmitter, Player owner, Action<DMM_RocketMini> activationCallback, SO_RocketSalvo data)
    {
        skillData = data;

        transform.position = spawnPos;
        transform.forward = (landingPoint - transform.position).normalized;
        transform.parent = null;
        gameObject.tag = emmitter;
        _owner = owner;

        _activationCallback = activationCallback;

        StartCoroutine(MoveToLandingPoint(landingPoint, spawnPos));

        return this;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("Shield")))
        {
            if (col.gameObject.GetComponentInParent<Player>().gameObject.TagMatchesWith(this.gameObject.tag)) return;
            _activationCallback(this);
        }
        else if (col.gameObject.TagDifferentFrom(this.gameObject.tag, "EMPCloud"))
        {
            if (col.GetComponent(typeof(IDamageBlocker)) as IDamageBlocker != null)
            {
                //do shit with the shield
                _activationCallback(this);
                return;
            }
            else ActivateAOE();
        }
    }

    void ActivateAOE()
    {
        StopAllCoroutines();

        var cols = Physics.OverlapSphere(transform.position, skillData.explosionRadius);

        var damageable = cols.Select(x => x.GetComponent(typeof(IDamageable)) as IDamageable).Where(x => x != null);

        foreach (var item in damageable)
        {
            item.TakeDamage(skillData.damage, gameObject.tag, transform.position);
        }

        var particleID = SimpleParticleSpawner.ParticleID.ROCKETMINI_EXPLOSION;
        var particle = SimpleParticleSpawner.Instance.particles[particleID].GetComponentInChildren<ParticleSystem>();

        SimpleParticleSpawner.Instance.SpawnParticle(particle.gameObject, transform.position, Quaternion.identity);

        _activationCallback(this);
    }

    IEnumerator MoveToLandingPoint(Vector3 landingPoint, Vector3 startPos)
    {
        var inst = new WaitForFixedUpdate();

        while (Vector3.Distance(transform.position, landingPoint) > .001f)
        {
            _rb.MovePosition(transform.position + (transform.forward * skillData.speed * Time.fixedDeltaTime));
            yield return inst;
        }

        ActivateAOE();
    }
}

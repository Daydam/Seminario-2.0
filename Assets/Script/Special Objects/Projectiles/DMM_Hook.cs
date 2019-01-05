using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DMM_Hook : MonoBehaviour
{
    public float maxRange = 7, travelTime = .5f;
    float _travelledDistance, _speed;

    Player _owner;
    Rigidbody _rb;
    bool _stopMoving;
    public bool movementFinished;

    IDamageable _target;
    public IDamageable Target
    {
        get { return _target; }
    }

    void Awake()
    {
        _speed = Time.fixedDeltaTime * maxRange / travelTime;
        _rb = GetComponent<Rigidbody>();
    }

    public DMM_Hook Spawn(Vector3 spawnPos, Vector3 dir, string emmitter, Player owner)
    {
        transform.position = spawnPos;
        transform.forward = dir;
        transform.parent = null;
        gameObject.tag = emmitter;
        _owner = owner;
        movementFinished = false;
        _stopMoving = false;
        _travelledDistance = 0;
        _target = null;

        return this;
    }

    void Update()
    {
        if (_travelledDistance >= maxRange)
        {
            _target = null;
            Activate();
        }
    }

    void FixedUpdate()
    {
        if (!_stopMoving)
        {
            _travelledDistance += _speed;
            _rb.MovePosition(_rb.position + transform.forward * _speed);
        }
    }

    void Activate()
    {
        movementFinished = true;
        _stopMoving = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("Shield")))
        {
            if (col.gameObject.GetComponentInParent<Player>().gameObject.TagMatchesWith(this.gameObject.tag)) return;

            _target = null;
            Activate();
        }
        else if (col.gameObject.TagDifferentFrom(this.gameObject.tag, "EMPCloud"))
        {
            if (col.GetComponent(typeof(IDamageBlocker)) as IDamageBlocker != null)
            {
                //do shit with the shield

                _target = null;
                Activate();

                return;
            }
            else if (col.GetComponent(typeof(IDamageable)) as IDamageable != null)
            {
                _target = col.GetComponent(typeof(IDamageable)) as IDamageable;
                Activate();
            }
        }
    }
}

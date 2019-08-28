using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PhoenixDevelopment;

public class DMM_ImplosiveCharge : MonoBehaviour
{
    public SO_ImplosiveCharge skillData;

    public bool movementFinished;

    Rigidbody _rb;
    Player _owner;
    bool _stopMoving;
    float _travelledDistance;

    public List<Player> targets;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        targets = new List<Player>();
    }

    public DMM_ImplosiveCharge Spawn(Vector3 spawnPos, Vector3 fwd, string emmitter, Player owner, SO_ImplosiveCharge data)
    {
        skillData = data;

        transform.position = spawnPos;
        transform.forward = fwd;
        transform.parent = null;
        _travelledDistance = 0;
        gameObject.tag = emmitter;
        _stopMoving = false;
        movementFinished = false;
        _owner = owner;

        return this;
    }

    void Update()
    {
        if (_travelledDistance >= skillData.maxDistance) ForceActivation();
    }

    void FixedUpdate()
    {
        if (!_stopMoving)
        {
            var distance = skillData.speed * Time.fixedDeltaTime;
            _travelledDistance += distance;
            _rb.MovePosition(_rb.position + transform.forward * distance);
        }
    }

    void FinishTrajectory()
    {
        movementFinished = true;
        _stopMoving = true;
    }

    void ActivateAOE()
    {
        _stopMoving = true;

        targets = Physics.OverlapSphere(transform.position, skillData.radius).Select(x => x.GetComponent<Player>()).Where(x => x != null && x != _owner).ToList();

        FinishTrajectory();
    }

    public void ForceActivation()
    {
        ActivateAOE();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("Shield")))
        {
            if (col.gameObject.GetComponentInParent<Player>().gameObject.TagMatchesWith(this.gameObject.tag)) return;

            //do shit with the shield?
            //targets = new List<Player>();
            //FinishTrajectory();

            ActivateAOE();
        }
        else if (col.gameObject.TagDifferentFrom(this.gameObject.tag, "EMPCloud"))
        {
            if (col.GetComponent(typeof(IDamageBlocker)) as IDamageBlocker != null)
            {
                //do shit with the shield
                targets = new List<Player>();
                FinishTrajectory();
                return;
            }
            else
            {
                ActivateAOE();
            }
        }
    }
}

//SANTIAGO HACEME LOS MODELOS
//PABLO LA CONCHA PUTA DE TU MADRE
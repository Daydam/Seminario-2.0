using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class DMM_ScramblerMine : MonoBehaviour
{
    Rigidbody _rb;
    /*Collider _coll;*/
    NavMeshAgent _nav;
    public float damage, activationDelay, speed/*, targettingRadius*/, maxHP;

    float _duration, _explosionRadius, _hp;

    bool _activated, _hasTarget;

    Player _target;

    #region States -- TODO!
    /*bool _isStunned;
    bool _isDisarmed;
    bool _isUnableToMove;
    public bool IsStunned { get { return _isStunned; } }
    public bool IsDisarmed { get { return _isDisarmed; } }
    public bool IsUnableToMove { get { return _isUnableToMove; } }

    /// <summary>
    /// TODO: Hacer que pueda ser mayor a 1 (si llegamos a usar cosas de aumentar la velocidad de movimiento)
    /// </summary>
    public float MovementMultiplier
    {
        get { return Mathf.Clamp01(_movementMultiplier); }
        set { _movementMultiplier = Mathf.Clamp01(value); }
    }

    float _movementMultiplier = 1;*/
    #endregion

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        /*_coll = GetComponent<Collider>();*/
        _nav = GetComponent<NavMeshAgent>();
    }

    void RecastNavMeshAgent(Vector3 spawnPos)
    {
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(spawnPos, out closestHit, 500, 1))
        {
            transform.position = closestHit.position;
        }
    }

    void ForceInitialPositionOnNavMesh(Vector3 spawnPos)
    {
        NavMeshHit closestHit;

        if (NavMesh.SamplePosition(spawnPos, out closestHit, 500f, NavMesh.AllAreas))
            gameObject.transform.position = closestHit.position;
    }

    void WarpNavMeshAgent(Vector3 pos)
    {
        _nav.Warp(pos);
    }

    public DMM_ScramblerMine Spawn(Vector3 spawnPos, Vector3 fwd, float duration, float explosionRadius, string emmitter)
    {
        transform.position = spawnPos;

        transform.forward = fwd;
        transform.parent = null;
        gameObject.tag = emmitter;
        _activated = false;

        _hp = maxHP;

        //_coll.isTrigger = true;

        _duration = duration;
        _explosionRadius = explosionRadius;

        _nav.ResetPath();
        _nav.speed = speed;

        WarpNavMeshAgent(spawnPos);

        Plant();

        return this;
    }

    public static void Initialize(DMM_ScramblerMine obj)
    {
        obj.gameObject.SetActive(true);
    }

    public static void Dispose(DMM_ScramblerMine ojb)
    {
        ojb.gameObject.SetActive(false);
    }

    void Plant()
    {
        //Do planting

        StartCoroutine(DelayedActivation());
    }

    IEnumerator DelayedActivation()
    {
        yield return new WaitForSeconds(activationDelay);
        Activate();
    }

    void Activate()
    {
        _activated = true;
    }

    Player GetTarget()
    {
        return GameManager.Instance.Players.Where(x => x.gameObject.tag != gameObject.tag).Aggregate(
            (accum, current) =>
            {
                var currentDistance = Vector3.Distance(current.transform.position, transform.position);
                var accumDistance = Vector3.Distance(accum.transform.position, transform.position);
                accum = currentDistance > accumDistance ? current : accum;
                return accum;
            });
    }

    void HuntTarget()
    {
        if (!_target) _target = GetTarget();
        if (!_target) return;
        _nav.SetDestination(_target.transform.position);
    }

    void Update()
    {
        if (_activated /* && _hasTarget */)
        {
            HuntTarget();
        }
    }

    void TakeDamage(float dmg)
    {
        _hp -= dmg;
        if (_hp <= 0)
        {
            Explode(true);
        }
    }

    void Explode(bool affectsUser)
    {
        var players = Physics.OverlapSphere(transform.position, _explosionRadius).Select(x => x.GetComponent<Player>()).Where(x => x != null);

        if (!affectsUser) players = players.Where(x => x.tag != gameObject.tag);

        foreach (var pl in players)
        {
            pl.ApplyDisarm(_duration);
        }

        ReturnToPool();
    }

    void ReturnToPool()
    {
        ScramblerMineSpawner.Instance.ReturnToPool(this);
    }

    void OnTriggerEnter(Collider col)
    {
        var colTag = col.gameObject.tag;
        var playerTags = colTag == "Player 1" || colTag == "Player 2" || colTag == "Player 3" || colTag == "Player 4";
        var filterLauncher = colTag != gameObject.tag;

        if (playerTags && filterLauncher)
        {
            CollidedAgainstPlayerTag(col.gameObject);
        }

    }

    void CollidedAgainstPlayerTag(GameObject collidedObj)
    {
        if (collidedObj.GetComponent<Player>())
        {
            Explode(false);
        }
        else
        {
            Explode(true);
        }
    }

    void CollidedAgainstStateChanger()
    {

    }

    #region StateChangers -- TODO!

    /*public void ApplyNeutralizeMovement(float duration)
    {
        StartCoroutine(ExecuteNeutralizedMovement(duration));
    }
    public void ApplySlowMovement(float duration, float amount)
    {
        StartCoroutine(ExecuteSlowMovement(duration, amount));
    }
    IEnumerator ExecuteNeutralizedMovement(float duration)
    {
        _isUnableToMove = true;

        yield return new WaitForSeconds(duration);

        _isUnableToMove = false;
    }

    IEnumerator ExecuteSlowMovement(float duration, float amount)
    {
        MovementMultiplier = amount;

        yield return new WaitForSeconds(duration);

        MovementMultiplier = 1;
    }*/

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class DMM_ScramblerMine : MonoBehaviour, IDamageable
{
    Rigidbody _rb;
    Collider _coll;
    NavMeshAgent _nav;
    public float damage, activationDelay, speed, maxHP;

    float _duration, _explosionRadius, _hp;

    bool _activated, _hasTarget;

    Player _target;

    public float Hp
    {
        get
        {
            return _hp;
        }

        private set
        {
            _hp = value >= maxHP ? maxHP : value <= 0 ? 0 : value;
        }
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _coll = GetComponent<Collider>();
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

        ResetHP();

        _coll.isTrigger = true;

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
        return GameManager.Instance.Players.Where(x => x.gameObject.tag != gameObject.tag).OrderBy(a => Vector3.Distance(a.transform.position, transform.position)).First();
    }

    void HuntTarget()
    {
        if (!_target) _target = GetTarget();
        if (_target) _nav.SetDestination(_target.transform.position);
    }

    void Update()
    {
        if (_activated)
        {
            HuntTarget();
        }
    }

    public void Explode(bool affectsUser)
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

    public void ResetHP()
    {
        Hp = maxHP;
    }

    public void TakeDamage(float damage)
    {
        SubstractLife(damage);
        if (Hp <= 0) Explode(true);
    }

    public void TakeDamage(float damage, string killerTag)
    {
        SubstractLife(damage);
        if (Hp <= 0) Explode(true);
    }

    void SubstractLife(float damage)
    {
        Hp -= damage;
    }

    void Destruction()
    {
        ReturnToPool();
    }
}

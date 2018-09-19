using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    float maxLifeTime = 5f;
    float MaxLifeTime { get { return maxLifeTime; } }
    float lifeTime = 0f;
    public float Lifetime { set { lifeTime = value; } }
    float _travelledDistance;
    public float DistanceTravelled { set { _travelledDistance = value; } }
    AnimationCurve _damageCurve;
    AnimationCurve _knockbackCurve;
    Rigidbody _rb;
    Vector3 _initialPos;


    float _knockback;
    float _damage;
    public float Damage { get { return _damage; } }

    Player _owner;
    public Player Owner
    {
        get { return _owner; }
    }

    string[] _tagsToEvade;
    string[] _tagsToMatch;
    int[] _layersToEvade;
    int[] _layersToMatch;

    public Bullet Spawn(float speed, AnimationCurve dmgCurve, AnimationCurve knockCurve, Vector3 position, Quaternion rotation, string emitter)
    {
        this.speed = speed;
        _damageCurve = dmgCurve;
        _knockbackCurve = knockCurve;
        gameObject.tag = emitter;
        transform.position = position;
        _initialPos = position;

        transform.rotation = rotation;
        transform.parent = null;

        _rb = GetComponent<Rigidbody>();

        return this;
    }

    public Bullet Spawn(float speed, AnimationCurve dmgCurve, AnimationCurve knockCurve, Vector3 position, Quaternion rotation, string emitter, Player owner)
    {
        this.speed = speed;
        _damageCurve = dmgCurve;
        _knockbackCurve = knockCurve;

        gameObject.tag = emitter;
        transform.position = position;
        _initialPos = position;

        transform.rotation = rotation;
        transform.parent = null;
        _owner = owner;

        _rb = GetComponent<Rigidbody>();

        return this;
    }

    public Bullet Spawn(float speed, float damage, AnimationCurve knockCurve, Vector3 position, Quaternion rotation, string emitter, Player owner)
    {
        this.speed = speed;
        _damageCurve = new AnimationCurve(new Keyframe(0, damage));
        _knockbackCurve = knockCurve;

        gameObject.tag = emitter;
        transform.position = position;
        _initialPos = position;

        transform.rotation = rotation;
        transform.parent = null;
        _owner = owner;

        _rb = GetComponent<Rigidbody>();

        return this;
    }

    /// <summary>
    /// Item1: Player
    /// Item2: Knockback amount
    /// Item3: Initial Position
    /// </summary>
    /// <returns></returns>
    public Tuple<Player, float, Vector3> GetKnockbackInfo()
    {
        return new Tuple<Player, float, Vector3>(_owner, _knockback, _initialPos);
    }

    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxLifeTime)
        {
            BulletSpawner.Instance.ReturnToPool(this);
        }
    }

    void FixedUpdate()
    {
        if (lifeTime < maxLifeTime)
        {
            var distance = speed * Time.fixedDeltaTime;
            _travelledDistance += distance;
            _rb.MovePosition(_rb.position + transform.forward * distance);
            _damage = GetActualDamage(_travelledDistance);
            _knockback = GetActualKnockback(_travelledDistance);
            //Acá tendrías que setear el daño actual al análisis del falloff de acuerdo al tiempo
            //que haya transcurrido dividido por el tiempo que se tardaría en completar el falloff.
            //Para esto tendrías que pedir velocidad, daño max y min, posición, rotación, emitter, curva de falloff y tiempo de duración del mismo al instanciar la bala.
        }
    }

    float GetActualDamage(float valueToAnalyze)
    {
        return _damageCurve.Evaluate(valueToAnalyze);
    }

    float GetActualKnockback(float valueToAnalyze)
    {
        return _knockbackCurve.Evaluate(valueToAnalyze);
    }

    public static void Initialize(Bullet bulletObj)
    {
        bulletObj.gameObject.SetActive(true);
        bulletObj.Lifetime = 0f;
        bulletObj.DistanceTravelled = 0f;
    }

    public static void Dispose(Bullet bulletObj)
    {
        bulletObj.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("DestructibleWall"), LayerMask.NameToLayer("DestructibleStructure")))
        {
            col.gameObject.SetActive(false);
            BulletSpawner.Instance.ReturnToPool(this);
        }
        else if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("Shield")))
        {
            if (col.gameObject.GetComponentInParent<Player>().gameObject.TagMatchesWith(this.gameObject.tag)) return;
            BulletSpawner.Instance.ReturnToPool(this);
        }
        else if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Antenna")))
            BulletSpawner.Instance.ReturnToPool(this);
    }
}

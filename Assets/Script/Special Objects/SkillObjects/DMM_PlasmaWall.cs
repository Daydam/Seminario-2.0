using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DMM_PlasmaWall : MonoBehaviour, IDamageable
{
    public float maxHP = 5;
    public float lifeTime = 5f;

    Coroutine _lifeTimerRoutine;
    Renderer _rend;

    float _hp;
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

    public DMM_PlasmaWall Spawn(Vector3 spawnPos, Vector3 fwd)
    {
        transform.position = spawnPos;
        transform.forward = fwd;
        transform.parent = null;

        ResetHP();

        _lifeTimerRoutine = StartCoroutine(LifeTimer());

        return this;
    }

    public static void Initialize(DMM_PlasmaWall obj)
    {
        obj.gameObject.SetActive(true);
    }

    public static void Dispose(DMM_PlasmaWall ojb)
    {
        ojb.gameObject.SetActive(false);
    }

    void ReturnToPool()
    {
        PlasmaWallSpawner.Instance.ReturnToPool(this);
    }

    IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);

        Destruction();
    }

    public void ResetHP()
    {
        Hp = maxHP;
        if (_rend == null) _rend = GetComponent<Renderer>();
        _rend.material.SetFloat("_Life", 1);
    }

    public void TakeDamage(float damage)
    {
        SubstractLife(damage);
        if (Hp <= 0) Destruction();
    }

    public void TakeDamage(float damage, string killerTag)
    {
        SubstractLife(damage);
        if (Hp <= 0) Destruction();
    }

    void SubstractLife(float damage)
    {
        Hp -= damage;
        _rend.material.SetFloat("_Life", Mathf.Lerp(0, 1, Hp / maxHP));
    }

    void Destruction()
    {
        StopCoroutine(_lifeTimerRoutine);
        _lifeTimerRoutine = null;
        ReturnToPool();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DMM_PlasmaWall : MonoBehaviour, IDamageable
{
    public float maxHP = 5;
    public float lifeTime = 5f;

    Coroutine _lifeTimerRoutine;

    float hp;
    public float Hp
    {
        get
        {
            return hp;
        }

        private set
        {
            hp = value >= maxHP ? maxHP : value <= 0 ? 0 : value;
        }
    }

    public DMM_PlasmaWall Spawn(Vector3 spawnPos, Vector3 fwd)
    {
        transform.position = spawnPos;
        transform.forward = fwd;
        transform.parent = null;

        _lifeTimerRoutine = StartCoroutine(LifeTimer());

        return this;
    }

    public static void Initialize(DMM_PlasmaWall obj)
    {
        obj.gameObject.SetActive(true);
        obj.ResetHP();
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
    }

    void Destruction()
    {
        StopCoroutine(_lifeTimerRoutine);
        _lifeTimerRoutine = null;
        ReturnToPool();
    }
}

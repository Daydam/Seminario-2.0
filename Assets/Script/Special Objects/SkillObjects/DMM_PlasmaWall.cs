using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DMM_PlasmaWall : MonoBehaviour, IDamageable
{
    public SO_PlasmaWall skillData;

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
            _hp = value >= skillData.maxHP ? skillData.maxHP : value <= 0 ? 0 : value;
        }
    }

    public DMM_PlasmaWall Spawn(Vector3 spawnPos, Vector3 fwd, float size, SO_PlasmaWall data)
    {
        skillData = data;

        transform.position = spawnPos;
        transform.forward = fwd;
        transform.parent = null;
        transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);

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
        yield return new WaitForSeconds(skillData.lifeTime);

        Destruction();
    }

    public void ResetHP()
    {
        Hp = skillData.maxHP;
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
        _rend.material.SetFloat("_Life", Mathf.Lerp(0, 1, Hp / skillData.maxHP));
    }

    void Destruction()
    {
        StopCoroutine(_lifeTimerRoutine);
        _lifeTimerRoutine = null;
        ReturnToPool();
    }

    public IDamageable GetThisEntity()
    {
        return this;
    }
}

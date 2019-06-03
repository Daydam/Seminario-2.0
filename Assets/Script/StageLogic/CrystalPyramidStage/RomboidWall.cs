using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Firepower.Events;

public class RomboidWall : MonoBehaviour, IDamageable
{
    Collider col;
    GameObject _baseObj;
    Animator _destructibleObj;
    GameObject[] _destroyedFragments;

    public int normalSpeedFrame = 20;

    public float maxHP = 5, normalAnimSpeed = .8f, slowAnimSpeed = .5f, fragmentMaxLifeTime = 10f, fragmentDisableDelay = 2f;
    private float hp;
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

    void Awake()
    {
        _baseObj = transform.parent.Find("BaseObj").gameObject;
        _destructibleObj = transform.parent.GetComponentInChildren<Animator>(true);
        var transformNode = _destructibleObj.transform.Find("TRANSFORM_NODE");
        _destroyedFragments = transformNode.GetComponentsInChildren<Transform>().Where(x => x != transformNode).Select(x => x.gameObject).ToArray();
        col = GetComponent<Collider>();
    }

    void Start()
    {
        ResetHP();
        EventManager.Instance.AddEventListener(GameEvents.RoundReset, ResetHP);
        EventManager.Instance.AddEventListener(CrystalPyramidEvents.DestructibleWallDestroyEnd, OnDestroyAnimationEnd);
    }

    public void ResetHP()
    {
        StopAllCoroutines();
        Hp = maxHP;
        col.enabled = true;
        _baseObj.SetActive(true);
        foreach (var item in _destroyedFragments) item.SetActive(true);
        _destructibleObj.gameObject.SetActive(false);
    }

    public void ResetHP(params object[] info)
    {
        ResetHP();
    }

    public void TakeDamage(float damage)
    {
        SubstractLife(damage);

        if (Hp <= 0) Death();
    }

    public void TakeDamage(float damage, string killerTag)
    {
        SubstractLife(damage);
        if (Hp <= 0) Death();
    }

    void Death()
    {
        _baseObj.SetActive(false);
        _destructibleObj.gameObject.SetActive(true);
        _destructibleObj.Play("Destroy");
        col.enabled = false;
    }

    void SubstractLife(float damage)
    {
        Hp -= damage;
    }

    public void SetDestrucibleAnimSpeed(bool fast)
    {
        _destructibleObj.SetFloat("animSpeed", fast ? normalAnimSpeed : slowAnimSpeed);
    }

    public void PlayBulletHitSound()
    {
        //No parece quedar bien
        //  NO QUEDA BIEEEN
        // _source.PlayOneShot(bulletHit);
    }

    public IDamageable GetThisEntity()
    {
        return this;
    }

    public void OnDestroyAnimationEnd(params object[] parameters)
    {
        var sender = (Animator)parameters[0];

        if (sender == _destructibleObj)
        {
            StartCoroutine(DisableFragments());
        }
    }

    IEnumerator DisableFragments()
    {
        yield return new WaitForSeconds(fragmentDisableDelay);

        float tick = _destroyedFragments.Length / fragmentMaxLifeTime;
        var inst = new WaitForSeconds(tick);

        foreach (var item in _destroyedFragments)
        {
            item.SetActive(false);

            yield return inst;
        }
    }
}

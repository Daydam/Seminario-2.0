using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Firepower.Events;

public class RomboidWall : MonoBehaviour, IDamageable
{
    Collider col;
    Renderer _baseObj;
    Animator _destructibleObj;

    public int normalSpeedFrame = 20;

    public float maxHP = 5, normalAnimSpeed = .8f, slowAnimSpeed = .5f;
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
        _baseObj = GetComponent<Renderer>();
        _destructibleObj = GetComponentInChildren<Animator>();
        col = GetComponent<Collider>();
    }

    void Start()
    {
        ResetHP();
        EventManager.Instance.AddEventListener(GameEvents.RoundReset, ResetHP);
    }

    public void ResetHP()
    {
        StopAllCoroutines();
        Hp = maxHP;
        col.enabled = true;
        _baseObj.enabled = true;
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
        _baseObj.enabled = false;
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

}

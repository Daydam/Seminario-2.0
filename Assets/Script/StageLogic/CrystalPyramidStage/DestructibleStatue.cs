using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Firepower.Events;

public class DestructibleStatue : MonoBehaviour, IDamageable
{
    Collider col;
    GameObject _baseObj;
    Animator _destructibleObj;

    public int normalSpeedFrame;

    public float maxHP = 5, normalAnimSpeed = 1, slowAnimSpeed = .7f;
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
        var obj = GetComponentsInChildren<Transform>(true).Where(x => x.name != "PedestalStatue");
        foreach (var x in obj)
        {
            var comp = x.GetComponent<Animator>();
            if (comp != null) _destructibleObj = comp;
            else if(x.gameObject.activeInHierarchy)
            {
                _baseObj = x.gameObject;
            }
        }
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
        _baseObj.SetActive(true);
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
        SetDestrucibleAnimSpeed(false);
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

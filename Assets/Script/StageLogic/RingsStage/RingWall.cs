using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Firepower.Events;

public class RingWall : DestructibleBase, IDamageable
{
    public AudioClip bulletHit;
    Material[] dissolveMaterials;
    Collider col;
    //AudioSource _source;

    readonly string _shaderTag = "Dissolver", _tagValue = "Nothing";
    
    public float maxHP = 5;
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

    void Start()
    {
        //_source = GetComponent<AudioSource>();
        dissolveMaterials = GetComponentsInChildren<Renderer>().SelectMany(x => x.materials).ToArray();
        SetDissolve(0);
        col = GetComponent<Collider>();
        ResetHP();
        EventManager.Instance.AddEventListener(GameEvents.RoundReset, ResetHP);
    }

    public override void ResetHP()
    {
        StopAllCoroutines();
        Hp = maxHP;
        SetDissolve(0);
        col.enabled = true;
    }

    public override void ResetHP(params object[] info)
    {
        StopAllCoroutines();
        Hp = maxHP;
        SetDissolve(0);
        col.enabled = true;
    }

    public override void TakeDamage(float damage)
    {
        SubstractLife(damage);

        if (Hp <= 0) Death();
    }

    public override void TakeDamage(float damage, string killerTag)
    {
        SubstractLife(damage);
        if (Hp <= 0) Death();
    }

    public override void TakeDamage(float damage, Vector3 hitPosition)
    {
        SubstractLife(damage);
        if (Hp <= 0) Death();
    }

    public override void TakeDamage(float damage, string killerTag, Vector3 hitPosition)
    {
        SubstractLife(damage);
        if (Hp <= 0) Death();
    }
    void SetDissolve(float val)
    {
        foreach (var item in dissolveMaterials)
        {
            item.SetFloat("_Dissolved", val);
        }
    }

    protected override void Death()
    {
        //gameObject.SetActive(false);
        StartCoroutine(DestroyRingWall());
    }

    protected override void SubstractLife(float damage)
    {
        Hp -= damage;
    }

    public void PlayBulletHitSound()
    {
        //No parece quedar bien
        //  NO QUEDA BIEEEN
       // _source.PlayOneShot(bulletHit);
    }

    IEnumerator DestroyRingWall()
    {
        col.enabled = false;
        float dissolveAmount = 0f;
        while (dissolveAmount < 1)
        {
            dissolveAmount += Time.deltaTime/2;
            SetDissolve(dissolveAmount);
            yield return new WaitForEndOfFrame();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Firepower.Events;

public class RingWall : MonoBehaviour, IDamageable
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

    public void ResetHP()
    {
        StopAllCoroutines();
        Hp = maxHP;
        SetDissolve(0);
        col.enabled = true;
    }

    public void ResetHP(params object[] info)
    {
        StopAllCoroutines();
        Hp = maxHP;
        SetDissolve(0);
        col.enabled = true;
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

    void SetDissolve(float val)
    {
        foreach (var item in dissolveMaterials)
        {
            item.SetFloat("_Dissolved", val);
        }
    }

    void Death()
    {
        //gameObject.SetActive(false);
        StartCoroutine(DestroyRingWall());
    }

    void SubstractLife(float damage)
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

    public IDamageable GetThisEntity()
    {
        return this;
    }

}

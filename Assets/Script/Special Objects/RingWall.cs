using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class RingWall : MonoBehaviour, IDamageable
{
    public AudioClip bulletHit;
    AudioSource _source;

    public float maxHP = 5;
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

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        ResetHP();
    }

    public void ResetHP()
    {
        Hp = maxHP;
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
        gameObject.SetActive(false);
    }

    void SubstractLife(float damage)
    {
        Hp -= damage;
    }

    public void PlayBulletHitSound()
    {
        _source.PlayOneShot(bulletHit);
    }
}

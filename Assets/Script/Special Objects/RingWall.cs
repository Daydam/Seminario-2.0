using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RingWall : MonoBehaviour, IDamageable
{
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

        if (Hp <= 0) gameObject.SetActive(false);
    }

    public void TakeDamage(float damage, string killerTag)
    {
        SubstractLife(damage);
        if (Hp <= 0) gameObject.SetActive(false);
    }

    void SubstractLife(float damage)
    {
        Hp -= damage;
    }
}

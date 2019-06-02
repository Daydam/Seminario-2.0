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
    GameObject _forceScale;

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
        _baseObj = transform.parent.Find("BaseObj").gameObject;
        _destructibleObj = transform.parent.GetComponentInChildren<Animator>(true);

        //Hardcode porque Alembic resulta que me achica el objeto cada vez que se prende
        var parentOfForced = _destructibleObj.GetComponentInChildren<UTJ.Alembic.AlembicStreamPlayer>();
        _forceScale = parentOfForced.transform.GetChild(0).gameObject;// Find("CrystalPyramidDestructibles_FBX").gameObject;

        col = GetComponent<Collider>();
    }

    void Start()
    {
        ResetHP();
        EventManager.Instance.AddEventListener(GameEvents.RoundReset, ResetHP);
    }

    void Update()
    {
        _forceScale.transform.localScale = Vector3.one;
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
        _destructibleObj.Play("Destroy");
        col.enabled = false;
        _forceScale.transform.localScale = Vector3.one;
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

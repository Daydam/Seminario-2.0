using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Firepower.Events;

public class RomboidWall : DestructibleBase, IDamageable
{
    Collider col;
    GameObject _baseObj;
    Animator _destructibleObj;
    GameObject[] _destroyedFragments;

    //private Pool<GameObject> _poolDustParticle;
    //private GameObject _dustParent;

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
        //_dustParent = GameObject.Find("AllParticles_Dust_Wall_Destruction");
        //_poolDustParticle = new Pool<GameObject>(10, Factory, TurnOnObject, TurnOffObject, true);
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

    public override void ResetHP()
    {
        StopAllCoroutines();
        Hp = maxHP;
        col.enabled = true;
        _baseObj.SetActive(true);
        foreach (var item in _destroyedFragments) item.SetActive(true);
        _destructibleObj.gameObject.SetActive(false);
    }

    public override void ResetHP(params object[] info)
    {
        ResetHP();
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

    protected override void Death()
    {
        SimpleParticleSpawner.Instance.SpawnDust(SimpleParticleSpawner.ParticleID.DUST_WALL_DESTRUCTION, transform.position);
        _baseObj.SetActive(false);
        _destructibleObj.gameObject.SetActive(true);
        _destructibleObj.Play("Destroy");
        col.enabled = false;
    }

    protected override void SubstractLife(float damage)
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

    //Seguir después: Hacer andar el pool.
    /*private GameObject Factory()
    {
        var go = Instantiate<GameObject>(SimpleParticleSpawner.Instance.GetParticleByID(SimpleParticleSpawner.ParticleID.DUST_WALL_DESTRUCTION));
        go.transform.parent = _dustParent.transform;
        return go;
    }

    private void TurnOnObject(GameObject go)
    {
        go.SetActive(true);
    }

    private void TurnOffObject(GameObject go)
    {
        go.SetActive(false);
    }*/
}

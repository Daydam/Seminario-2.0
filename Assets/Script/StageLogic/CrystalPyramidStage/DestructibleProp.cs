using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Firepower.Events;
using Firepower;
using PhoenixDevelopment;

public class DestructibleProp : DestructibleBase, IDamageable
{
    public AudioClip bulletHit;
    AudioSource _source;

    Collider col;
    GameObject _baseObj;
    Animator _destructibleObj;
    GameObject[] _destroyedFragments;

    public float fragmentMaxLifeTime = 6f, fragmentDisableDelay = 2f;

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
        _source = GetComponent<AudioSource>();
        if (_source == null) _source = gameObject.AddComponent<AudioSource>();

        ResetHP();

        EventManager.Instance.AddEventListener(GameEvents.RoundReset, ResetHP);
        EventManager.Instance.AddEventListener(CrystalPyramidEvents.DestructibleWallDestroyEnd, OnDestroyAnimationEnd);
    }

    public override void ResetHP()
    {
        StopAllCoroutines();
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
        Death();
    }

    public override void TakeDamage(float damage, string killerTag)
    {
        Death();
    }

    public override void TakeDamage(float damage, Vector3 hitPosition)
    {
        Death();
    }

    public override void TakeDamage(float damage, string killerTag, Vector3 hitPosition)
    {
        Death();
    }

    protected override void Death()
    {
        _baseObj.SetActive(false);
        _destructibleObj.gameObject.SetActive(true);
        _destructibleObj.Play("Destroy");
        col.enabled = false;
    }

    protected override void SubstractLife(float damage)
    {

    }

    public override void PlayBulletHitSound(float volume, float pitch)
    {
        _source.pitch = pitch;
        _source.PlayOneShot(bulletHit, volume);
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.LayerMatchesWith("Player1","Player2","Player3","Player4"))
        {
            TakeDamage(1);
        }
    }
}
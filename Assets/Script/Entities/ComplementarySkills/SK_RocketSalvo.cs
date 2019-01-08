using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_RocketSalvo : ComplementarySkillBase
{
    public int rocketCount = 25;
    public float maxCooldown = 7, duration = 3, effectRadius = 2.5f, rocketCooldown = .12f;

    float _currentCooldown = 0;
    bool _canTap = true;
    bool _skillActive = false;

    SphereCollider _effectArea;

    List<DMM_RocketMini> _rockets = new List<DMM_RocketMini>();

    protected override void Start()
    {
        base.Start();

        LoadPrefabs();
    }

    void OnEnable()
    {
        if (_effectArea == null)
        {
            _effectArea = GetComponentsInChildren<SphereCollider>(true).Where(x => x.gameObject.name == "EffectArea").First();
            _effectArea.transform.localScale = new Vector3(effectRadius, 0, effectRadius);
            //concha en el medio del código como pidió santi
        }

        SetCollisionsIgnored();
    }

    void SetCollisionsIgnored()
    {
        var list = GameObject.FindGameObjectsWithTag("DeathZone").Select(x => x.GetComponent<Collider>()).Where(x => x != null).ToFList();
        list += StageManager.instance.empCloud.col;

        foreach (var item in list)
        {
            Physics.IgnoreCollision(_effectArea, item);
        }
    }

    void LoadPrefabs()
    {
        var loadedPrefab = Resources.Load<DMM_RocketMini>("Prefabs/Projectiles/RocketMini");

        for (byte i = 0; i < rocketCount; i++)
        {
            var rocket = Instantiate(loadedPrefab);
            _rockets.Add(rocket);
            rocket.gameObject.SetActive(false);
        }

        _effectArea = GetComponentsInChildren<SphereCollider>(true).Where(x => x.gameObject.name == "EffectArea").First();
        _effectArea.transform.localScale = new Vector3(effectRadius, 0, effectRadius);
        SetCollisionsIgnored();
    }

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && !_skillActive && !_owner.lockedByGame && _currentCooldown <= 0;
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (inputMethod() && _canUseSkill())
        {
            ActivateSalvo();
        }
    }

    void ActivateSalvo()
    {
        _owner.ApplyDisarm(duration);

        StartCoroutine(FireRockets());
    }

    IEnumerator FireRockets()
    {
        var inst = new WaitForSeconds(rocketCooldown);

        int rocketsFired = 0;
        float timeElapsed = 0f;

        while (rocketsFired < rocketCount - 1)
        {
            timeElapsed += rocketCooldown;

            _rockets[rocketsFired].gameObject.SetActive(true);

            var bounds = _effectArea.bounds;

            var rndX = Random.Range(bounds.min.x, bounds.max.x);
            var rndZ = Random.Range(bounds.min.z, bounds.max.z);

            var rocketLandingPoint = new Vector3(rndX, 0, rndZ);

            _rockets[rocketsFired].Spawn(transform.position, rocketLandingPoint, _owner.tag, _owner, OnRocketActivation);

            rocketsFired++;

            yield return inst;
        }

        //set player state enabled
    }

    public override void ResetRound()
    {
        StopAllCoroutines();

        foreach (var item in _rockets)
        {
            item.gameObject.SetActive(false);
        }

        _skillActive = false;
        _currentCooldown = 0;
    }

    void OnRocketActivation(DMM_RocketMini obj)
    {
        obj.gameObject.SetActive(false);
    }

    public override SkillState GetActualState()
    {
        var unavailable = _currentCooldown > 0;
        var userDisabled = _owner.IsStunned || _owner.IsDisarmed;

        if (userDisabled) return SkillState.UserDisabled;
        else if (unavailable) return SkillState.Unavailable;
        else if (_skillActive) return SkillState.Active;
        else return SkillState.Available;
    }
}

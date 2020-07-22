using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_ImplosiveCharge : ComplementarySkillBase
{
    public SO_ImplosiveCharge skillData;

    ModuleParticleController _particleModule;

    float _currentCooldown;
    bool _skillActive, _canTap = true;

    readonly float _debugMaxTimeOfCast = 2f;

    DMM_ImplosiveCharge _charge;

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && !_owner.lockedByGame && _currentCooldown <= 0;
    }

    protected override void Start()
    {
        base.Start();
        _particleModule = GetComponent<ModuleParticleController>();

        if(_owner != null) skillData = Resources.Load<SO_ImplosiveCharge>("Scriptable Objects/Skills/Complementary/" + _owner.weightModule.prefix + GetSkillName() + _owner.weightModule.sufix) as SO_ImplosiveCharge;

        var loadedPrefab = Resources.Load<DMM_ImplosiveCharge>("Prefabs/Projectiles/ImplosiveCharge");

        _charge = Instantiate(loadedPrefab);
        _charge.gameObject.SetActive(false);
    }
    public override void RegisterInput()
    {
        base.RegisterInput();
        skillData = Resources.Load<SO_ImplosiveCharge>("Scriptable Objects/Skills/Complementary/" + _owner.weightModule.prefix + GetSkillName() + _owner.weightModule.sufix) as SO_ImplosiveCharge;
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (inputMethod())
        {
            if (_canUseSkill() && _canTap)
            {
                if (!_skillActive) ShootProyectile();
                else ForceActivation();
                _canTap = false;
            }
            //else _stateSource.PlayOneShot(unavailableSound);
        }
        else _canTap = true;
    }

    public void ShootProyectile()
    {
        StartCoroutine(ChargeActivationHandler());
    }

    IEnumerator ChargeActivationHandler()
    {
        _particleModule.OnShoot();
        StartCoroutine(ApplyReadyParticles());

        _charge.gameObject.SetActive(true);
        _charge.Spawn(_owner.transform.position, _owner.transform.forward, _owner.tag, _owner, skillData);
        _skillActive = true;

        yield return new WaitUntil(() => _charge.movementFinished);

        if (_charge.targets.Any())
        {
            foreach (var player in _charge.targets)
            {
                StartCoroutine(SuckPlayer(player, _charge.transform.position));
            }
        }

        _charge.gameObject.SetActive(false);
        _skillActive = false;
        _currentCooldown = skillData.maxCooldown;
        NotifyUIModule();
    }

    public IEnumerator ApplyReadyParticles()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            if (GetActualState() != SkillState.Available)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                _particleModule.OnAvailableShot();
                yield break;
            }
        }
    }

    /// <summary>
    /// Como tu vieja
    /// </summary>
    IEnumerator SuckPlayer(Player player, Vector3 landingPoint)
    {
        player.CancelForces();
        var endCast = false;

        var debugTime = 0f;

        player.ApplyStun(() => endCast);

        var startPoint = player.transform.position;

        var tTick = Time.fixedDeltaTime / skillData.playerTravelTime;
        var elapsed = 0f;

        while (Vector3.Distance(player.transform.position, landingPoint) > .3f)
        {
            elapsed += tTick;

            var nextPos = Vector3.Lerp(startPoint, landingPoint, elapsed);

            player.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();

            debugTime += Time.fixedDeltaTime;
            if (debugTime >= _debugMaxTimeOfCast)
            {
                endCast = true;
                _skillActive = false;
                _canTap = false;
                yield break;
            }
        }

        endCast = true;
        _skillActive = false;
        _canTap = false;
    }

    public void ForceActivation()
    {
        _charge.ForceActivation();
    }

    public override void ResetRound()
    {
        StopAllCoroutines();
        _charge.gameObject.SetActive(false);
        _skillActive = false;
        _currentCooldown = 0;
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

    public override float GetCooldownPerc()
    {
        return Mathf.Lerp(1, 0, _currentCooldown / skillData.maxCooldown);
    }
}

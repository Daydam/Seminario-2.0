using Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public float turningSpeed;

    public DroneWeightModule weightModule;
    public int Weight
    {
        get { return weightModule.weight; }
    }

    Controller _control;
    public Controller Control { get { return _control; } }

    public float movementSpeed;
    public Vector3 movDir;
    public GameObject playerEndgameTexts;
    public GameObject playerUI;

    CameraShake _camShake;
    DroneSoundController _soundModule;
    Collider _col;

    Coroutine _actualPushCouroutine;

    public bool lockedByGame;

    Rigidbody _rb;
    public Rigidbody GetRigidbody { get { return _rb; } }

    PlayerAnimations _animationController;
    PlayerLifeForcefield _lifeForcefield;
    PlayerScoreController _scoreController;
    public PlayerScoreController ScoreController
    {
        get
        {
            return _scoreController;
        }

        private set
        {
            _scoreController = value;
        }
    }

    public int myID;

    bool _vibrationAvailable = true;

    bool _isStunned;
    bool _isDisarmed;
    bool _isUnableToMove;
    bool _isCasting;
    bool _invulnerable;
    public bool IsStunned { get { return _isStunned; } }
    public bool IsDisarmed { get { return _isDisarmed; } }
    public bool IsUnableToMove { get { return _isUnableToMove; } }
    public bool IsCasting { get { return _isCasting; } }
    public bool Invulnerable { get { return _invulnerable; } }

    Renderer _rend;
    public Renderer Rend { get { return _rend; } }

    float _movementMultiplier = 1;
    public float MovementMultiplier
    {
        get { return Mathf.Clamp01(_movementMultiplier); }
        set { _movementMultiplier = Mathf.Clamp01(value); }
    }

    float _knockbackMultiplier = 1;
    public float KnockbackMultiplier
    {
        get { return Mathf.Abs(_knockbackMultiplier); }
        set { _knockbackMultiplier = Mathf.Abs(value); }
    }

    public float maxHP = 100;
    float _hp;
    public float Hp
    {
        get
        {
            return _hp;
        }

        private set
        {
            _hp = value >= maxHP ? maxHP : value <= 0 ? 0 : value;
        }
    }

    public bool isPushed;
    public Player myPusher;
    PlayerStats _stats;
    public float pushTimeCheck = 2;

    public PlayerStats Stats
    {
        get { return _stats; }
        set { _stats = value; }
    }

    CamFollow _cam;
    public CamFollow Cam
    {
        get
        {
            return _cam;
        }

        private set
        {
            _cam = value;
        }
    }

    void Awake()
    {
        int playerID = GameManager.Instance.Register(this);
        myID = playerID;
        _control = new Controller(playerID);
        _rb = GetComponent<Rigidbody>();
        _rend = GetComponentInChildren<Renderer>();
        MovementMultiplier = 1;
        Hp = maxHP;
        gameObject.name = "Player " + (playerID + 1);

        _stats = new PlayerStats();

        ScoreController = GetComponent<PlayerScoreController>();
        _soundModule = GetComponent<DroneSoundController>();
        _animationController = GetComponent<PlayerAnimations>();
        _lifeForcefield = GetComponentInChildren<PlayerLifeForcefield>();
        _col = GetComponent<Collider>();
        weightModule = GetComponent<DroneWeightModule>();

    }

    void Start()
    {
        GameManager.Instance.StartRound += () => lockedByGame = false;
        GameManager.Instance.OnResetRound += StopVibrating;
        GameManager.Instance.OnResetRound += ResetRound;
        GameManager.Instance.OnChangeScene += StopVibrating;
    }

    void Update()
    {
        if (lockedByGame) return;

        _control.UpdateState();

        if (!IsStunned && _control.RightAnalog() != Vector2.zero)
        {
            transform.Rotate(transform.up, _control.RightAnalog().x * turningSpeed);
        }
    }

    public void AssignCamera(CamFollow cam)
    {
        Cam = cam;
        _camShake = cam.GetComponent<CameraShake>();
        GetComponent<PlayerSightingHandler>().Init();
    }

    public void DeactivateCamera()
    {
        Cam.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("DeathZone")))
        {
            if (isPushed && myPusher != null)
            {
                DestroyPlayer(DeathType.LaserGrid, myPusher.gameObject.tag);
            }
            else DestroyPlayer(DeathType.LaserGrid);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("EMPCloud")))
        {
            if (isPushed && myPusher != null)
            {
                DestroyPlayer(DeathType.LaserGrid, myPusher.gameObject.tag);
            }
            else DestroyPlayer(DeathType.LaserGrid);
        }
    }

    public void UpdateScore(int points)
    {
        if (points == 0) return;

        var prefix = points < 0 ? "- " : "+ ";


        if ((_stats.Score + points) >= GameManager.Instance.GetScoreToWin())
        {
            _stats.Score = GameManager.Instance.GetScoreToWin();
        }
        else
        {
            _stats.Score += points;
            _stats.Score = Mathf.Clamp(_stats.Score, 0, GameManager.Instance.GetScoreToWin());
        }

        ScoreController.SetScore(_stats.Score, points);
    }

    public void ResetHP()
    {
        Hp = maxHP;
        _rend.material.SetFloat("_Life", Hp / maxHP);
    }

    public void ResetRound()
    {
        StopAllCoroutines();
        StopVibrating();
        _isStunned = false;
        _isDisarmed = false;
        _isUnableToMove = false;
        _isCasting = false;
        _invulnerable = false;
        lockedByGame = true;
        _movementMultiplier = 1;
        _vibrationAvailable = true;
        isPushed = false;
        myPusher = null;
        CancelForces();
        StopAllCoroutines();
    }

    public void ActivatePlayerEndgame(bool activate, string replaceName, string replaceScore)
    {
        playerEndgameTexts.SetActive(activate);
        var tx = playerEndgameTexts.GetComponentInChildren<UnityEngine.UI.Text>();
        tx.text = gameObject.name + "\n" + _stats.Score.ToString();
    }

    public void ActivatePlayerEndgame(bool activate = false)
    {
        playerEndgameTexts.SetActive(activate);
    }

    void DestroyPlayer(DeathType type)
    {
        StopVibrating();
        _soundModule.PlayDeathSound();

        var deathPartID = SimpleParticleSpawner.ParticleID.DEATHPARTICLE;
        var deathParticle = SimpleParticleSpawner.Instance.particles[deathPartID].GetComponentInChildren<ParticleSystem>();
        SimpleParticleSpawner.Instance.SpawnParticle(deathParticle.gameObject, transform.position, transform.forward, transform);

        EventManager.Instance.DispatchEvent(PlayerEvents.Death, this, type, isPushed, gameObject.tag);
        _rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    void DestroyPlayer(DeathType type, string killerTag)
    {
        StopVibrating();
        _soundModule.PlayDeathSound();
        var deathPartID = SimpleParticleSpawner.ParticleID.DEATHPARTICLE;
        var deathParticle = SimpleParticleSpawner.Instance.particles[deathPartID].GetComponentInChildren<ParticleSystem>();
        SimpleParticleSpawner.Instance.SpawnParticle(deathParticle.gameObject, transform.position, transform.forward);

        EventManager.Instance.DispatchEvent(PlayerEvents.Death, this, type, isPushed, killerTag);
        _rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (!IsStunned && !IsUnableToMove && !lockedByGame && !IsCasting)
        {
            Movement();
        }
    }

    void Movement()
    {
        var dir = transform.forward * _control.LeftAnalog().y + transform.right * _control.LeftAnalog().x;
        var movVector = _rb.position + dir.normalized * Time.fixedDeltaTime * movementSpeed * MovementMultiplier;
        movDir = dir;
        _rb.MovePosition(movVector);
        _animationController.SetMovementDir(_control.LeftAnalog());
        //_soundModule.SetEnginePitch((control.LeftAnalog().y + control.LeftAnalog().x)/2 * movementSpeed * MovementMultiplier);
    }

    public IDamageable GetEntityType()
    {
        return this;
    }

    public void RepairDrone(float heal)
    {
        AddLife(heal);
    }

    void AddLife(float heal)
    {
        if (_invulnerable) return;

        Hp += heal;
        _rend.material.SetFloat("_Life", Hp / maxHP);
    }

    public void TakeDamage(float damage)
    {
        SubstractLife(damage);
        if (Hp <= 0) DestroyPlayer(DeathType.Player);
    }

    public void TakeDamage(float damage, Vector3 hitPosition)
    {
        SubstractLife(damage, hitPosition);
        if (Hp <= 0) DestroyPlayer(DeathType.Player);
    }

    public void TakeDamage(float damage, string killerTag)
    {
        SubstractLife(damage);
        if (Hp <= 0) DestroyPlayer(DeathType.Player, killerTag);
    }

    public void TakeDamage(float damage, string killerTag, Vector3 hitPosition)
    {
        SubstractLife(damage, hitPosition);
        if (Hp <= 0) DestroyPlayer(DeathType.Player, killerTag);
    }

    void SubstractLife(float damage)
    {
        if (_invulnerable) return;
        if (_vibrationAvailable)
        {
            ApplyVibration(0, 2, 0.2f);
            _vibrationAvailable = false;
            StartCoroutine(VibrationCooldown());
        }

        Hp -= damage;
        _rend.material.SetFloat("_Life", Hp / maxHP);
        _lifeForcefield.TakeDamage();
    }

    void SubstractLife(float damage, Vector3 hitPosition)
    {
        if (_invulnerable) return;
        if (_vibrationAvailable)
        {
            ApplyVibration(0, 2, 0.2f);
            _vibrationAvailable = false;
            StartCoroutine(VibrationCooldown());
        }

        Hp -= damage;
        _rend.material.SetFloat("_Life", Hp / maxHP);
        _lifeForcefield.TakeDamage(hitPosition);
    }

    public void ApplyKnockback(float amount, Vector3 dir)
    {
        if (_invulnerable) return;
        CancelForces();
        _rb.AddForce(dir * (amount * KnockbackMultiplier), ForceMode.Impulse);
    }

    public void ApplyKnockback(float amount, Vector3 dir, Player pusher)
    {
        if (_invulnerable) return;

        CancelForces();
        _rb.AddForce(dir * (amount * KnockbackMultiplier), ForceMode.Impulse);

        if (_actualPushCouroutine != null) StopCoroutine(_actualPushCouroutine);

        if (gameObject.activeInHierarchy) _actualPushCouroutine = StartCoroutine(ApplyPush(pusher));
    }

    public void ApplyKnockback(float amount, Vector3 dir, float pushedTime, Player pusher)
    {
        if (_invulnerable) return;

        CancelForces();

        _rb.AddForce(dir * (amount * KnockbackMultiplier), ForceMode.Impulse);

        if (_actualPushCouroutine != null) StopCoroutine(_actualPushCouroutine);

        if (gameObject.activeInHierarchy) _actualPushCouroutine = StartCoroutine(ApplyPush(pushedTime, pusher));
    }

    public void ApplyExplosionForce(float amount, Vector3 position, float radius, float upwardsModifier = 0)
    {
        if (_invulnerable) return;

        CancelForces();

        _rb.AddExplosionForce(amount * KnockbackMultiplier, position, radius, upwardsModifier, ForceMode.VelocityChange);

    }

    public void ApplyExplosionForce(float amount, Vector3 position, float radius, Player pusher, float upwardsModifier = 0)
    {
        if (_invulnerable) return;

        CancelForces();

        _rb.AddExplosionForce(amount * KnockbackMultiplier, position, radius, upwardsModifier, ForceMode.VelocityChange);


        if (_actualPushCouroutine != null) StopCoroutine(_actualPushCouroutine);

        if (gameObject.activeInHierarchy) _actualPushCouroutine = StartCoroutine(ApplyPush(pusher));
    }

    public void ApplyExplosionForce(float amount, Vector3 position, float radius, float pushedTime, Player pusher, float upwardsModifier = 0)
    {
        if (_invulnerable) return;

        CancelForces();

        _rb.AddExplosionForce(amount * KnockbackMultiplier, position, radius, upwardsModifier, ForceMode.VelocityChange);

        if (_actualPushCouroutine != null) StopCoroutine(_actualPushCouroutine);

        if (gameObject.activeInHierarchy) _actualPushCouroutine = StartCoroutine(ApplyPush(pushedTime, pusher));
    }

    public void CancelForces()
    {
        _rb.velocity = Vector3.zero;

    }

    #region Timed States

    public void ApplySlowMovement(float duration, float amount)
    {
        if (_invulnerable) return;

        _soundModule.PlayDisarmSound();
        StartCoroutine(ExecuteSlowMovement(duration, amount));
    }

    public void ApplyKnockbackMultiplierChange(float duration, float amount)
    {
        if (_invulnerable) return;

        StartCoroutine(ExecuteKnockbackMultiplierChange(duration, amount));
    }

    public void ApplyStun(float duration)
    {
        if (_invulnerable) return;

        _soundModule.PlayStunSound();
        StartCoroutine(ExecuteStun(duration));
    }

    public void ApplyDisarm(float duration)
    {
        if (_invulnerable) return;

        _soundModule.PlayDisarmSound();
        StartCoroutine(ExecuteDisarm(duration));
    }

    public void ApplyCastState(float duration)
    {
        StartCoroutine(ExecuteCastTime(duration));
    }

    public void ApplyInvulnerability(float duration)
    {
        StartCoroutine(ExecuteInvulnerabilityTime(duration));
    }

    public bool FinishedCasting()
    {
        return true;
    }

    IEnumerator ExecuteStun(float duration)
    {
        _isStunned = true;

        yield return new WaitForSeconds(duration);

        _isStunned = false;
    }

    IEnumerator ExecuteSlowMovement(float duration, float amount)
    {
        var oldMulti = MovementMultiplier;

        MovementMultiplier = amount;

        yield return new WaitForSeconds(duration);

        MovementMultiplier = oldMulti;
    }

    IEnumerator ExecuteKnockbackMultiplierChange(float duration, float amount)
    {
        var oldMulti = KnockbackMultiplier;

        KnockbackMultiplier = amount;

        yield return new WaitForSeconds(duration);

        KnockbackMultiplier = oldMulti;
    }

    IEnumerator ExecuteDisarm(float duration)
    {
        _isDisarmed = true;

        yield return new WaitForSeconds(duration);

        _isDisarmed = false;
    }

    IEnumerator ExecuteCastTime(float duration)
    {
        _isCasting = true;

        yield return new WaitForSeconds(duration);

        _isCasting = false;
        FinishedCasting();
    }

    IEnumerator ExecuteInvulnerabilityTime(float duration)
    {
        _invulnerable = true;

        yield return new WaitForSeconds(duration);

        _invulnerable = false;
    }

    IEnumerator ApplyPush(Player pusher)
    {
        myPusher = pusher;
        isPushed = true;

        yield return new WaitForSeconds(pushTimeCheck);

        myPusher = null;
        isPushed = false;

    }

    IEnumerator ApplyPush(float time, Player pusher)
    {
        myPusher = pusher;
        isPushed = true;

        yield return new WaitForSeconds(time);

        myPusher = null;
        isPushed = false;

    }

    #endregion

    #region States with callback
    public void ApplySlowMovement(Func<bool> callback, float amount)
    {
        if (_invulnerable) return;

        _soundModule.PlayDisarmSound();
        StartCoroutine(ExecuteSlowMovement(callback, amount));
    }

    public void ApplyKnockbackMultiplierChange(Func<bool> callback, float amount)
    {
        if (_invulnerable) return;

        StartCoroutine(ExecuteKnockbackMultiplierChange(callback, amount));
    }

    public void ApplyStun(Func<bool> callback)
    {
        if (_invulnerable) return;

        _soundModule.PlayStunSound();
        StartCoroutine(ExecuteStun(callback));
    }

    public void ApplyDisarm(Func<bool> callback)
    {
        if (_invulnerable) return;

        _soundModule.PlayDisarmSound();
        StartCoroutine(ExecuteDisarm(callback));
    }

    public void ApplyCastState(Func<bool> callback)
    {
        StartCoroutine(ExecuteCastTime(callback));
    }

    public void ApplyInvulnerability(Func<bool> callback)
    {
        StartCoroutine(ExecuteInvulnerabilityTime(callback));
    }

    IEnumerator ExecuteStun(Func<bool> callback)
    {
        _isStunned = true;

        yield return new WaitUntil(callback);

        _isStunned = false;
    }

    IEnumerator ExecuteKnockbackMultiplierChange(Func<bool> callback, float amount)
    {
        var oldMulti = KnockbackMultiplier;

        KnockbackMultiplier = amount;

        yield return new WaitUntil(callback);

        KnockbackMultiplier = oldMulti;
    }

    IEnumerator ExecuteSlowMovement(Func<bool> callback, float amount)
    {
        var oldMulti = MovementMultiplier;

        MovementMultiplier = amount;

        yield return new WaitUntil(callback);

        MovementMultiplier = oldMulti;
    }

    IEnumerator ExecuteDisarm(Func<bool> callback)
    {
        _isDisarmed = true;

        yield return new WaitUntil(callback);

        _isDisarmed = false;
    }

    IEnumerator ExecuteCastTime(Func<bool> callback)
    {
        _isCasting = true;

        yield return new WaitUntil(callback);

        _isCasting = false;
        FinishedCasting();
    }

    IEnumerator ExecuteInvulnerabilityTime(Func<bool> callback)
    {
        _invulnerable = true;

        yield return new WaitUntil(callback);

        _invulnerable = false;
    }
    #endregion

    public void ApplyVibration(float lowFrequencyIntensity, float highFrequencyIntensity, float duration)
    {
        if (!_vibrationAvailable) return;
        StopCoroutine("Vibrate");
        StartCoroutine(Vibrate(lowFrequencyIntensity, highFrequencyIntensity, duration));
    }

    IEnumerator VibrationCooldown()
    {
        yield return new WaitForSeconds(0.2f);

        _vibrationAvailable = true;
    }

    public void StopVibrating()
    {
        Control.SetVibration(0, 0);
    }

    IEnumerator Vibrate(float lowFrequencyIntensity, float highFrequencyIntensity, float duration)
    {
        Control.SetVibration(lowFrequencyIntensity, highFrequencyIntensity);

        yield return new WaitForSeconds(duration);

        StopVibrating();
    }

    public void ApplyShake(float duration, float intensity)
    {
        _camShake.Shake(duration, intensity);
    }

    public IDamageable GetThisEntity()
    {
        return this;
    }

}

public enum DeathType
{
    Player,
    LaserGrid,
    COUNT
}

using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public float turningSpeed;

    Controller control;
    public Controller Control { get { return control; } }

    public float movementSpeed;
    public Vector3 movDir;
    public GameObject playerEndgameTexts;
    public GameObject playerUI;

    CameraShake _camShake;
    DroneSoundController _soundModule;

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
    /// <summary>
    /// TODO: Hacer que pueda ser mayor a 1 (si llegamos a usar cosas de aumentar la velocidad de movimiento)
    /// </summary>
    public float MovementMultiplier
    {
        get { return Mathf.Clamp01(_movementMultiplier); }
        set { _movementMultiplier = Mathf.Clamp01(value); }
    }

    float _movementMultiplier = 1;

    public float maxHP = 100;
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

    public bool isPushed;
    public Player myPusher;
    PlayerStats stats;
    public float pushTimeCheck = 2;

    public PlayerStats Stats
    {
        get { return stats; }
        set { stats = value; }
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
        control = new Controller(playerID);
        _rb = GetComponent<Rigidbody>();
        _rend = GetComponentInChildren<Renderer>();
        MovementMultiplier = 1;
        Hp = maxHP;
        gameObject.name = "Player " + (playerID + 1);

        stats = new PlayerStats();

        ScoreController = GetComponent<PlayerScoreController>();
        _soundModule = GetComponent<DroneSoundController>();
        _animationController = GetComponent<PlayerAnimations>();
        _lifeForcefield = GetComponentInChildren<PlayerLifeForcefield>();

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

        control.UpdateState();

        if (!IsStunned && control.RightAnalog() != Vector2.zero)
        {
            transform.Rotate(transform.up, control.RightAnalog().x * turningSpeed);
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


        if ((stats.Score + points) >= GameManager.Instance.GetScoreToWin())
        {
            stats.Score = GameManager.Instance.GetScoreToWin();
        }
        else
        {
            stats.Score += points;
            stats.Score = Mathf.Clamp(stats.Score, 0, GameManager.Instance.GetScoreToWin());
        }

        ScoreController.SetScore(stats.Score, points);
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
        tx.text = gameObject.name + "\n" + stats.Score.ToString();
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
        var dir = transform.forward * control.LeftAnalog().y + transform.right * control.LeftAnalog().x;
        var movVector = _rb.position + dir.normalized * Time.fixedDeltaTime * movementSpeed * MovementMultiplier;
        movDir = dir;
        _rb.MovePosition(movVector);
        _animationController.SetMovementDir(control.LeftAnalog());
        //_soundModule.SetEnginePitch((control.LeftAnalog().y + control.LeftAnalog().x)/2 * movementSpeed * MovementMultiplier);
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
        _rb.AddForce(dir * amount, ForceMode.Impulse);
    }

    public void ApplyKnockback(float amount, Vector3 dir, Player pusher)
    {
        if (_invulnerable) return;

        CancelForces();
        _rb.AddForce(dir * amount, ForceMode.Impulse);

        if (_actualPushCouroutine != null) StopCoroutine(_actualPushCouroutine);

        if (gameObject.activeInHierarchy) _actualPushCouroutine = StartCoroutine(ApplyPush(pusher));
    }

    public void ApplyKnockback(float amount, Vector3 dir, float pushedTime, Player pusher)
    {
        if (_invulnerable) return;

        CancelForces();

        _rb.AddForce(dir * amount, ForceMode.Impulse);

        if (_actualPushCouroutine != null) StopCoroutine(_actualPushCouroutine);

        if (gameObject.activeInHierarchy) _actualPushCouroutine = StartCoroutine(ApplyPush(pushedTime, pusher));
    }

    public void ApplyExplosionForce(float amount, Vector3 position, float radius, float upwardsModifier = 0)
    {
        if (_invulnerable) return;

        CancelForces();

        _rb.AddExplosionForce(amount, position, radius, upwardsModifier, ForceMode.VelocityChange);

    }

    public void ApplyExplosionForce(float amount, Vector3 position, float radius, Player pusher, float upwardsModifier = 0)
    {
        if (_invulnerable) return;

        CancelForces();

        _rb.AddExplosionForce(amount, position, radius, upwardsModifier, ForceMode.VelocityChange);


        if (_actualPushCouroutine != null) StopCoroutine(_actualPushCouroutine);

        if (gameObject.activeInHierarchy) _actualPushCouroutine = StartCoroutine(ApplyPush(pusher));
    }

    public void ApplyExplosionForce(float amount, Vector3 position, float radius, float pushedTime, Player pusher, float upwardsModifier = 0)
    {
        if (_invulnerable) return;

        CancelForces();

        _rb.AddExplosionForce(amount, position, radius, upwardsModifier, ForceMode.VelocityChange);

        if (_actualPushCouroutine != null) StopCoroutine(_actualPushCouroutine);

        if (gameObject.activeInHierarchy) _actualPushCouroutine = StartCoroutine(ApplyPush(pushedTime, pusher));
    }

    public void CancelForces()
    {
        _rb.velocity = Vector3.zero;

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

}

public enum DeathType
{
    Player,
    LaserGrid,
    COUNT
}

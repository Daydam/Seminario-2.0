using Firepower.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhoenixDevelopment;
using Photon.Pun;

public class Player : MonoBehaviour, IDamageable, IPunObservable
{
    #region VARIABLES

    public float turningSpeed;

    public DroneWeightModule weightModule;
    public int Weight
    {
        get { return weightModule.weight; }
    }

    Controller _control;
    public Controller Control { get { return _control; } }
    Vector2 lastDir = Vector2.zero;
    Vector2 lastAnimMovement = Vector2.zero;
    Vector3 lastMovement = Vector2.zero;
    float inertiaFactor = 0.05f;
    float animInertiaFactor = 0.2f;

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
    PlayerCameraModule _camModule;
    PlayerParticlesModule _particleModule;
    PlayerControlModule _controlModule;
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

    PlayerLightsModuleHandler _lightsModule;
    public PlayerLightsModuleHandler LightsModule
    {
        get { return _lightsModule; }
    }


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

    public PlayerControlModule ControlModule
    {
        get
        {
            return _controlModule;
        }

        private set
        {
            _controlModule = value;
        }
    }

    public PlayerAnimations AnimationController
    {
        get
        {
            return _animationController;
        }

        set
        {
            _animationController = value;
        }
    }

    PhotonView pv;


    #endregion
    void Awake()
    {
        if (!PhotonNetwork.InRoom || (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient))
        {
            int playerID = GameManager.Instance.Register(this);

            myID = playerID;
            if(!PhotonNetwork.InRoom || ((PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient) && playerID == 0)) _control = new Controller(playerID);
            _rb = GetComponent<Rigidbody>();
            _lightsModule = GetComponent<PlayerLightsModuleHandler>();
            MovementMultiplier = 1;
            Hp = maxHP;


            gameObject.name = "Player " + (playerID + 1);

            _stats = new PlayerStats();

            ScoreController = GetComponent<PlayerScoreController>();
            _soundModule = GetComponent<DroneSoundController>();
            AnimationController = GetComponent<PlayerAnimations>();
            _lifeForcefield = GetComponentInChildren<PlayerLifeForcefield>();
            _camModule = GetComponentInChildren<PlayerCameraModule>();
            _controlModule = GetComponent<PlayerControlModule>();
            _col = GetComponent<Collider>();
            weightModule = GetComponent<DroneWeightModule>();
            _particleModule = GetComponent<PlayerParticlesModule>();
        }
    }

    void Start()
    {
        if (!PhotonNetwork.InRoom || (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient))
        {
            //We had to add an RPC for each of these.
            GameManager.Instance.StartRound += () => LockPlayer(false);
            GameManager.Instance.OnResetRound += StopVibrating;
            GameManager.Instance.OnResetRound += ResetRound;
            GameManager.Instance.OnChangeScene += StopVibrating;
        }
        if (PhotonNetwork.InRoom)
        {
            Stats.Score = 0;
            lockedByGame = true;
        }
        transform.Find("DronePos_To_RT").gameObject.layer = LayerMask.NameToLayer("Drone");
        ControlModule.HandleCollisions(this);
    }

    void Update()
    {
        if (lockedByGame) return;

        _control.UpdateState();

        if (!IsStunned && _control.RightAnalog() != Vector2.zero)
        {
            _controlModule.HandleRotation(transform.up, _control.RightAnalog().x * turningSpeed);
        }
    }

    public Vector3 GetCameraOffset()
    {
        return _camModule.Offset;
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

    public string GetBodyName()
    {
        return _animationController.GetBodyName();
    }

    public void LockPlayer(bool locked)
    {
        lockedByGame = locked;
        if(pv == null) pv = GetComponent<PhotonView>();
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient) pv.RPC("LockPlayerRPC", RpcTarget.Others, lockedByGame);
    }

    void OnTriggerEnter(Collider col)
    {
        //You left here man. Send a global RPC for hit detection and make each player see if they're hit and send back an update!
        //Basically, everyone checks their own health.
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
        LightsModule.SetLifeValue(Hp / maxHP);
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
        _cam.gameObject.SetActive(true);
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

        if (!PhotonNetwork.InRoom || (PhotonNetwork.InRoom && PhotonNetwork.LocalPlayer.NickName == gameObject.name))
        {
            StopVibrating();
            _soundModule.PlayDeathSound();

            var deathPartID = SimpleParticleSpawner.ParticleID.DEATHPARTICLE;
            var deathParticle = SimpleParticleSpawner.Instance.particles[deathPartID].GetComponentInChildren<ParticleSystem>();
            SimpleParticleSpawner.Instance.SpawnParticle(deathParticle.gameObject, transform.position, transform.forward);

            _cam.OnPlayerDeath(type);

            EventManager.Instance.DispatchEvent(PlayerEvents.Death, this, type, isPushed, gameObject.tag);
            _rb.velocity = Vector3.zero;
            gameObject.SetActive(false);

            //Iván si se rompe algo perdón
            lastMovement = Vector3.zero;
            lastAnimMovement = Vector3.zero;
            lastDir = Vector3.zero;
        }
        if (PhotonNetwork.InRoom && int.Parse(PhotonNetwork.NickName.Split(' ')[1]) == int.Parse(gameObject.name.Split(' ')[1])) pv.RPC("DestroyPlayerRPC", RpcTarget.Others, (int)type, null);
    }

    void DestroyPlayer(DeathType type, string killerTag)
    {
        if (!PhotonNetwork.InRoom || (PhotonNetwork.InRoom && PhotonNetwork.LocalPlayer.NickName == gameObject.name))
        {
            StopVibrating();
            _soundModule.PlayDeathSound();
            var deathPartID = SimpleParticleSpawner.ParticleID.DEATHPARTICLE;
            var deathParticle = SimpleParticleSpawner.Instance.particles[deathPartID].GetComponentInChildren<ParticleSystem>();
            SimpleParticleSpawner.Instance.SpawnParticle(deathParticle.gameObject, transform.position, transform.forward);

            _cam.OnPlayerDeath(type);

            EventManager.Instance.DispatchEvent(PlayerEvents.Death, this, type, isPushed, killerTag);
            _rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
        //SET DESTROYPLAYERRPC
        if (PhotonNetwork.InRoom && PhotonNetwork.LocalPlayer.NickName == gameObject.name) pv.RPC("DestroyPlayerRPC", RpcTarget.Others, (int)type, killerTag);
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
        var newMovement = Vector3.Lerp(lastMovement, dir.normalized * Time.fixedDeltaTime * movementSpeed * MovementMultiplier, inertiaFactor);
        var movVector = _rb.position + newMovement;
        movDir = dir;
        _rb.MovePosition(movVector);

        Vector2 animMovement = _control.LeftAnalog();

        animMovement = Vector2.Lerp(lastAnimMovement, animMovement, inertiaFactor);

        Vector2 finalDir = new Vector2(animMovement.x - lastAnimMovement.x, animMovement.y - lastAnimMovement.y);

        if (Mathf.Sign(_control.LeftAnalog().x) != Mathf.Sign(lastAnimMovement.x) || Mathf.Abs(_control.LeftAnalog().x) <= 0.004f)
            finalDir.x = Mathf.Abs(finalDir.x) <= 0.007f ? animMovement.x : Mathf.Sign(finalDir.x);
        else
            finalDir.x = animMovement.x;

        if (Mathf.Sign(_control.LeftAnalog().y) != Mathf.Sign(lastAnimMovement.y) || Mathf.Abs(_control.LeftAnalog().y) <= 0.004f)
            finalDir.y = Mathf.Abs(finalDir.y) <= 0.007f ? animMovement.y : Mathf.Sign(finalDir.y);
        else
            finalDir.y = animMovement.y;

        finalDir.x = Mathf.Lerp(lastDir.x, finalDir.x, animInertiaFactor);
        finalDir.y = Mathf.Lerp(lastDir.y, finalDir.y, animInertiaFactor);

        AnimationController.SetMovementDir(finalDir);
        _controlModule.HandleMovement(finalDir);

        //_soundModule.SetEnginePitch((control.LeftAnalog().y + control.LeftAnalog().x)/2 * movementSpeed * MovementMultiplier);
        lastMovement = newMovement;
        lastAnimMovement = animMovement;
        lastDir = finalDir;
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
        LightsModule.SetLifeValue(Hp / maxHP);
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
        LightsModule.SetLifeValue(Hp / maxHP);
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
        LightsModule.SetLifeValue(Hp / maxHP);
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

        //Iván si se rompe algo perdón
        lastMovement = Vector3.zero;
        lastAnimMovement = Vector3.zero;
        lastDir = Vector3.zero;
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
        _particleModule.ApplyStun(true);

        StartCoroutine(ExecuteStun(duration));

        //Iván si se rompe algo perdón
        lastMovement = Vector3.zero;
        lastAnimMovement = Vector3.zero;
        lastDir = Vector3.zero;
    }

    public void ApplyDisarm(float duration, bool scrambleScreen)
    {
        if (_invulnerable) return;

        _soundModule.PlayDisarmSound();
        StartCoroutine(ExecuteDisarm(duration, scrambleScreen));
    }

    public void ApplyCastState(float duration)
    {
        StartCoroutine(ExecuteCastTime(duration));

        //Iván si se rompe algo perdón
        lastMovement = Vector3.zero;
        lastAnimMovement = Vector3.zero;
        lastDir = Vector3.zero;
    }

    public void ApplyInvulnerability(float duration)
    {
        StartCoroutine(ExecuteInvulnerabilityTime(duration));
    }

    public void ActivateRepulsion(float duration, float radius)
    {
        StartCoroutine(RepulsionManagement(duration, radius));
    }

    public IEnumerator RepulsionManagement(float duration, float radius)
    {
        _cam.OnPlayerUseRepulsion(true, radius, duration);

        yield return new WaitForSeconds(duration);

        _cam.OnPlayerUseRepulsion(false, radius, duration);
    }

    public bool FinishedCasting()
    {
        return true;
    }

    IEnumerator ExecuteStun(float duration)
    {
        _isStunned = true;

        yield return new WaitForSeconds(duration);

        _particleModule.ApplyStun(false);

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

    IEnumerator ExecuteDisarm(float duration, bool scrambleScreen)
    {
        _isDisarmed = true;
        if (scrambleScreen) _cam.OnPlayerDisarm(_isDisarmed);

        yield return new WaitForSeconds(duration);

        _isDisarmed = false;
        if (scrambleScreen) _cam.OnPlayerDisarm(_isDisarmed);
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

    public void ApplyStun(Func<bool> callback)
    {
        if (_invulnerable) return;
        _soundModule.PlayStunSound();

        _particleModule.ApplyStun(true);

        StartCoroutine(ExecuteStun(callback));
    }

    public void ApplyCastState(Func<bool> callback)
    {
        StartCoroutine(ExecuteCastTime(callback));
    }

    IEnumerator ExecuteStun(Func<bool> callback)
    {
        _isStunned = true;

        yield return new WaitUntil(callback);

        _particleModule.ApplyStun(false);

        _isStunned = false;
    }

    IEnumerator ExecuteCastTime(Func<bool> callback)
    {
        _isCasting = true;

        yield return new WaitUntil(callback);

        _isCasting = false;
        FinishedCasting();
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
    #region ONLINE PLAY
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }

    [PunRPC]
    public void initPlayerRPC(int playerId)
    {
        myID = playerId;
        if(int.Parse(PhotonNetwork.NickName.Split(' ')[1]) == playerId) _control = new Controller(0);
        _rb = GetComponent<Rigidbody>();
        _lightsModule = GetComponent<PlayerLightsModuleHandler>();
        MovementMultiplier = 1;
        Hp = maxHP;


        gameObject.name = "Player " + (playerId + 1);

        _stats = new PlayerStats();

        ScoreController = GetComponent<PlayerScoreController>();
        _soundModule = GetComponent<DroneSoundController>();
        AnimationController = GetComponent<PlayerAnimations>();
        _lifeForcefield = GetComponentInChildren<PlayerLifeForcefield>();
        _camModule = GetComponentInChildren<PlayerCameraModule>();
        _controlModule = GetComponent<PlayerControlModule>();
        _col = GetComponent<Collider>();
        weightModule = GetComponent<DroneWeightModule>();
        _particleModule = GetComponent<PlayerParticlesModule>();

        pv = GetComponent<PhotonView>();

        //REGISTER IT TO THE GAME MANAGER
        //If we want to register to the local game manager, we can do a coroutine to do it in order,
        //wait while the previous index hasn't been loaded
    }

    [PunRPC]
    public void LockPlayerRPC(bool locked)
    {
        lockedByGame = locked;
    }

    [PunRPC]
    public void DestroyPlayerRPC(int deathType, string killerTag)
    {
        var type = (DeathType)deathType;

        StopVibrating();
        _soundModule.PlayDeathSound();
        var deathPartID = SimpleParticleSpawner.ParticleID.DEATHPARTICLE;
        var deathParticle = SimpleParticleSpawner.Instance.particles[deathPartID].GetComponentInChildren<ParticleSystem>();
        SimpleParticleSpawner.Instance.SpawnParticle(deathParticle.gameObject, transform.position, transform.forward);

        _cam.OnPlayerDeath(type);

        EventManager.Instance.DispatchEvent(PlayerEvents.Death, this, type, isPushed, killerTag);
        _rb.velocity = Vector3.zero;
        gameObject.SetActive(false);

    }

    [PunRPC]
    public void StopVibratingRPC()
    {
        Control.SetVibration(0, 0);
    }

    [PunRPC]
    public void ResetRoundRPC()
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
        _cam.gameObject.SetActive(true);
    }
    #endregion
}

public enum DeathType
{
    Player,
    LaserGrid,
    COUNT
}

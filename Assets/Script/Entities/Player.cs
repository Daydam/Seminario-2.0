using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float turningSpeed;

    Controller control;
    public Controller Control { get { return control; } }

    public float movementSpeed;
    public Vector3 movDir;
    public GameObject playerEndgameTexts;
    public GameObject playerUI;

    CamFollow _cam;

    Coroutine _actualPushCouroutine;

    public bool lockedByGame;

    Rigidbody _rb;
    public Rigidbody GetRigidbody { get { return _rb; } }

    bool _isStunned;
    bool _isDisarmed;
    bool _isUnableToMove;
    public bool IsStunned { get { return _isStunned; } }
    public bool IsDisarmed { get { return _isDisarmed; } }
    public bool IsUnableToMove { get { return _isUnableToMove; } }

    Renderer _rend;
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

    public bool isPushed;
    public Player myPusher;
    private int _score;
    public float pushTimeCheck = 2;

    public int Score
    {
        get { return _score; }
        set { _score = value <= 0 ? 0 : value; }
    }

    void Awake()
    {
        int playerID = GameManager.Instance.Register(this);
        control = new Controller(playerID);
        _rb = GetComponent<Rigidbody>();
        _rend = GetComponentInChildren<SkinnedMeshRenderer>();
        MovementMultiplier = 1;
        Hp = maxHP;
        gameObject.name = "Player " + (playerID + 1);
    }

    private void Start()
    {
        GameManager.Instance.OnResetRound += ResetRound;
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
        _cam = cam;
    }

    public void DeactivateCamera()
    {
        _cam.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("Bullet")) && gameObject.TagDifferentFrom(col.gameObject.tag))
        {
            Bullet b = col.GetComponent<Bullet>();
            TakeDamage(b.Damage, b.tag);

            var knockbackInfo = b.GetKnockbackInfo();
            var forceDir = transform.position - knockbackInfo.Item3;

            ApplyKnockback(knockbackInfo.Item2, forceDir.normalized, knockbackInfo.Item1);

            BulletSpawner.Instance.ReturnToPool(b);
        }
        else if (col.gameObject.LayerMatchesWith(LayerMask.NameToLayer("DeathZone")))
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

    public void UpdateScore(int score)
    {
        Score += score;
    }

    public void ResetHP()
    {
        Hp = maxHP;
        _rend.material.SetFloat("_Life", Hp / maxHP);
    }

    public void ResetRound()
    {
        StopAllCoroutines();
        _isStunned = false;
        _isDisarmed = false;
        _isUnableToMove = false;
        _movementMultiplier = 1;
        isPushed = false;
        myPusher = null;
    }

    public void ActivatePlayerEndgame(bool activate, string replaceName, string replaceScore)
    {
        playerEndgameTexts.SetActive(activate);
        var tx = playerEndgameTexts.GetComponentInChildren<UnityEngine.UI.Text>();
        tx.text = gameObject.name + "\n" + Score.ToString();
    }

    public void ActivatePlayerEndgame(bool activate = false)
    {
        playerEndgameTexts.SetActive(activate);
    }

    void DestroyPlayer(DeathType type)
    {
        EventManager.DispatchEvent(PlayerEvents.Death, this, type, isPushed, gameObject.tag);
        gameObject.SetActive(false);
    }

    void DestroyPlayer(DeathType type, string killerTag)
    {
        EventManager.DispatchEvent(PlayerEvents.Death, this, type, isPushed, killerTag);
        gameObject.SetActive(false);

    }

    void FixedUpdate()
    {
        if (!IsStunned && !IsUnableToMove && !lockedByGame)
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
    }

    public void TakeDamage(float damage)
    {
        SubstractLife(damage);

        if (Hp <= 0) DestroyPlayer(DeathType.Player);
    }

    public void TakeDamage(float damage, string killerTag)
    {
        SubstractLife(damage);
        if (Hp <= 0) DestroyPlayer(DeathType.Player, killerTag);
    }

    void SubstractLife(float damage)
    {
        Hp -= damage;
        _rend.material.SetFloat("_Life", Hp / maxHP);
    }

    public void ApplyKnockback(float amount, Vector3 dir)
    {
        _rb.AddForce(dir * amount, ForceMode.Impulse);
    }

    public void ApplyKnockback(float amount, Vector3 dir, Player pusher)
    {
        _rb.AddForce(dir * amount, ForceMode.Impulse);

        if (_actualPushCouroutine != null) StopCoroutine(_actualPushCouroutine);

        if (gameObject.activeInHierarchy) _actualPushCouroutine = StartCoroutine(ApplyPush(pusher));
    }

    public void ApplyKnockback(float amount, Vector3 dir, float pushedTime, Player pusher)
    {
        _rb.AddForce(dir * amount, ForceMode.Impulse);

        if (_actualPushCouroutine != null) StopCoroutine(_actualPushCouroutine);

        if (gameObject.activeInHierarchy) _actualPushCouroutine = StartCoroutine(ApplyPush(pushedTime, pusher));
    }

    public void ApplyStun(float duration)
    {
        StartCoroutine(ExecuteStun(duration));
    }

    public void ApplyDisarm(float duration)
    {
        StartCoroutine(ExecuteDisarm(duration));
    }

    public void ApplyNeutralizeMovement(float duration)
    {
        StartCoroutine(ExecuteNeutralizedMovement(duration));
    }

    public void ApplySlowMovement(float duration, float amount)
    {
        StartCoroutine(ExecuteSlowMovement(duration, amount));
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

    IEnumerator ExecuteNeutralizedMovement(float duration)
    {
        _isUnableToMove = true;

        yield return new WaitForSeconds(duration);

        _isUnableToMove = false;
    }

    IEnumerator ExecuteSlowMovement(float duration, float amount)
    {
        MovementMultiplier = amount;

        yield return new WaitForSeconds(duration);

        MovementMultiplier = 1;
    }

    IEnumerator ApplyPush(Player pusher)
    {
        myPusher = pusher;
        isPushed = true;

        yield return new WaitForSeconds(pushTimeCheck);

        myPusher = null;
        isPushed = false;

        //_actualPushCouroutine = null;
    }

    IEnumerator ApplyPush(float time, Player pusher)
    {
        myPusher = pusher;
        isPushed = true;

        yield return new WaitForSeconds(time);

        myPusher = null;
        isPushed = false;

        //_actualPushCouroutine = null;
    }

}

public enum DeathType
{
    Player,
    LaserGrid,
    COUNT
}
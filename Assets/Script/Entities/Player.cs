using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Controller control;
    public Controller Control { get { return control; } }

    public float movementSpeed;
    public Vector3 movDir;

    Rigidbody _rb;
    public Rigidbody GetRigidbody { get { return _rb; } }

    bool _isStunned;
    bool _isDisarmed;
    bool _isUnableToMove;
    public bool IsStunned { get { return _isStunned; } }
    public bool IsDisarmed { get { return _isDisarmed; } }
    public bool IsUnableToMove { get { return _isUnableToMove; } }

    /// <summary>
    /// TODO: Hacer que pueda ser mayor a 1 (si llegamos a usar cosas de aumentar la velocidad de movimiento)
    /// </summary>
    public float MovementMultiplier
    {
        get { return Mathf.Clamp01(_movementMultiplier); }
        set { _movementMultiplier = Mathf.Clamp01(value); }
    }

    float _movementMultiplier = 1;

    public float hp;

    #region Cambios Iván 10/6
    public bool isPushed;
    public Player myPusher;
    private int _score;
    public int Score
    {
        get { return _score; }
        set { _score = value <= 0 ? 0 : value; }
    }
    #endregion

    void Awake()
    {
        int playerID = GameManager.Instance.Register(this);
        control = new Controller(playerID);
        _rb = GetComponent<Rigidbody>();
        MovementMultiplier = 1;
    }

    void Update()
    {
        if (control.RightAnalog() != Vector2.zero && !IsStunned)
        {
            transform.LookAt(transform.position + new Vector3(control.RightAnalog().x, 0, control.RightAnalog().y));
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Bullet") && gameObject.tag != col.gameObject.tag)
        {
            Bullet b = col.GetComponent<Bullet>();
            TakeDamage(b.Damage, b.tag);
            BulletSpawner.Instance.ReturnToPool(b);
            //DoKnockback
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("DeathZone"))
        {
            DestroyPlayer(DeathType.LaserGrid);
        }
    }

    #region Cambios Iván 10/6

    public void UpdateScore(int score)
    {
        Score += score;
    }

    #endregion

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
        if (!IsStunned && !IsUnableToMove)
        {
            Movement();
        }
    }

    void Movement()
    {
        var movVector = _rb.position + new Vector3(control.LeftAnalog().x, 0, control.LeftAnalog().y) * Time.fixedDeltaTime * movementSpeed * MovementMultiplier;
        movDir = movVector - _rb.position;
        _rb.MovePosition(movVector);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0) DestroyPlayer(DeathType.Player);
    }

    public void TakeDamage(float damage, string killerTag)
    {
        hp -= damage;
        if (hp <= 0) DestroyPlayer(DeathType.Player, killerTag);
    }

    public void ApplyKnockback(float amount, Vector3 dir)
    {
        _rb.AddForce(dir * amount, ForceMode.Impulse);
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

}

public enum DeathType
{
    Player,
    LaserGrid,
    COUNT
}
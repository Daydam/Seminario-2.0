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

    public Rigidbody GetRigidbody
    {
        get
        {
            return _rb;
        }
    }

    public float hp;

    void Awake()
    {
        int playerID = GameManager.Instance.Register(this);
        control = new Controller(playerID + 1);
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GetRandomWeapon();
    }

    void GetRandomWeapon()
    {
        int rndIndex = Random.Range(0, GameManager.Instance.AllWeapons.Count);
        var myWeapon = GameObject.Instantiate(GameManager.Instance.AllWeapons[rndIndex], transform, false);
        myWeapon.transform.localPosition = Vector3.zero;
        myWeapon.transform.forward = transform.forward;
        myWeapon.gameObject.tag = gameObject.tag;
    }

    void Update()
    {
        if (control.RightAnalog() != Vector2.zero)
        {
            transform.LookAt(transform.position + new Vector3(control.RightAnalog().x, 0, control.RightAnalog().y));
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Bullet") && gameObject.tag != col.gameObject.tag)
        {
            Bullet b = col.GetComponent<Bullet>();
            BulletSpawner.Instance.ReturnBulletToPool(b);
            hp -= b.Damage;
            if (hp <= 0) DestroyPlayer();
        }
    }

    void DestroyPlayer()
    {
        GameManager.Instance.Unregister(this);
        Destroy(this);
    }

    void FixedUpdate()
    {
        //Test futuro: cambiar MovePosition por velocity
        //var movVector = new Vector3(control.LeftAnalog().x, 0, control.LeftAnalog().y) /** Time.fixedDeltaTime*/ * movementSpeed;
        //_rb.velocity = movVector;

        var movVector = _rb.position + new Vector3(control.LeftAnalog().x, 0, control.LeftAnalog().y) * Time.fixedDeltaTime * movementSpeed;
        movDir = movVector - _rb.position;
        _rb.MovePosition(movVector);
    }
}

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

    void Awake()
    {
        int playerID = GameManager.Instance.Register(this);
        control = new Controller(playerID + 1);
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (control.RightAnalog() != Vector2.zero)
        {
            transform.LookAt(transform.position + new Vector3(control.RightAnalog().x, 0, control.RightAnalog().y));
        }

        /*if (control.DefensiveSkill()) Debug.Log("entre secondary weapon");
        if (control.ComplimentarySkill1()) Debug.Log("entre primary skill");
        if (control.ComplimentarySkill2()) Debug.Log("entre secondary skill");*/
    }

    void OnTriggerEnter(Collider col)
    {
        //Acá detectaría la colisión con la bala
        if(gameObject.layer != col.gameObject.layer && col.GetComponent<Bullet>() != null)
        {
            print("LA COMI MAN");
        }
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

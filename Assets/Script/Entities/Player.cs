using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Controller control;
    public Controller Control { get { return control; } }

    public float movementSpeed;

    Rigidbody _rb;

    void Start()
    {
        int playerID = GameManager.Instance.Register(this);
        control = new Controller(playerID + 1);
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //Test futuro: cambiar MovePosition por velocity
        //var movVector = new Vector3(control.LeftAnalog().x, 0, control.LeftAnalog().y) /** Time.fixedDeltaTime*/ * movementSpeed;
        //_rb.velocity = movVector;

        var movVector = _rb.position + new Vector3(control.LeftAnalog().x, 0, control.LeftAnalog().y) * Time.fixedDeltaTime * movementSpeed;
        
        _rb.MovePosition(movVector);

        if(control.RightAnalog() != Vector2.zero)
        {
            transform.LookAt(_rb.position + new Vector3(control.RightAnalog().x, 0, control.RightAnalog().y));
        }
        
        //Esto queda para probar que los comandos funcionen, hasta que los scripts de weapon estén implementados.
        if (control.PrimaryWeapon()) Debug.Log("entre primary weapon");
        if (control.SecondaryWeapon()) Debug.Log("entre secondary weapon");
        if (control.PrimarySkill()) Debug.Log("entre primary skill");
        if (control.SecondarySkill()) Debug.Log("entre secondary skill");
    }
}

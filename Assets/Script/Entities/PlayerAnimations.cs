using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAnimations : MonoBehaviour
{
    Animator _an;

	void Start ()
	{
        _an = GetComponent<Animator>();
	}

    public void SetMovementDir(Vector2 dir)
    {
        _an.SetFloat("Front", dir.y);
        _an.SetFloat("Sides", dir.x);
    }
}
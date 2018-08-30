using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Reflectors : MonoBehaviour
{
    public float speed;
	
	void Update ()
	{
        transform.Rotate(transform.up * speed * Time.deltaTime);
	}
}

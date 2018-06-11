using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EmpCloud : MonoBehaviour
{
	void Start ()
	{
		
	}
	
	void Update ()
	{
        var coso = 1;
        transform.localScale -= new Vector3(coso * Time.deltaTime, coso * Time.deltaTime, coso * Time.deltaTime);
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Antenna")
        {
            GameObject.Find("OuterRing").gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovementController : MonoBehaviour
{
    public Material vertexCollapse;
    public List<Rigidbody> allBodies;
    public float radiusGravityAffect;

	public float speed;
    public bool activeBlackHole;

	private Vector3 _startPos;

    private void Start()
    {
    	_startPos = transform.position;
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        	
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
        {
            activeBlackHole = true;
            allBodies = new List<Rigidbody>();

            foreach(var item in Physics.OverlapSphere(transform.position, radiusGravityAffect))
            {
                if(item.GetComponent<Rigidbody>() && item.gameObject.layer == 13)
                    allBodies.Add(item.GetComponent<Rigidbody>());
            }
            vertexCollapse.SetFloat("_Radius", 0.0f);
            vertexCollapse.SetVector("_BlackHolePos", transform.position);

            gameObject.SetActive(false);
        }
    }
}

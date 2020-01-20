using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleGravity : MonoBehaviour
{
    public Material vertexCollapse;
    public float collapseSpeed;
    public float radius;
    public float power;

    private float gravitacionalConstant = 6.67e-11f;

    private float _time;
    private bool _applyCollapse = true;

    private List<Rigidbody> _allBodies;

    private void Start()
    {
        _allBodies = new List<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //ApplyGForce();
        //ApplyGravity();
    }

    private void Aceleration()
    {

    }

    private void GForce(Rigidbody rb)
    {
        var m = rb.mass;
        var d2 = Mathf.Pow(Vector3.Distance(rb.position, transform.position), 2.0f);

        var g = (m / d2) * gravitacionalConstant;
    }

    private void ApplyGravity()
    {
        var massCenter = transform.position;
        Collider[] colliders = Physics.OverlapSphere(massCenter, radius);

        foreach (Collider hit in colliders)
        {
            if (hit && hit.transform != transform && hit.GetComponent<Rigidbody>() != null)
            {
                var difference = hit.transform.position - transform.position;
                var rb = hit.GetComponent<Rigidbody>();


                rb.AddForce(difference.normalized * power, ForceMode.Force);
                rb.velocity += GAcceleration(transform.position, 10, rb);
            }
        }
    }

    public Vector3 GAcceleration(Vector3 position, float mass, Rigidbody rb)
    {
        Vector3 dir = position - rb.position;

        float gForce = gravitacionalConstant * ((mass * rb.mass) / dir.sqrMagnitude);
        gForce /= rb.mass;

        return dir.normalized * gForce * Time.fixedDeltaTime;
    }

    private void ApplyGForce()
    {
        for (int i = 0; i < _allBodies.Count; i++)
        {
            //_allBodies[i].MovePosition(transform.position);
            _allBodies[i].velocity += GAcceleration(transform.position, 2, _allBodies[i]);
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            _time += collapseSpeed * Time.deltaTime;

            if (_time > 1.0f && vertexCollapse.GetFloat("_Radius") >= 3.0f)
                _time = 0.0f;
            else
                vertexCollapse.SetFloat("_Radius", Mathf.Lerp(0.0f, 3.0f, _time));
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
            _applyCollapse = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_applyCollapse && other.gameObject.layer == 13)
        {
            _time += collapseSpeed * Time.deltaTime;

            if (_time > 1.0f && vertexCollapse.GetFloat("_Radius") >= 3.0f)
            {
                _time = 0.0f;
                _applyCollapse = false;
            }

            else
                vertexCollapse.SetFloat("_Radius", Mathf.Lerp(0.0f, 3.0f, _time));
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
        {
            print("colisiono");
            var rb = other.GetComponent<Rigidbody>();
            _allBodies.Add(rb);
            //rb.velocity += GAcceleration(transform.position, 10, rb);
        }
    }*/
}

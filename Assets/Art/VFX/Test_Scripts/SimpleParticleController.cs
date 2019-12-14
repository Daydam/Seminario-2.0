using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParticleController : MonoBehaviour
{
    public Material vertexCollapse;
    public ParticleSystem ps_Proyectile;
    public GameObject vfx_blackHole;

    public float explosionForce;
    public float explosionRadius;
    //public ParticleSystem ps_Explosion;

    public List<ParticleSystem> allPS_BlackHole;

    private SimpleMovementController _movementController;
    private Vector3 _startProyectilePos;

    private float _gravitacionalConstant = 6.67e-11f;

    private void Start()
    {
        _startProyectilePos = ps_Proyectile.transform.position;
        _movementController = ps_Proyectile.GetComponent<SimpleMovementController>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            vertexCollapse.SetVector("_BlackHolePos", Vector3.zero);
            vertexCollapse.SetFloat("_Radius", 0.0f);
            for (int i = 0; i < allPS_BlackHole.Count; i++)
            {
                allPS_BlackHole[i].Stop();
            }
            ps_Proyectile.gameObject.SetActive(true);
            _movementController.transform.position = _startProyectilePos;
            _movementController.activeBlackHole = false;
        }

        if (_movementController.activeBlackHole)
        {
            vfx_blackHole.transform.position = ps_Proyectile.transform.position;
            vfx_blackHole.GetComponentInChildren<ParticleSystem>().Play();
            _movementController.activeBlackHole = false;
        }
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        foreach (var item in _movementController.allBodies)
        {
            item.AddExplosionForce(explosionForce, vfx_blackHole.transform.position, explosionRadius);
            //item.AddForce(FGravity(vfx_blackHole.transform.position, vfx_blackHole.GetComponent<Rigidbody>().mass, item) * -10f, ForceMode.Force);
            //item.velocity *= Acceleration(vfx_blackHole.transform.position, vfx_blackHole.GetComponent<Rigidbody>().mass, item) * Time.fixedDeltaTime;
        }
    }

    private Vector3 FGravity(Vector3 gravityCenter, float mass, Rigidbody rb)
    {
        var massMultiply = mass * rb.mass;
        var distance = gravityCenter - rb.position;
        var division = massMultiply / distance.sqrMagnitude;

        var g = _gravitacionalConstant * division;

        return distance.normalized * g * Time.fixedDeltaTime;
    }

    private float Acceleration(Vector3 gravityCenter, float mass, Rigidbody rb)
    {
        var d2 = Mathf.Pow(Vector3.Distance(gravityCenter, rb.position), 2.0f);
        var g = _gravitacionalConstant * (mass / d2);

        return g;
    }
}

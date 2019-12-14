using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestingAnim_ImplosiveCharge : MonoBehaviour
{
    public ParticleSystem trails;
    public GameObject replace;

    public float speed;

    public float timeToExplode;

    private float _currentSpeed;
    private float _timer;
    private bool _spawnImplosiveCharge;

    private Vector3 _startPos;
    private ParticleSystem[] _allParticles;


    void Start()
    {
        _currentSpeed = speed;
        _timer = timeToExplode;
        _startPos = transform.position;

        _allParticles = GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            replace.GetComponentInChildren<ParticleSystem>().Stop();

            GetComponentInChildren<ParticleSystem>().Play();

            _currentSpeed = speed;
            _timer = timeToExplode;

            transform.position = _startPos;

            _allParticles.Select(x => x.main).Aggregate(FList.Create<ParticleSystem.MainModule>(), (acum, current) =>
            {
                current.loop = true;
                return acum + current;
            });
        }

        _timer -= Time.deltaTime;

        if(_timer <= 0.0f)
        {
            GetComponentInChildren<ParticleSystem>().Stop();
        }

        transform.position += transform.forward * _currentSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
        {
            _currentSpeed = 0;

            foreach (var ps in _allParticles)
            {
                ps.Stop();
            }

            replace.transform.position = transform.position;
            replace.SetActive(true);



            /*_allParticles.Select(x => x.main).Aggregate(FList.Create<ParticleSystem.MainModule>(), (acum, current) =>
            {
                current.loop = false;
                return acum + current;
            });*/

            
        }
    }
}

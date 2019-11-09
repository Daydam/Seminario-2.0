using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModuleParticleController : MonoBehaviour
{
    public GameObject readyParticle, shootParticle;
    ParticleSystem _readyParticle, _shootParticle;

    void Awake()
    {
        _readyParticle = readyParticle.GetComponentInChildren<ParticleSystem>();
        _shootParticle = shootParticle.GetComponentInChildren<ParticleSystem>();
        _shootParticle.Stop();
    }

    public void OnShoot()
    {
        _readyParticle.Stop();
        _shootParticle.Play();
    }

    public void OnAvailableShot()
    {
        _readyParticle.Play();
    }
}

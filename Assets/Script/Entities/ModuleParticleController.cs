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
        _readyParticle.Play(true);
        _shootParticle = shootParticle.GetComponentInChildren<ParticleSystem>();

        _shootParticle.Stop(true);
        shootParticle.SetActive(false);
    }

    public void OnShoot()
    {
        print("HOLANDAAAA");
        _readyParticle.Stop(true);
        readyParticle.SetActive(false);

        shootParticle.SetActive(true);
        _shootParticle.Play(true);
    }

    public void OnAvailableShot()
    {
        print("PEPOSAAA");
        readyParticle.SetActive(true);
        _readyParticle.Play(true);

        _shootParticle.Stop(true);
        shootParticle.SetActive(false);
    }
}

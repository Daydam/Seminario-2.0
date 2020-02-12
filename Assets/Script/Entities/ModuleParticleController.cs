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
        if (readyParticle) _readyParticle = readyParticle.GetComponentInChildren<ParticleSystem>();
        //_readyParticle.Play(true);

        if (shootParticle)
        {
            _shootParticle = shootParticle.GetComponentInChildren<ParticleSystem>();

            _shootParticle.Stop(true);
            shootParticle.SetActive(false);
        }
    }

    public void OnShoot()
    {
        //_readyParticle.Stop(true);
        if (readyParticle) readyParticle.SetActive(false);

        if (shootParticle)
        {
            shootParticle.SetActive(true);
            _shootParticle.Play(true);
        }
    }

    public void OnAvailableShot()
    {
        if (readyParticle) readyParticle.SetActive(true);
        //_readyParticle.Play(true);

        if (shootParticle)
        {
            _shootParticle.Stop(true);
            shootParticle.SetActive(false);
        }
    }
}

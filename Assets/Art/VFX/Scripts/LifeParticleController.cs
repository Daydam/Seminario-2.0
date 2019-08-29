using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeParticleController : MonoBehaviour
{
    public float lifeTime;
    public bool backToPool;

    private float _currentTime;

    private void OnEnable()
    {
        backToPool = false;
    }

    private void Update()
    {
        if (_currentTime > 0.0f)
            _currentTime -= Time.deltaTime;
        else
        {
            _currentTime = lifeTime;
            backToPool = true;
        }
    }
}

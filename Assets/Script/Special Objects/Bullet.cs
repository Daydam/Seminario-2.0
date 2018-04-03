using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    float maxLifeTime = 5f;
    float MaxLifeTime { get { return maxLifeTime; } }
    float lifeTime = 0f;
    public float Lifetime { set { lifeTime = value; } }
    float _travelledDistance;
    public float DistanceTravelled { set { _travelledDistance = value; } }
    AnimationCurve _damageCurve;
    float _damage;

    //Modifiqué los parámetros para que tome un daño mínimo y un daño máximo, además de la curva de daño
    //Lo ideal sería heredar de Bullet y hacer balas para distinguir proyectil común, cargado y continuo.
    //De esta manera, podremos hacer el cálculo de daño actual (entre mínimo y máximo)
    public Bullet ConfigurateBullet(float speed, AnimationCurve curve, Vector3 position, Quaternion rotation, int emitter)
    {
        this.speed = speed;
        _damageCurve = curve;
        gameObject.layer = emitter;
        transform.position = position;
        transform.rotation = rotation;

        return this;
    }

    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxLifeTime)
        {
            BulletSpawner.Instance.ReturnBulletToPool(this);
        }
        else
        {
            var distance = speed * Time.deltaTime;
            _travelledDistance += distance;
            transform.position += transform.forward * distance;
            _damage = GetActualDamage(_travelledDistance);
            print("DMG: " + _damage);
            //Acá tendrías que setear el daño actual al análisis del falloff de acuerdo al tiempo
            //que haya transcurrido dividido por el tiempo que se tardaría en completar el falloff.
            //Para esto tendrías que pedir velocidad, daño max y min, posición, rotación, emitter, curva de falloff y tiempo de duración del mismo al instanciar la bala.
        }
    }

    float GetActualDamage(float valueToAnalyze)
    {
        return _damageCurve.Evaluate(valueToAnalyze);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer != gameObject.layer)
        {
            BulletSpawner.Instance.ReturnBulletToPool(this);
        }
    }

    public static void InitializeBullet(Bullet bulletObj)
    {
        bulletObj.gameObject.SetActive(true);
        bulletObj.Lifetime = 0f;
        bulletObj.DistanceTravelled = 0f;
    }

    public static void DisposeBullet(Bullet bulletObj)
    {
        bulletObj.gameObject.SetActive(false);
    }
}

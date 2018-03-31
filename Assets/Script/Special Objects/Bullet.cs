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
    AnimationCurve _damageCurve;
    float _damage;

    //Modifiqué los parámetros para que tome un daño mínimo y un daño máximo. 
    //Lo ideal sería heredar de Bullet y hacer balas para distinguir proyectil común, cargado y continuo.
    //De esta manera, podremos hacer el cálculo de daño actual (entre mínimo y máximo)
    public Bullet ConfigurateBullet(float speed, AnimationCurve curve, Vector3 position, Quaternion rotation, int emitter)
    {
        this.speed = speed;
        _damageCurve = curve;
        gameObject.layer = emitter;
        transform.position = position;
        transform.rotation = rotation;

        print("lacaca " + _damageCurve.Evaluate(34));

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
            transform.position += transform.forward * speed * Time.deltaTime;
            //_damage = _damageCurve.Evaluate(lifeTime);
            //Acá tendrías que setear el daño actual al análisis del falloff de acuerdo al tiempo
            //que haya transcurrido dividido por el tiempo que se tardaría en completar el falloff.
            //Para esto tendrías que pedir velocidad, daño max y min, posición, rotación, emitter, curva de falloff y tiempo de duración del mismo al instanciar la bala.
        }
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
    }

    public static void DisposeBullet(Bullet bulletObj)
    {
        bulletObj.gameObject.SetActive(false);
    }
}

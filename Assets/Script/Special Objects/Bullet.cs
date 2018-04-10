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
    Rigidbody _rb;
    float _damage;
    public float Damage { get { return _damage; } }

    //Modifiqué los parámetros para que tome un daño mínimo y un daño máximo, además de la curva de daño
    //Lo ideal sería heredar de Bullet y hacer balas para distinguir proyectil común, cargado y continuo.
    //De esta manera, podremos hacer el cálculo de daño actual (entre mínimo y máximo)
    public Bullet ConfigurateBullet(float speed, AnimationCurve curve, Vector3 position, Quaternion rotation, string emitter)
    {
        this.speed = speed;
        _damageCurve = curve;
        gameObject.tag = emitter;
        transform.position = position;
        transform.rotation = rotation;

        _rb = GetComponent<Rigidbody>();

        return this;
    }

    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxLifeTime)
        {
            BulletSpawner.Instance.ReturnBulletToPool(this);
        }
    }

    void FixedUpdate()
    {
        if (lifeTime < maxLifeTime)
        {
            var distance = speed * Time.fixedDeltaTime;
            _travelledDistance += distance;
            _rb.MovePosition(_rb.position + transform.forward * distance);
            _damage = GetActualDamage(_travelledDistance);
            //Acá tendrías que setear el daño actual al análisis del falloff de acuerdo al tiempo
            //que haya transcurrido dividido por el tiempo que se tardaría en completar el falloff.
            //Para esto tendrías que pedir velocidad, daño max y min, posición, rotación, emitter, curva de falloff y tiempo de duración del mismo al instanciar la bala.
        }
    }

    float GetActualDamage(float valueToAnalyze)
    {
        return _damageCurve.Evaluate(valueToAnalyze);
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

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("DestructibleWall") 
           ||
            col.gameObject.layer == LayerMask.NameToLayer("DestructibleStructure")
           )
        {
            col.gameObject.SetActive(false);
            BulletSpawner.Instance.ReturnBulletToPool(this);

        }
		else if (col.gameObject.layer == LayerMask.NameToLayer("Default")) BulletSpawner.Instance.ReturnBulletToPool(this);
			
    }
}

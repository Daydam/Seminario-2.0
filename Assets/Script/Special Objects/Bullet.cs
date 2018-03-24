using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    float minDamage;
    float maxDamage;
    float maxLifeTime = 5f;
    float MaxLifeTime { get { return maxLifeTime; } }
    float lifeTime = 0f;
    public float Lifetime { set { lifeTime = value; } }

    #region Cambios Iván 23/3
    //Modifiqué los parámetros para que tome un daño mínimo y un daño máximo. 
    //Lo ideal sería heredar de Bullet y hacer balas para distinguir proyectil común, cargado y continuo.
    //De esta manera, podremos hacer el cálculo de daño actual (entre mínimo y máximo)
    public Bullet ConfigurateBullet(float speed, float minDamage, float maxDamage, Vector3 position, Quaternion rotation, int emitter)
    {
        this.speed = speed;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        gameObject.layer = emitter;
        transform.position = position;
        transform.rotation = rotation;
        return this;
    }
    #endregion

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletSpawner: MonoBehaviour
{
    Bullet bulletPrefab;
    Pool<Bullet> bulletPool;
    public Pool<Bullet> BulletPool { get { return bulletPool; } }

	private static BulletSpawner instance;
	public static BulletSpawner Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<BulletSpawner>();
				if(instance == null)
				{
					instance = new GameObject("new BulletSpawner Object").AddComponent<BulletSpawner>().GetComponent<BulletSpawner>();
				}
			}
			return instance;
		}
	}

    void Awake()
    {
        instance = this;
        bulletPrefab = Resources.Load<Bullet>("Prefabs/Bullet");
        bulletPool = new Pool<Bullet>(8, Factory, Bullet.Initialize, Bullet.Dispose, true);
    }

    void Start()
    {
        GameManager.Instance.OnChangeScene += DestroyStatic;
        GameManager.Instance.OnResetRound += ResetRound;
    }

    private Bullet Factory()
    {
        var b = Instantiate<Bullet>(bulletPrefab);
        b.transform.parent = transform;
        return b;
        //return Instantiate<Bullet>(bulletPrefab);
    }

    public void ReturnToPool(Bullet bullet)
    {
        bullet.transform.parent = transform;
        bulletPool.DisablePoolObject(bullet);
    }

    void DestroyStatic()
    {
        StopAllCoroutines();
        instance = null;
        //Destroy(gameObject);
    }

    void ResetRound()
    {
        foreach (var item in BulletPool.PoolList)
        {
            ReturnToPool(item.GetObj);
        }
    }
}

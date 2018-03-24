﻿using System.Collections;
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
        bulletPool = new Pool<Bullet>(8, BulletFactory, Bullet.InitializeBullet, Bullet.DisposeBullet, true);
    }

    private Bullet BulletFactory()
    {
        return Instantiate<Bullet>(bulletPrefab);
    }

    public void ReturnBulletToPool(Bullet bullet)
    {
        bulletPool.DisablePoolObject(bullet);
    }
}

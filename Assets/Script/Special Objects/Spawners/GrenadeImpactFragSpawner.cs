using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrenadeImpactFragSpawner: MonoBehaviour
{
    DMM_GrenadeImpactFrag objPrefab;
    Pool<DMM_GrenadeImpactFrag> objPool;
    public Pool<DMM_GrenadeImpactFrag> ObjectPool { get { return objPool; } }

    private static GrenadeImpactFragSpawner instance;
	public static GrenadeImpactFragSpawner Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<GrenadeImpactFragSpawner>();
				if(instance == null)
				{
					instance = new GameObject("new GrenadeImpactFragSpawner Object").AddComponent<GrenadeImpactFragSpawner>().GetComponent<GrenadeImpactFragSpawner>();
				}
			}
			return instance;
		}
	}

    void Awake()
    {
        instance = this;
        objPrefab = Resources.Load<DMM_GrenadeImpactFrag>("Prefabs/GrenadeImpactFrag");
        objPool = new Pool<DMM_GrenadeImpactFrag>(8, Factory, DMM_GrenadeImpactFrag.InitializeBullet, DMM_GrenadeImpactFrag.DisposeBullet, true);
    }

    private DMM_GrenadeImpactFrag Factory()
    {
        var b = Instantiate<DMM_GrenadeImpactFrag>(objPrefab);
        b.transform.parent = transform;
        return b;
        //return Instantiate<DMM_GrenadeImpactFrag>(objPrefab);
    }

    public void ReturnToPool(DMM_GrenadeImpactFrag obj)
    {
        obj.transform.parent = transform;
        objPool.DisablePoolObject(obj);
    }
}

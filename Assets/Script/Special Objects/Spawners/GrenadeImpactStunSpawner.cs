using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrenadeImpactStunSpawner: MonoBehaviour
{
    DMM_GrenadeImpactStun objPrefab;
    Pool<DMM_GrenadeImpactStun> objPool;
    public Pool<DMM_GrenadeImpactStun> ObjectPool { get { return objPool; } }

    private static GrenadeImpactStunSpawner instance;
	public static GrenadeImpactStunSpawner Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<GrenadeImpactStunSpawner>();
				if(instance == null)
				{
					instance = new GameObject("new GrenadeImpactStunSpawner Object").AddComponent<GrenadeImpactStunSpawner>().GetComponent<GrenadeImpactStunSpawner>();
				}
			}
			return instance;
		}
	}

    void Awake()
    {
        instance = this;
        objPrefab = Resources.Load<DMM_GrenadeImpactStun>("Prefabs/GrenadeImpactStun");
        objPool = new Pool<DMM_GrenadeImpactStun>(8, Factory, DMM_GrenadeImpactStun.Initialize, DMM_GrenadeImpactStun.Dispose, true);
    }

    private DMM_GrenadeImpactStun Factory()
    {
        var b = Instantiate<DMM_GrenadeImpactStun>(objPrefab);
        b.transform.parent = transform;
        return b;
        //return Instantiate<DMM_GrenadeImpactStun>(objPrefab);
    }

    public void ReturnToPool(DMM_GrenadeImpactStun obj)
    {
        obj.transform.parent = transform;
        objPool.DisablePoolObject(obj);
    }
}
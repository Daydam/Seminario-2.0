using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StunMissileSpawner: MonoBehaviour
{
    DMM_StunMissile objPrefab;
    Pool<DMM_StunMissile> objPool;
    public Pool<DMM_StunMissile> ObjectPool { get { return objPool; } }

    private static StunMissileSpawner instance;
	public static StunMissileSpawner Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<StunMissileSpawner>();
				if(instance == null)
				{
					instance = new GameObject("new StunMissileSpawner Object").AddComponent<StunMissileSpawner>().GetComponent<StunMissileSpawner>();
				}
			}
			return instance;
		}
	}

    void Awake()
    {
        instance = this;

        objPrefab = Resources.Load<DMM_StunMissile>("Prefabs/Projectiles/StunMissile");
        objPool = new Pool<DMM_StunMissile>(8, Factory, DMM_StunMissile.Initialize, DMM_StunMissile.Dispose, true);
    }

    void Start()
    {
        GameManager.Instance.OnChangeScene += DestroyStatic;
        GameManager.Instance.OnResetRound += ResetRound;
    }

    private DMM_StunMissile Factory()
    {
        var b = Instantiate<DMM_StunMissile>(objPrefab);
        b.transform.parent = transform;
        return b;
    }

    public void ReturnToPool(DMM_StunMissile obj)
    {
        obj.transform.parent = transform;
        objPool.DisablePoolObject(obj);
    }

    void DestroyStatic()
    {
        StopAllCoroutines();
        instance = null;
    }

    void ResetRound()
    {
        foreach (var item in ObjectPool.PoolList)
        {
            ReturnToPool(item.GetObj);
        }
    }
}
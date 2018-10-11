using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FragmentMissileSpawner: MonoBehaviour
{
    DMM_FragmentMissile objPrefab;
    Pool<DMM_FragmentMissile> objPool;
    public Pool<DMM_FragmentMissile> ObjectPool { get { return objPool; } }

    private static FragmentMissileSpawner instance;
	public static FragmentMissileSpawner Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<FragmentMissileSpawner>();
				if(instance == null)
				{
					instance = new GameObject("new FragmentMissileSpawner Object").AddComponent<FragmentMissileSpawner>().GetComponent<FragmentMissileSpawner>();
				}
			}
			return instance;
		}
	}

    void Awake()
    {
        instance = this;
        objPrefab = Resources.Load<DMM_FragmentMissile>("Prefabs/Projectiles/FragmentMissile");
        objPool = new Pool<DMM_FragmentMissile>(8, Factory, DMM_FragmentMissile.Initialize, DMM_FragmentMissile.Dispose, true);
    }

    void Start()
    {
        GameManager.Instance.OnChangeScene += DestroyStatic;
        GameManager.Instance.OnResetRound += ResetRound;
    }

    private DMM_FragmentMissile Factory()
    {
        var b = Instantiate<DMM_FragmentMissile>(objPrefab);
        b.transform.parent = transform;
        return b;
    }

    public void ReturnToPool(DMM_FragmentMissile obj)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RepulsiveBatterySpawner : MonoBehaviour
{
    DMM_RepulsiveBattery objPrefab;
    Pool<DMM_RepulsiveBattery> objPool;
    public Pool<DMM_RepulsiveBattery> ObjectPool { get { return objPool; } }

    static RepulsiveBatterySpawner instance;
	public static RepulsiveBatterySpawner Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<RepulsiveBatterySpawner>();
				if(instance == null)
				{
					instance = new GameObject("new RepulsiveBatterySpawner Object").AddComponent<RepulsiveBatterySpawner>().GetComponent<RepulsiveBatterySpawner>();
				}
			}
			return instance;
		}
	}

    void Awake()
    {
        instance = this;
        objPrefab = Resources.Load<DMM_RepulsiveBattery>("Prefabs/Projectiles/RepulsiveBattery");
        objPool = new Pool<DMM_RepulsiveBattery>(8, Factory, DMM_RepulsiveBattery.Initialize, DMM_RepulsiveBattery.Dispose, true);
    }

    void Start()
    {
        GameManager.Instance.OnChangeScene += DestroyStatic;
        GameManager.Instance.OnResetRound += ResetRound;
    }

    DMM_RepulsiveBattery Factory()
    {
        var b = Instantiate<DMM_RepulsiveBattery>(objPrefab);
        b.transform.parent = transform;
        return b;
    }

    public void ReturnToPool(DMM_RepulsiveBattery obj)
    {
        obj.transform.parent = transform;
        objPool.DisablePoolObject(obj);
    }

    void DestroyStatic()
    {
        StopAllCoroutines();
        instance = null;
        //Destroy(gameObject);
    }

    void ResetRound()
    {
        foreach (var item in ObjectPool.PoolList)
        {
            ReturnToPool(item.GetObj);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EMPCaltropSpawner : MonoBehaviour
{
    DMM_EMPCaltrop objPrefab;
    Pool<DMM_EMPCaltrop> objPool;
    public Pool<DMM_EMPCaltrop> ObjectPool { get { return objPool; } }

    private static EMPCaltropSpawner instance;
	public static EMPCaltropSpawner Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<EMPCaltropSpawner>();
				if(instance == null)
				{
					instance = new GameObject("new EMPCaltropSpawner Object").AddComponent<EMPCaltropSpawner>().GetComponent<EMPCaltropSpawner>();
				}
			}
			return instance;
		}
	}

    void Awake()
    {
        instance = this;
        objPrefab = Resources.Load<DMM_EMPCaltrop>("Prefabs/Projectiles/EMPCaltrop");
        objPool = new Pool<DMM_EMPCaltrop>(8, Factory, DMM_EMPCaltrop.Initialize, DMM_EMPCaltrop.Dispose, true);
    }

    void Start()
    {
        GameManager.Instance.OnChangeScene += DestroyStatic;
        GameManager.Instance.OnResetRound += ResetRound;
    }

    private DMM_EMPCaltrop Factory()
    {
        var b = Instantiate<DMM_EMPCaltrop>(objPrefab);
        b.transform.parent = transform;
        return b;
    }

    public void ReturnToPool(DMM_EMPCaltrop obj)
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

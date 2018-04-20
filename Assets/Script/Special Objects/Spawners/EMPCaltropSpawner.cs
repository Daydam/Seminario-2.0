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
        objPrefab = Resources.Load<DMM_EMPCaltrop>("Prefabs/EMPCaltrop");
        objPool = new Pool<DMM_EMPCaltrop>(8, EMPCaltropFactory, DMM_EMPCaltrop.Initialize, DMM_EMPCaltrop.Dispose, true);
    }

    private DMM_EMPCaltrop EMPCaltropFactory()
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
}

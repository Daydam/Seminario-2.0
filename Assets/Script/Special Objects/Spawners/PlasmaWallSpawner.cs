using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlasmaWallSpawner : MonoBehaviour
{
    DMM_PlasmaWall objPrefab;
    Pool<DMM_PlasmaWall> objPool;
    public Pool<DMM_PlasmaWall> ObjectPool { get { return objPool; } }

    static PlasmaWallSpawner instance;
	public static PlasmaWallSpawner Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<PlasmaWallSpawner>();
				if(instance == null)
				{
					instance = new GameObject("new PlasmaWallSpawner Object").AddComponent<PlasmaWallSpawner>().GetComponent<PlasmaWallSpawner>();
				}
			}
			return instance;
		}
	}

    void Awake()
    {
        instance = this;
        objPrefab = Resources.Load<DMM_PlasmaWall>("Prefabs/PlasmaWall");
        objPool = new Pool<DMM_PlasmaWall>(8, Factory, DMM_PlasmaWall.Initialize, DMM_PlasmaWall.Dispose, true);
    }

    void Start()
    {
        GameManager.Instance.OnChangeScene += DestroyStatic;
        GameManager.Instance.OnResetRound += ResetRound;
    }

    DMM_PlasmaWall Factory()
    {
        var b = Instantiate<DMM_PlasmaWall>(objPrefab);
        b.transform.parent = transform;
        return b;
    }

    public void ReturnToPool(DMM_PlasmaWall obj)
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

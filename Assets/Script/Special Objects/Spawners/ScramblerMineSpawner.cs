using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScramblerMineSpawner : MonoBehaviour
{
    DMM_ScramblerMine objPrefab;
    Pool<DMM_ScramblerMine> objPool;
    public Pool<DMM_ScramblerMine> ObjectPool { get { return objPool; } }

    private static ScramblerMineSpawner instance;
    public static ScramblerMineSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScramblerMineSpawner>();
                if (instance == null)
                {
                    instance = new GameObject("new ScramblerMineSpawner Object").AddComponent<ScramblerMineSpawner>().GetComponent<ScramblerMineSpawner>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        objPrefab = Resources.Load<DMM_ScramblerMine>("Prefabs/ScramblerMine");
        objPool = new Pool<DMM_ScramblerMine>(8, Factory, DMM_ScramblerMine.Initialize, DMM_ScramblerMine.Dispose, true);
    }

    void Start()
    {
        GameManager.Instance.OnChangeScene += DestroyStatic;
        GameManager.Instance.OnResetRound += ResetRound;
    }

    private DMM_ScramblerMine Factory()
    {
        var b = Instantiate<DMM_ScramblerMine>(objPrefab);
        b.transform.parent = transform;
        return b;
    }

    public void ReturnToPool(DMM_ScramblerMine obj)
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

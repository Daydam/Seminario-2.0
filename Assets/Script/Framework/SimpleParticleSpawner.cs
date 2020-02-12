using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SimpleParticleSpawner : MonoBehaviour
{
    static SimpleParticleSpawner instance;
    public static SimpleParticleSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SimpleParticleSpawner>();
                if (instance == null)
                {
                    instance = new GameObject("new SimpleParticleSpawner Object").AddComponent<SimpleParticleSpawner>().GetComponent<SimpleParticleSpawner>();
                }
            }
            return instance;
        }
    }

    List<GameObject> _activeParts;
    List<GameObject> ActiveParticles
    {
        get
        {
            if (_activeParts == null) _activeParts = new List<GameObject>();
            return _activeParts;
        }

        set
        {
            _activeParts = value;
        }
    }

    public GameObject parentStoneImpact;
    public GameObject parentDustDestruction;
    public GameObject[] particles;

    public struct ParticleID
    {
        public const int BULLET = 0;
        public const int DAMAGE = 1;
        public const int MUZZLE_FLASH = 2;
        public const int REPULSIVE_BATTERY = 3;
        public const int FRAGMENT_MISSILE = 4;
        public const int STUN_MISSILE = 5;
        public const int VORTEX = 6;
        public const int PLAYER_DEATH = 7;
        public const int BULLET_STONE_IMPACT = 8;
        public const int DUST_WALL_DESTRUCTION = 9;
        public const int BLACK_HOLE_EXPLOSION = 10;
        public const int HOOK_IMPACT = 11;
    }

    #region Particles Pool

    private Pool<GameObject> _poolHitStone;
    private Pool<GameObject> _poolDustDestruction;
    private Quaternion _dir;

    private List<LifeParticleController> _activeHitStoneParticles;
    private List<LifeParticleController> _activeDustDestruction;
    private float _timer;
    private bool _destroyHitStone;

    private void Awake()
    {
        _poolHitStone = new Pool<GameObject>(30, FactoryHitStone, TurnOnObject, TurnOffObject, true);
        _poolDustDestruction = new Pool<GameObject>(10, FactoryDustDestrcution, TurnOnObject, TurnOffObject, true);
    }

    private GameObject FactoryHitStone()
    {
        var go = Instantiate<GameObject>(GetParticleByID(ParticleID.BULLET_STONE_IMPACT));
        go.transform.parent = parentStoneImpact.transform;
        return go;
    }

    private GameObject FactoryDustDestrcution()
    {
        var go = Instantiate<GameObject>(GetParticleByID(ParticleID.DUST_WALL_DESTRUCTION));
        go.transform.parent = parentDustDestruction.transform;
        return go;
    }

    private void TurnOnObject(GameObject go)
    {
        go.SetActive(true);
    }

    private void TurnOffObject(GameObject go)
    {
        go.SetActive(false);
    }

    #endregion

    void Start()
    {
        _activeHitStoneParticles = new List<LifeParticleController>();
        _activeDustDestruction = new List<LifeParticleController>();

        GameManager.Instance.OnResetRound += ResetRound;
        GameManager.Instance.OnChangeScene += ResetRound;

        EventManager.Instance.AddEventListener("SpawnDust", RecieveNormalDir);
    }

    public GameObject GetParticleByID(int id)
    {
        return particles[id];
    }

    public void SpawnParticle(GameObject part, Vector3 pos, Vector3 dir, Transform prnt = null)
    {
        var p = prnt ? GameObject.Instantiate(part, pos, Quaternion.identity, prnt) : GameObject.Instantiate(part, pos, Quaternion.identity);
        p.transform.forward = dir.normalized;
        GameObject.Destroy(p, 3);
        ActiveParticles.Add(p);
        Invoke("CleanParticles", 3.3f);
    }

    public void SpawnParticle(GameObject part, Vector3 pos, Vector3 dir, float lifeTime, Transform prnt = null)
    {
        var p = prnt ? GameObject.Instantiate(part, pos, Quaternion.identity, prnt) : GameObject.Instantiate(part, pos, Quaternion.identity);
        p.transform.forward = dir.normalized;
        GameObject.Destroy(p, lifeTime);
        ActiveParticles.Add(p);
        Invoke("CleanParticles", lifeTime + .3f);
    }

    public void SpawnParticle(GameObject part, Vector3 pos, Quaternion dir, Transform prnt = null)
    {
        var p = prnt ? GameObject.Instantiate(part, pos, dir, prnt) : GameObject.Instantiate(part, pos, dir);
        GameObject.Destroy(p, 3);
        ActiveParticles.Add(p);
        Invoke("CleanParticles", 3.3f);

    }

    public void SpawnParticle(GameObject part, Vector3 pos, Quaternion dir, float lifeTime, Transform prnt = null)
    {
        var p = prnt ? GameObject.Instantiate(part, pos, dir, prnt) : GameObject.Instantiate(part, pos, dir);
        GameObject.Destroy(p, lifeTime);
        ActiveParticles.Add(p);
        Invoke("CleanParticles", lifeTime + .3f);
    }

    public void SpawnParticle(int part, Vector3 pos, Vector3 dir, Transform prnt = null)
    {
        var p = prnt ? GameObject.Instantiate(particles[part], pos, Quaternion.identity, prnt) : GameObject.Instantiate(particles[part], pos, Quaternion.identity);
        p.transform.forward = dir.normalized;
        GameObject.Destroy(p, 3);
        ActiveParticles.Add(p);
        Invoke("CleanParticles", 3.3f);
    }

    public void SpawnParticle(int part, Vector3 pos, Vector3 dir, float lifeTime, Transform prnt = null)
    {
        var p = prnt ? GameObject.Instantiate(particles[part], pos, Quaternion.identity, prnt) : GameObject.Instantiate(particles[part], pos, Quaternion.identity);
        p.transform.forward = dir.normalized;
        GameObject.Destroy(p, lifeTime);
        ActiveParticles.Add(p);
        Invoke("CleanParticles", lifeTime + .3f);
    }

    public void SpawnParticle(int part, Vector3 pos, Quaternion dir, Transform prnt = null)
    {
        var p = prnt ? GameObject.Instantiate(particles[part], pos, dir, prnt) : GameObject.Instantiate(particles[part], pos, dir);
        GameObject.Destroy(p, 3);
        ActiveParticles.Add(p);
        Invoke("CleanParticles", 3.3f);
    }

    public void SpawnParticle(int part, Vector3 pos, Quaternion dir, float lifeTime, Transform prnt = null)
    {
        var p = prnt ? GameObject.Instantiate(particles[part], pos, dir, prnt) : GameObject.Instantiate(particles[part], pos, dir);
        GameObject.Destroy(p, lifeTime);
        ActiveParticles.Add(p);
        Invoke("CleanParticles", lifeTime + .3f);
    }

    private void RecieveNormalDir(object[] paramsContainer)
    {
        var dir = (Vector3)paramsContainer[0];
        _dir = Quaternion.LookRotation(dir);
    }

    public void SpawnDust(Vector3 pos, float lifeTime)
    {
        var p = _poolDustDestruction.GetObjectFromPool();
        p.transform.position = pos;
        p.transform.rotation = _dir;

        var lifeController = p.GetComponent<LifeParticleController>();
        lifeController.lifeTime = lifeTime;

        _activeDustDestruction.Add(lifeController);
    }

    public void SpawnParticlesImpact(Vector3 pos, Quaternion dir, float lifeTime)
    {
        var go = _poolHitStone.GetObjectFromPool();
        go.transform.position = pos;
        go.transform.rotation = dir;

        var lifeController = go.GetComponent<LifeParticleController>();
        lifeController.lifeTime = lifeTime;

        _activeHitStoneParticles.Add(lifeController);
    }

    private void Update()
    {
        CheckParticles();
    }

    private void CheckParticles()
    {
        for (int i = 0; i < _activeHitStoneParticles.Count; i++)
        {
            if (_activeHitStoneParticles[i].backToPool)
            {
                _poolHitStone.DisablePoolObject(_activeHitStoneParticles[i].gameObject);
                _activeHitStoneParticles.RemoveAt(i);
            }
        }

        for (int i = 0; i < _activeDustDestruction.Count; i++)
        {
            if (_activeDustDestruction[i].backToPool)
            {
                _poolDustDestruction.DisablePoolObject(_activeDustDestruction[i].gameObject);
                _activeDustDestruction.RemoveAt(i);
            }
        }
    }

    void CleanParticles()
    {
        ActiveParticles = ActiveParticles.Where(x => x != null).ToList();
    }

    void ResetRound()
    {
        for (int i = 0; i < ActiveParticles.Count; i++)
        {
            Destroy(ActiveParticles[i]);
        }

        ActiveParticles = null;
    }
}

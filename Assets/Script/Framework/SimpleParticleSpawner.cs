using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public GameObject[] particles;

    public struct ParticleID
    {
        public const int BULLET = 0;
        public const int DAMAGE = 1;
        public const int MUZZLEFLASH = 2;
        public const int REPULSIVEBATTERY = 3;
        public const int FRAGMENTMISSILE = 4;
        public const int STUNMISSILE = 5;
        public const int VORTEX = 6;
        public const int DEATHPARTICLE = 7;
    }

    void Start()
    {
        GameManager.Instance.OnResetRound += ResetRound;
        GameManager.Instance.OnChangeScene += ResetRound;
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

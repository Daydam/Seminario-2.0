using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsController : MonoBehaviour
{
    public List<Transform> bulletPosList;
    public Transform[] positions;

    private Vector4[] _vectorPositions;
    private Renderer _rend;
    private MaterialPropertyBlock _mpb;


    public float destroyAfter = 2.5f;
    private float timer;


    private void Awake()
    {
        _rend = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();
    }

    private void Start()
    {
        timer = destroyAfter;
        bulletPosList = new List<Transform>();
        _vectorPositions = new Vector4[positions.Length];
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            timer = Random.Range(1.0f, destroyAfter);

            HitPlasmaWall(positions[Random.Range(0, positions.Length)]);
        }
    }

    private void HitPlasmaWall(Transform bullet)
    {
        if(!bulletPosList.Contains(bullet))
            bulletPosList.Add(bullet);


        for (int i = 0; i < bulletPosList.Count; i++)
        {
            if(bulletPosList[i] == bullet)
            {
                _vectorPositions[i] = new Vector4(bulletPosList[i].position.x,
                                               bulletPosList[i].position.y,
                                               bulletPosList[i].position.z, 0);
            }
        }

        _mpb.SetVectorArray("myObjects", _vectorPositions);
        _rend.SetPropertyBlock(_mpb);

        for (int i = 0; i < bulletPosList.Count; i++)
        {
            StartCoroutine(FadeAfter(i));
        }
    }

    IEnumerator FadeAfter(int index)
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 2.0f));

        for (int i = 0; i < bulletPosList.Count; i++)
        {
            if(i == index)
            {
                _vectorPositions[i] = new Vector4(999.0f, 999.0f, 999.0f, 0);

                _mpb.SetVectorArray("myObjects", _vectorPositions);
                _rend.SetPropertyBlock(_mpb);

            }
        }
    }
}

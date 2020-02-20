using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DMM_PlasmaWallPointsController : MonoBehaviour
{
    public List<Vector3> hitPositionsList;

    List<Vector4> _vectorPositions;
    Renderer _rend;
    MaterialPropertyBlock _mpb;


    public float destroyAfter = 2.5f;
    float _timer;


    void Awake()
    {
        _vectorPositions = new List<Vector4>();
        _rend = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();
    }

    void Start()
    {
        _timer = destroyAfter;
        hitPositionsList = new List<Vector3>();
    }

    void Update()
    {
        /*timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            timer = Random.Range(1.0f, destroyAfter);
        }*/
    }

    public void HitPlasmaWall(Vector3 hitPos)
    {
        if(!hitPositionsList.Contains(hitPos))
            hitPositionsList.Add(hitPos);


        for (int i = 0; i < hitPositionsList.Count; i++)
        {
            if(hitPositionsList[i] == hitPos)
            {
                _vectorPositions.Add(new Vector4(hitPositionsList[i].x,
                                               hitPositionsList[i].y,
                                               hitPositionsList[i].z, 0));
            }
        }

        _mpb.SetVectorArray("myObjects", _vectorPositions.ToArray());
        _rend.SetPropertyBlock(_mpb);

        for (int i = 0; i < hitPositionsList.Count; i++)
        {
            StartCoroutine(FadeAfter(i));
        }
    }

    IEnumerator FadeAfter(int index)
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 2.0f));

        for (int i = 0; i < hitPositionsList.Count; i++)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EmpCloud : MonoBehaviour
{
    public Collider col;
    public Vector3 minimumScale = new Vector3(22.79417f, 22.79417f, 22.79417f);
    public Vector3 initialScale;
    public float shrinkSpeed = .3f;
    bool _shrink;

    void Start()
    {
        col = GetComponent<Collider>();
        initialScale = transform.localScale;
        StartCoroutine(StartShrinking(1));
        //GameManager.Instance.OnResetGame += Clean;
    }

    void Update()
    {
        if (Vector3.Distance(transform.localScale, minimumScale) <= .1f)
        {
            _shrink = false;
            transform.localScale = minimumScale;
            return;
        }
        else if (_shrink)
        {
            if (!StageManager.instance.isLast) CheckAntennas();
            transform.localScale -= new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime);
        }
    }

    void CheckAntennas()
    {
        if (!StageManager.instance.levelRings[StageManager.instance.actualRing].antennas.Where(x => col.bounds.Contains(x.position)).Any())
        {
            StageManager.instance.DestroyRing();
        }
    }

    public void ResetRound()
    {
        transform.localScale = initialScale;
        StartCoroutine(StartShrinking(1));
    }


    IEnumerator StartShrinking(float t)
    {
        yield return new WaitForSeconds(t);
        _shrink = true;
    }

    void Clean()
    {
        Destroy(gameObject);
    }
}

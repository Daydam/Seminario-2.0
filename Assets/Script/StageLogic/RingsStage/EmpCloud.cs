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
    RingsStage _stage;

    void Start()
    {
        col = GetComponent<Collider>();
        initialScale = transform.localScale;
        GameManager.Instance.StartRound += () => StartCoroutine(StartShrinking(1));
        _stage = StageManager.instance.stage as RingsStage;
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
            if (_stage) if (!_stage.isLast) CheckAntennas();
            transform.localScale -= new Vector3(shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime, shrinkSpeed * Time.deltaTime);
        }
    }

    void CheckAntennas()
    {
        if (_stage)
        {
            if (!_stage.levelRings[_stage.actualRing].antennas
            .Where(x => col.bounds.Contains(x.transform.position)).Any())
            {
                _stage.DestroyRing();
            }
        }
    }

    public void ResetRound()
    {
        transform.localScale = initialScale;
    }

    IEnumerator StartShrinking(float t)
    {
        yield return new WaitForSeconds(t);
        _shrink = true;
    }
}

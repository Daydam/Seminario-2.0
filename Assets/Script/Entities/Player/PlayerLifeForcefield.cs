using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerLifeForcefield : MonoBehaviour
{
    Renderer _rend;
    Player _owner;
    Coroutine _actualCorr;

    void Start()
    {
        _owner = GetComponentInParent<Player>();
        _rend = GetComponent<Renderer>();
        GameManager.Instance.OnResetRound += () => _rend.material.SetFloat("_HologramOpacity", 0);
    }

    void Update()
    {
        _rend.material.SetFloat("_Life", _owner.LightsModule.GetLifeValue());
    }

    public void TakeDamage()
    {
        if (_actualCorr != null) StopCoroutine(_actualCorr);

        _actualCorr = StartCoroutine(HologramVisible());
    }

    public void TakeDamage(Vector3 hitPos)
    {
        if (_actualCorr != null) StopCoroutine(_actualCorr);

        _actualCorr = StartCoroutine(HologramVisible(hitPos));
    }

    IEnumerator HologramVisible()
    {
        var curve = new AnimationCurve();
        float start = 0f, mid = .1f, end = .2f;

        curve.AddMultipleKeys(new Keyframe(start, 0), new Keyframe(mid, 1), new Keyframe(end, 0));

        var elapsed = 0f;

        var inst = new WaitForFixedUpdate();

        while (elapsed <= end)
        {
            _rend.material.SetFloat("_HologramOpacity", curve.Evaluate(elapsed));

            yield return inst;

            elapsed += Time.fixedDeltaTime;
        }
    }

    IEnumerator HologramVisible(Vector3 hitPos)
    {
        var curve = new AnimationCurve();
        float start = 0f, mid = .1f, end = .2f;

        curve.AddMultipleKeys(new Keyframe(start, 0), new Keyframe(mid, 1), new Keyframe(end, 0));

        var elapsed = 0f;

        var inst = new WaitForFixedUpdate();

        _rend.material.SetVector("_HitPosition", transform.InverseTransformPoint(hitPos));
        var hitTime = end;

        while (elapsed <= end)
        {
            _rend.material.SetFloat("_HologramOpacity", curve.Evaluate(elapsed));
            _rend.material.SetFloat("_HitTime", hitTime);
            yield return inst;
            hitTime -= Time.fixedDeltaTime;
            elapsed += Time.fixedDeltaTime;
        }
    }
}

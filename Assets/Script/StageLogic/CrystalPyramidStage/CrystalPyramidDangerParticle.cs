using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PhoenixDevelopment;

public class CrystalPyramidDangerParticle : MonoBehaviour
{
    public LineRenderer laser;

    public GameObject laserStartParticlePrefab;
    GameObject _laserStartParticleCurrent;

    public GameObject floorHitParticle;

    AnimationCurve _anCurveX, _anCurveY, _anCurveZ;

    public float laserSpawnDelay, laserToFloorDelay;

    Coroutine _coroutine;

    public TrailRenderer wallOfLight;

    void Start()
    { 
        laser = transform.GetComponentInChildren<LineRenderer>(true);
        wallOfLight = transform.parent.GetComponentInChildren<TrailRenderer>(true);

        floorHitParticle.SetActive(false);
        laser.enabled = false;
        wallOfLight.Clear();
        wallOfLight.enabled = false;
    }

    public void Initialize(Transform[] laserPoints)
    {
        _anCurveX = new AnimationCurve(new Keyframe(0, transform.position.x));
        _anCurveY = new AnimationCurve(new Keyframe(0, transform.position.y));
        _anCurveZ = new AnimationCurve(new Keyframe(0, transform.position.z));

        for (int i = 0; i < laserPoints.Length; i++)
        {
            _anCurveX.AddKey(new Keyframe(i + 1, laserPoints[i].position.x));
            _anCurveY.AddKey(new Keyframe(i + 1, laserPoints[i].position.y));
            _anCurveZ.AddKey(new Keyframe(i + 1, laserPoints[i].position.z));
        }

        _anCurveX.AddKey(new Keyframe(laserPoints.Length + 1, laserPoints[0].position.x));
        _anCurveY.AddKey(new Keyframe(laserPoints.Length + 1, laserPoints[0].position.y));
        _anCurveZ.AddKey(new Keyframe(laserPoints.Length + 1, laserPoints[0].position.z));
    }

    public void StartDanger(float t)
    {
        if (_coroutine != null) { StopCoroutine(_coroutine); }
        _coroutine = StartCoroutine(LaserBehaviourHandler(t));
    }

    IEnumerator LaserBehaviourHandler(float t)
    {
        //actual danger time without setting up of feedback
        var dangerTime = t - laserSpawnDelay;

        //Spawn lightning ball on top of the obelisk and wait for it to end

        if (_laserStartParticleCurrent != null) Destroy(_laserStartParticleCurrent);

        _laserStartParticleCurrent = Instantiate(laserStartParticlePrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(laserSpawnDelay);

        Destroy(_laserStartParticleCurrent);

        //Spawn laser and make it grow from the top of the obelisk to the ground
        laser.enabled = true;

        float _currentTimeElapsed = 0;

        laser.SetPositions(new Vector3[] { transform.position, transform.position });

        var newFloorPos = new Vector3(_anCurveX.Evaluate(0), _anCurveY.Evaluate(0), _anCurveZ.Evaluate(0));

        while (_currentTimeElapsed < laserToFloorDelay)
        {
            yield return new WaitForEndOfFrame();
            _currentTimeElapsed += Time.deltaTime;

            var newPosVkt = Vector3.Lerp(transform.position, newFloorPos, _currentTimeElapsed / laserToFloorDelay);
            laser.SetPosition(1, newPosVkt);
        }

        //Activate particle on the floor and start moving the ground position
        floorHitParticle.SetActive(true);

        wallOfLight.Clear();
        wallOfLight.transform.position = new Vector3(_anCurveX.Evaluate(1), _anCurveY.Evaluate(1), _anCurveZ.Evaluate(1));
        wallOfLight.transform.eulerAngles = new Vector3(0, 90, 0);
        wallOfLight.enabled = true;
        wallOfLight.Clear();

        //Get time to get to the next index of the laser points
        var laserToPointTime = dangerTime / _anCurveX.length;

        for (int i = 1; i < _anCurveX.length; i++)
        {
            _currentTimeElapsed = 0;
            var currentLaserPos = new Vector3(_anCurveX.Evaluate(i - 1), _anCurveY.Evaluate(i - 1), _anCurveZ.Evaluate(i - 1));

            while (_currentTimeElapsed < laserToPointTime)
            {
                yield return new WaitForEndOfFrame();
                _currentTimeElapsed += Time.deltaTime;

                newFloorPos = new Vector3(_anCurveX.Evaluate(i), _anCurveY.Evaluate(i), _anCurveZ.Evaluate(i));

                var newPosVkt = Vector3.Lerp(currentLaserPos, newFloorPos, _currentTimeElapsed / laserToPointTime);
                laser.SetPosition(1, newPosVkt);
                floorHitParticle.transform.position = newPosVkt;

                wallOfLight.transform.position = floorHitParticle.transform.position + Vector3.up;
                wallOfLight.transform.eulerAngles = new Vector3(0, 90, 0);
            }
        }

        //Move laser back to the top of the obelisk
        _currentTimeElapsed = 0;

        var finalGroundPosition = laser.GetPosition(1);

        while (_currentTimeElapsed < laserToFloorDelay)
        {
            yield return new WaitForEndOfFrame();
            _currentTimeElapsed += Time.deltaTime;

            var newPosVkt = Vector3.Lerp(finalGroundPosition, transform.position, _currentTimeElapsed / laserToFloorDelay);
            laser.SetPosition(1, newPosVkt);
        }

        //End
        _currentTimeElapsed = 0;
        floorHitParticle.SetActive(false);
        laser.enabled = false;
    }

    public void ResetRound()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        StopAllCoroutines();
        CancelInvoke();

        if (_laserStartParticleCurrent != null) Destroy(_laserStartParticleCurrent);
        floorHitParticle.SetActive(false);
        laser.enabled = false;
        for (int i = 0; i < wallOfLight.positionCount; i++)
        {
            wallOfLight.SetPosition(i, Vector3.zero);
        }
        wallOfLight.enabled = false;
    }

}

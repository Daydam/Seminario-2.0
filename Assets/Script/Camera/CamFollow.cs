using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Drone rear camera
/// </summary>
public class CamFollow : MonoBehaviour
{
    bool running;
    bool _playerDead;
    public Vector3 positionOffset;
    public Vector3 aimOffset;
    [Range(0, 1)]
    public float movementSpeed;
    [Range(0, 1)]
    public float aimSpeed;

    Coroutine _cameraFallCoroutine;
    Camera _cam;

    readonly float _maxFallDuration = .5f, _maxRayDistance = 2;

    Player target;
    Vector3 targetSight;
    RaycastHit _toFloorRay;

    void Start()
    {
        _cam = GetComponent<Camera>();
        GameManager.Instance.OnResetRound += ResetRound;
    }

    public void AssignTarget(Player target)
    {
        this.target = target;
        transform.position = target.transform.position;
        transform.forward = target.transform.forward;
        targetSight = transform.forward;
        target.AssignCamera(this);
        _playerDead = false;
        running = true;
    }

    public void ResetRound()
    {
        StopAllCoroutines();
        _cameraFallCoroutine = null;
        _playerDead = false;
        running = true;
    }

    public void OnPlayerDeath()
    {
        Physics.Raycast(target.transform.position, Vector3.down, out _toFloorRay, _maxRayDistance);
        _playerDead = true;
    }

    private void FixedUpdate()
    {
        if (running)
        {
            if (!_playerDead && target) CameraMovement();
            else if (_cameraFallCoroutine == null) _cameraFallCoroutine = StartCoroutine(CameraFall());
        }
    }

    void CameraMovement()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + target.transform.right * positionOffset.x + target.transform.up * positionOffset.y + target.transform.forward * positionOffset.z, movementSpeed);
        targetSight = Vector3.Lerp(targetSight, target.transform.position + target.transform.right * aimOffset.x + target.transform.up * aimOffset.y + target.transform.forward * aimOffset.z, aimSpeed);
        transform.LookAt(targetSight);
    }

    IEnumerator CameraFall()
    {
        var elapsed = 0f;
        var moveDelta = (Physics.gravity / 2) * Time.fixedDeltaTime;
        var yieldInstruction = new WaitForFixedUpdate();

        while (elapsed <= _maxFallDuration)
        {
            transform.position += moveDelta;

            yield return yieldInstruction;
            elapsed += Time.fixedDeltaTime;

            if (_toFloorRay.point != null && Vector3.Distance(transform.position, _toFloorRay.point) < 0.01f)
            {
                EnableDeathCam();
                yield break;
            }

        }
        EnableDeathCam();
    }

    void EnableDeathCam()
    {
        //do the thing with canvas
    }
}

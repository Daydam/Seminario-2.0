using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PPFX_RepulsionScreen : MonoBehaviour
{
    public Material imageFX;
    float _effectDuration;

    Coroutine _runningRoutine;

    void Start()
    {
        GameManager.Instance.OnResetRound += OnResetRound;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, imageFX);
    }

    public void DeactivatePostProcess()
    {
        imageFX.SetFloat("_Activation", 0);
    }

    public void ActivatePostProcess(bool activation, Vector3 playerPosition, float radius, float duration)
    {
        SetRepulsionValues(playerPosition, radius);
        imageFX.SetFloat("_Activation", activation ? 1 : 0);
        _effectDuration = duration;
    }

    public void SetRepulsionValues(Vector3 playerPosition, float radius)
    {
        imageFX.SetVector("_PlayerPosition", playerPosition);
        imageFX.SetFloat("_RepulsionRadius", radius);
    }

    public void SetRepulsionValues(Vector3 playerPosition)
    {
        imageFX.SetVector("_PlayerPosition", playerPosition);
    }

    public void SetRepulsionValues(float radius)
    {
        imageFX.SetFloat("_RepulsionRadius", radius);
    }

    void OnResetRound()
    {
        DeactivatePostProcess();
        StopCoroutine(_runningRoutine);
        _runningRoutine = null;
    }
}

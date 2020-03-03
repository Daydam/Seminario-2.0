using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PPFX_BlinkPostProcess : MonoBehaviour
{
    public Material imageFX;
    bool _activated;
    float temp = 0f;

    void Start()
    {
        GameManager.Instance.OnResetRound += () => ActivatePostProcess(false);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, imageFX);
    }

    void Update()
    {
        if (_activated)
        {
            temp += Time.deltaTime;
            imageFX.SetFloat("_Activation", temp);
        }
        else
        {
            temp = 0;
        }
        imageFX.SetFloat("_Activation", temp);
    }

    public void ActivatePostProcess(bool activation)
    {
        _activated = activation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Firepower.Events;

public class PPFX_EMPScramble : MonoBehaviour
{
    public Material imageFX;

    void Start()
    {
        GameManager.Instance.OnResetRound += () => ActivateEMPFX(false);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, imageFX);
    }

    public void ActivateEMPFX(bool activation)
    {
    	imageFX.SetFloat("_Activation", activation ? 1 : 0);
    }
}

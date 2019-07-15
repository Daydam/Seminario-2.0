using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PPFX_EMPScramble : MonoBehaviour
{
    public Material imageFX;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, imageFX);
    }

    public void ActivateEMPFX(bool activation)
    {
        print("idioita " + activation);
    	imageFX.SetFloat("_Activation", activation ? 1 : 0);
    }
}

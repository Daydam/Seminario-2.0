using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PPFX_EMPScramble : MonoBehaviour
{
    public Material imageFX;
    /*[Range(0, 1)]
    public float alphaScrambled;
    public float panSpeed;*/

    void OnEnable()
    {
    	ActivatePostProcess(true);
        StartCoroutine(TuViejaEnTanga());
    }

    void OnDisable()
    {
    	StopAllCoroutines();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, imageFX);
    }

    void Update()
    {
        /*imageFX.SetFloat("_AlphaScramble", alphaScrambled);
        imageFX.SetFloat("_PanSpeed", panSpeed);*/
    }

    public void ActivatePostProcess(bool activation)
    {
    	imageFX.SetInt("_Activation", activation ? 1 : 0);
    }

    IEnumerator TuViejaEnTanga()
    {
        while(true)
        {
            yield return new WaitForSeconds(.3f);
            var rnd = Random.Range(0, .15f);
            imageFX.SetFloat("_GlitchStrenght", rnd);
            //imageFX.SetFloat("_ChromaticAberrationStrenght", rnd/100);
        }
    }
}

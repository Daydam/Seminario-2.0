using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrystalPyramidFallHandler : MonoBehaviour
{
    public ParticleSystem obeliskParticle;
    ParticleSystemRenderer _obeliskParticleRenderer;
    public Renderer obeliskRenderer;
    public Animator centerLight;
    Light _obeliskLight;

	public void Awake()
	{
        _obeliskLight = centerLight.GetComponent<Light>();

        _obeliskParticleRenderer = obeliskParticle.GetComponent<ParticleSystemRenderer>();
        _obeliskParticleRenderer.material.EnableKeyword("_EMISSION");

        obeliskRenderer.material.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        obeliskRenderer.material.SetColor("_EmissionColor", _obeliskLight.intensity * _obeliskLight.color);
        _obeliskParticleRenderer.material.SetColor("_EmissionColor", _obeliskLight.color);
    }

    public void SetDanger()
    {
        centerLight.SetTrigger("StartFall");
    }

    public void SetEndFall()
    {
        centerLight.SetTrigger("EndFall");
    }

    public void ResetRound()
    {
        centerLight.ResetTrigger("StartFall");
        centerLight.ResetTrigger("EndFall");
        centerLight.CrossFadeInFixedTime("Normal", .1f);
    }
}

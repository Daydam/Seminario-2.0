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

    FourPartMinimap _minimap;

    public FourPartMinimap Minimap
    {
        get
        {
            if (_minimap == null) _minimap = UIManager.Instance.stageCanvas.transform.Find(GameManager.Instance.Players.Count + " Player").GetComponentInChildren<FourPartMinimap>(); ;
            return _minimap;
        }

        set
        {
            _minimap = value;
        }
    }

    void Awake()
    {
        _obeliskLight = centerLight.GetComponent<Light>();

        _obeliskParticleRenderer = obeliskParticle.GetComponent<ParticleSystemRenderer>();
        _obeliskParticleRenderer.material.EnableKeyword("_EMISSION");

        obeliskRenderer.material.EnableKeyword("_EMISSION");
    }

    void Start()
    {
        Minimap = UIManager.Instance.stageCanvas.transform.Find(GameManager.Instance.Players.Count + " Player").GetComponentInChildren<FourPartMinimap>(); ;
    }

    void Update()
    {
        obeliskRenderer.material.SetColor("_EmissionColor", _obeliskLight.intensity * _obeliskLight.color);
        _obeliskParticleRenderer.material.SetColor("_EmissionColor", _obeliskLight.color);
    }

    /// <summary>
    /// Recieves 0 to 3
    /// </summary>
    /// <param name="indx"> 0 to 3 </param>
    public void SetDangerState(int romboidIndex)
    {
        centerLight.SetTrigger("StartFall");
        Minimap.SetDangerState(romboidIndex);
    }

    /// <summary>
    /// Recieves 0 to 3
    /// </summary>
    /// <param name="indx"> 0 to 3 </param>
    public void SetFallState(int romboidIndex)
    {
        centerLight.SetTrigger("EndFall");
        Minimap.SetFallState(romboidIndex);
    }

    public void ResetRound()
    {
        centerLight.ResetTrigger("StartFall");
        centerLight.ResetTrigger("EndFall");
        centerLight.CrossFadeInFixedTime("Normal", .1f);
    }
}

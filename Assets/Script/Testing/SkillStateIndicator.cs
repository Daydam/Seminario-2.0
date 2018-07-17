using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillStateIndicator : MonoBehaviour
{
    MeshRenderer _rend;
    public Dictionary<SkillState, Color> moduleFeedback;
    public SkillBase mySkill;
    bool _intermitentLighting;

    void Start()
    {
        _rend = GetComponent<MeshRenderer>();
        InitModuleColors();
    }

    public void InitializeIndicator(SkillBase skill)
    {
        mySkill = skill;
    }

    public void InitModuleColors()
    {
        moduleFeedback = new Dictionary<SkillState, Color>();
        moduleFeedback.Add(SkillState.Unavailable, Color.red);
        moduleFeedback.Add(SkillState.Available, Color.green);
        moduleFeedback.Add(SkillState.Reloading, Color.green);
        moduleFeedback.Add(SkillState.Active, Color.yellow);
        moduleFeedback.Add(SkillState.UserDisabled, Color.black);
    }

    void Update()
    {
        if (mySkill) CheckActualState();
    }

    public void CheckActualState()
    {
        ApplyStateFeedback(mySkill.GetActualState());
    }

    public void ApplyStateFeedback(SkillState state)
    {
        if (_intermitentLighting)
        {
            if (state != SkillState.Reloading)
            {
                _rend.material.EnableKeyword("_EMISSION");
                _rend.material.SetColor("_EmissionColor", moduleFeedback[state]);
            }
        }
        else
        {
            if (state != SkillState.Reloading)
            {
                _rend.material.EnableKeyword("_EMISSION");
                _rend.material.SetColor("_EmissionColor", moduleFeedback[state]);
            }
            else StartCoroutine(IntermitentLighting());
        }

        
    }

    IEnumerator IntermitentLighting()
    {
        _intermitentLighting = true;
        while (mySkill.GetActualState() == SkillState.Reloading)
        {
            _rend.material.EnableKeyword("_EMISSION");
            _rend.material.SetColor("_EmissionColor", moduleFeedback[SkillState.Reloading]);
            yield return new WaitForSeconds(.4f);

            _rend.material.EnableKeyword("_EMISSION");
            _rend.material.SetColor("_EmissionColor", Color.cyan);

            yield return new WaitForSeconds(.4f);
        }
        _intermitentLighting = false;

    }

}

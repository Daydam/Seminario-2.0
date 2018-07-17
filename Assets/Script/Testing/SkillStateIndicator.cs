using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillStateIndicator : MonoBehaviour
{
    MeshRenderer _rend;
    public Dictionary<SkillState, Color> moduleFeedback;
    public SkillBase mySkill;

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
        ApplyStateFeedback(moduleFeedback[mySkill.GetActualState()]);
    }

    public void ApplyStateFeedback(Color color)
    {
        _rend.material.EnableKeyword("_EMISSION");
        _rend.material.SetColor("_EmissionColor", color);
    }

}

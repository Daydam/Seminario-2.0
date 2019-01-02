using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillStateIndicator : MonoBehaviour
{
    Renderer[] _rends;
    public Dictionary<SkillState, Color> moduleFeedback;
    public SkillBase mySkill;
    public readonly string shaderTag = "SkillStateColor";
    public readonly string defensiveTagValue = "Defensive";
    public readonly string compTagValue = "Complementary";

    public void InitializeIndicator(SkillBase skill, Renderer[] renderer, bool defensive)
    {
        var tagValue = defensive ? defensiveTagValue : compTagValue;
        _rends = renderer.Where(x => x.material.GetTag(shaderTag, true, "Nothing") == tagValue).ToArray();
        mySkill = skill;
        InitModuleColors();
    }

    public void InitModuleColors()
    {
        moduleFeedback = new Dictionary<SkillState, Color>
        {
            { SkillState.Unavailable, Color.red },
            { SkillState.Available, Color.cyan },
            { SkillState.Active, Color.yellow },
            { SkillState.UserDisabled, Color.black }
        };
    }

    void Update()
    {
        if (mySkill) CheckActualState();
    }

    public void CheckActualState()
    {
        if (_rends != null && _rends.Any()) ApplyStateFeedback(mySkill.GetActualState());
    }

    public void ApplyStateFeedback(SkillState state)
    {
        foreach (var item in _rends)
        {
            item.material.SetColor("_SkillStateColor", moduleFeedback[state]);
        }

    }
}

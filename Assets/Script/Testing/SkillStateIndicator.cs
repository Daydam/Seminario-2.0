using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillStateIndicator : MonoBehaviour
{
    Renderer _rend;
    public Dictionary<SkillState, Color> moduleFeedback;
    public SkillBase mySkill;
    bool _intermitentLighting;

    /// <summary>
    /// Use for complementary
    /// </summary>
    /// <param name="skill"></param>
    public void InitializeIndicator(SkillBase skill)
    {
        mySkill = skill;
        _rend = mySkill.GetComponentInChildren<Renderer>();
        InitModuleColors();
    }

    /// <summary>
    /// Use for defensive
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="renderer"></param>
    public void InitializeIndicator(SkillBase skill, Renderer renderer)
    {
        _rend = renderer;
        mySkill = skill;
        InitModuleColors();
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
#if UNITY_EDITOR
        if (_rend) ApplyStateFeedback(mySkill.GetActualState());
#else
        ApplyStateFeedback(mySkill.GetActualState());
#endif

    }

    public void ApplyStateFeedback(SkillState state)
    {
        if (_intermitentLighting)
        {
            if (state != SkillState.Reloading)
            {
                _rend.material.SetColor("_SkillStateColor", moduleFeedback[state]);
            }
        }
        else
        {
            if (state != SkillState.Reloading)
            {
                _rend.material.SetColor("_SkillStateColor", moduleFeedback[state]);
            }
            else StartCoroutine(IntermitentLighting());
        }


    }

    IEnumerator IntermitentLighting()
    {
        _intermitentLighting = true;
        while (mySkill.GetActualState() == SkillState.Reloading)
        {
            _rend.material.SetColor("_SkillStateColor", moduleFeedback[SkillState.Reloading]);
            yield return new WaitForSeconds(.4f);

            _rend.material.SetColor("_SkillStateColor", Color.cyan);
            yield return new WaitForSeconds(.4f);
        }
        _intermitentLighting = false;

    }

}

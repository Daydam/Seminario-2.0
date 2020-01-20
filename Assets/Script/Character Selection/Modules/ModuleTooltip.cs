using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class ModuleTooltip
{
    public CanvasModuleTooltip tooltip;
    public Camera playerCam;

    public enum ModuleType { Body, Weapon, Defensive, Comp1, Comp2 }

    string _tag;

    public ModuleType moduleType;

    public ModuleTooltip(string tag, ModuleType type)
    {
        _tag = tag;
        moduleType = type;
        tooltip = FindTooltipInCanvas();
        tooltip.gameObject.SetActive(false);
        playerCam = GameObject.Find(_tag + " CamPod").GetComponentInChildren<Camera>();
    }

    public bool Visible() { return tooltip.isActiveAndEnabled; }

    public void EnableViewing(Transform point)
    {
        //canvasTooltip pos
        tooltip.gameObject.SetActive(true);
        SetTooltipPosition(point);
    }

    public void DisableViewing()
    {
        tooltip.gameObject.SetActive(false);
    }

    void SetTooltipPosition(Transform module)
    {
        tooltip.SetPosition(playerCam, module.transform.position);
    }

    public void SetModuleName(string moduleName)
    {
        var finalName = "< " + moduleName + " >";

        tooltip.SetModuleName(finalName);
    }

    public CanvasModuleTooltip FindTooltipInCanvas()
    {
        var parnt = GameObject.Find(_tag + " Text BG").transform;
        string prefix;
        switch (moduleType)
        {
            case ModuleType.Body:
                prefix = "Body";
                break;
            case ModuleType.Weapon:
                prefix = "Weapon";
                break;
            case ModuleType.Defensive:
                prefix = "Defensive";
                break;
            case ModuleType.Comp1:
                prefix = "Comp1";
                break;
            case ModuleType.Comp2:
                prefix = "Comp2";
                break;
            default:
                prefix = "NULL";
                Debug.LogError("ModuleType not set: " + _tag);
                break;
        }

        return parnt.Find(prefix + "Tooltip").GetComponent<CanvasModuleTooltip>();
    }
}

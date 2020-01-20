using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class CanvasModuleTooltip : MonoBehaviour
{
    TextMeshPro _type, _name;
    SpriteRenderer _button, _line;
    Animator _an;

    void Awake()
    {
        _an = GetComponent<Animator>();

        var textes = GetComponentsInChildren<TextMeshPro>();
        var images = GetComponentsInChildren<SpriteRenderer>();

        //En el mismo orden que se ve en la jerarquía
        _button = images.Where(x => x.name == "Button").FirstOrDefault();
        _type = textes.Where(x => x.name == "Type").FirstOrDefault();
        _name = textes.Where(x => x.name == "Name").FirstOrDefault();
        _line = images.Where(x => x.name == "Line").FirstOrDefault();
    }

    void OnEnable()
    {
        //do anims
        _an.Play("In");
    }

    public void SetPosition(Camera playerCam, Vector3 position)
    {
        transform.position = position;
        transform.LookAt(playerCam.transform);
    }

    public void SetModuleName(string moduleText)
    {
        _name.text = moduleText;
    }
}

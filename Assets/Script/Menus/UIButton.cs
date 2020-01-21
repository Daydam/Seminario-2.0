using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    Image imgComp;
    public Color regularColor = new Color(0.9f, 0.9f, 0.9f);
    public Color highlightedColor = new Color(0.7f,0.7f,0.7f);
    public Color clickedColor = new Color(0.3f,0.3f,0.3f);

    private void Awake()
    {
        imgComp = GetComponent<Image>();
    }

    public void SetHighlighted(bool state)
    {
        if (state) imgComp.color = highlightedColor;
        else imgComp.color = regularColor;
    }

    public void SetPressed(bool state)
    {
        if (state) imgComp.color = clickedColor;
        else imgComp.color = highlightedColor;
    }
    
}

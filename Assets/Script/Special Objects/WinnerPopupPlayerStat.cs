using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinnerPopupPlayerStat : MonoBehaviour
{
    Animator _an;
    TextMeshProUGUI _statName;
    Image _statImage;

    void Start()
    {
        _an = GetComponent<Animator>();
    }

    public void SetStatInfo(string statName, Sprite statImage)
    {
        _statName.text = statName;
        _statImage.sprite = statImage;
    }
}

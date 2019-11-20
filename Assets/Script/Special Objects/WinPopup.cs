using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinPopup : MonoBehaviour
{
    WinnerPopupPlayerStat _body, _def, _wpn, _skill1, _skill2;
    TextMeshProUGUI _winnerText;

    void Awake()
    {
        _winnerText = transform.Find("Winner").GetComponent<TextMeshProUGUI>();

        var playerStatsContainer = transform.Find("Stats");


        _body = playerStatsContainer.transform.Find("Body").GetComponent<WinnerPopupPlayerStat>();
        _def = playerStatsContainer.transform.Find("Defensive").GetComponent<WinnerPopupPlayerStat>();
        _wpn = playerStatsContainer.transform.Find("Weapon").GetComponent<WinnerPopupPlayerStat>();
        _skill1 = playerStatsContainer.transform.Find("Skill1").GetComponent<WinnerPopupPlayerStat>();
        _skill2 = playerStatsContainer.transform.Find("Skill2").GetComponent<WinnerPopupPlayerStat>();
    }

    public void Initialize(Player body, DefensiveSkillBase def, Weapon wpn, ComplementarySkillBase skill1, ComplementarySkillBase skill2)
    {
        var winnerName = _winnerText.text.Replace("[PLAYERNAME]", "Player " + (body.myID + 1));

        _winnerText.text = winnerName;

        _body.SetStatInfo(body.GetBodyName(), body.GetComponent<UI_ImageAssets>().iconAsset);
        _def.SetStatInfo(def.GetSkillName(), def.GetComponent<UI_ImageAssets>().iconAsset);
        _wpn.SetStatInfo(wpn.GetWeaponName(), wpn.GetComponent<UI_ImageAssets>().iconAsset);
        _skill1.SetStatInfo(skill1.GetSkillName(), skill1.GetComponent<UI_ImageAssets>().iconAsset);
        _skill2.SetStatInfo(skill2.GetSkillName(), skill2.GetComponent<UI_ImageAssets>().iconAsset);
    }
}

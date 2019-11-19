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
		_body = transform.Find("Body").GetComponent<WinnerPopupPlayerStat>();
        _def = transform.Find("Defensive").GetComponent<WinnerPopupPlayerStat>();
        _wpn = transform.Find("Weapon").GetComponent<WinnerPopupPlayerStat>();
        _skill1 = transform.Find("Skill1").GetComponent<WinnerPopupPlayerStat>();
        _skill2 = transform.Find("Skill2").GetComponent<WinnerPopupPlayerStat>();
    }

    public void Initialize(Player body, DefensiveSkillBase def, Weapon wpn, ComplementarySkillBase skill1, ComplementarySkillBase skill2)
    {
		var winnerName = _winnerText.text.Replace("[PLAYERNAME]", "Player " + body.myID);

		_winnerText.text = winnerName;

        _body.SetStatInfo(body.name, body.GetComponent<UI_ImageAssets>().iconAsset);
        _def.SetStatInfo(def.GetSkillName(), _def.GetComponent<UI_ImageAssets>().iconAsset);
        _wpn.SetStatInfo(wpn.name, _wpn.GetComponent<UI_ImageAssets>().iconAsset);
        _skill1.SetStatInfo(skill1.GetSkillName(), _skill1.GetComponent<UI_ImageAssets>().iconAsset);
        _skill2.SetStatInfo(skill2.GetSkillName(), _skill2.GetComponent<UI_ImageAssets>().iconAsset);
    }
}

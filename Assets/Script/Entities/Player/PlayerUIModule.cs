using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerUIModule : MonoBehaviour
{
    public string canvasName = "Skills";
    Image[] _skillIcons = new Image[3];
    DefensiveSkillBase _defSk;
    ComplementarySkillBase[] _compSk = new ComplementarySkillBase[2];
    PhotonView pv;

    public enum SkillType { Defensive, Complementary1, Complementary2 }

    void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            var playerCount = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players").playerControllers.Length;
            int playerIndex = GameManager.Instance.Players.IndexOf(GetComponent<Player>());
            var canv = GameObject.Find(playerCount.ToString() + " Player").transform.Find(canvasName).transform.Find("Player" + (playerIndex + 1));
            _skillIcons[0] = canv.Find("Def").GetComponent<Image>();
            _skillIcons[1] = canv.Find("Comp1").GetComponent<Image>();
            _skillIcons[2] = canv.Find("Comp2").GetComponent<Image>();

            GameManager.Instance.OnResetRound += ResetRound;
            GameManager.Instance.OnChangeScene += ResetRound;
        }
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            var playerCount = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players").playerControllers.Length;
            int playerIndex = GameManager.Instance.Players.IndexOf(GetComponent<Player>());
            var canv = GameObject.Find(playerCount.ToString() + " Player").transform.Find(canvasName).transform.Find("Player" + (playerIndex + 1));
            _skillIcons[0] = canv.Find("Def").GetComponent<Image>();
            _skillIcons[1] = canv.Find("Comp1").GetComponent<Image>();
            _skillIcons[2] = canv.Find("Comp2").GetComponent<Image>();

            GameManager.Instance.OnResetRound += ResetRound;
            GameManager.Instance.OnChangeScene += ResetRound;
            
            pv = GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient) pv.RPC("RPCSetLeadingPlayer", RpcTarget.Others, playerCount, playerIndex, GameManager.Instance.GetScoreToWin());
        }
    }

    [PunRPC]
    void RPCStart(int playerCount, int playerIndex)
    {
        var canv = GameObject.Find(playerCount.ToString() + " Player").transform.Find(canvasName).transform.Find("Player" + (playerIndex + 1));
        _skillIcons[0] = canv.Find("Def").GetComponent<Image>();
        _skillIcons[1] = canv.Find("Comp1").GetComponent<Image>();
        _skillIcons[2] = canv.Find("Comp2").GetComponent<Image>();

        GameManager.Instance.OnResetRound += ResetRound;
        GameManager.Instance.OnChangeScene += ResetRound;
    }

    public void InitializeDefensiveSkill(DefensiveSkillBase sk)
    {
        _defSk = sk;
    }

    public void InitializeComplementarySkill(ComplementarySkillBase sk, SkillType skType)
    {
        var indx = (int)skType;
        indx--;
        _compSk[indx] = sk;
    }

    public void UpdateSkillState(SkillType skType)
    {
        SkillBase sk;
        if (skType == SkillType.Defensive) sk = _defSk;
        else if (skType == SkillType.Complementary1) sk = _compSk[0];
        else sk = _compSk[1];

        StartCoroutine(SkillUpdateRoutine(sk, skType));
    }

    IEnumerator SkillUpdateRoutine(SkillBase sk, SkillType skType)
    {
        var waitForFrameEnd = new WaitForEndOfFrame();
        while (sk.GetCooldownPerc() >= 0)
        {
            _skillIcons[(int)skType].fillAmount = sk.GetCooldownPerc();
            yield return waitForFrameEnd;
        }
        _skillIcons[(int)skType].fillAmount = 1;
    }

    void ResetRound()
    {
        StopAllCoroutines();
        foreach (var item in _skillIcons) item.fillAmount = 1;
    }

}

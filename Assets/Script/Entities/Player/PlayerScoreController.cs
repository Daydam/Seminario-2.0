using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerScoreController : MonoBehaviour
{
    public string canvasName = "ScoreContainer";

    readonly string _idleState = "Idle";
    readonly string _addState = "Add";
    readonly string _removeState = "Remove";
    readonly string _maxScoreReachedState = "MaxScoreReached";

    readonly string _bestPlayerShaderTag = "BestPlayerMaterial";

    public Text mainScore;
    Animator _an;
    Player _owner;
    PhotonView pv;

    PlayerSightingHandler _sighting;

    void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            _owner = GetComponent<Player>();
            var playerCount = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players").playerControllers.Length;
            int playerIndex = GameManager.Instance.Players.IndexOf(GetComponent<Player>());
            mainScore = GameObject.Find(playerCount.ToString() + " Player").transform.Find(canvasName).transform.Find("Player " + (playerIndex + 1)).GetComponentInChildren<Text>();
            _an = mainScore.GetComponent<Animator>();

            _sighting = GetComponent<PlayerSightingHandler>();

            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
            {
                pv = GetComponent<PhotonView>();
                pv.RPC("RPCStart", RpcTarget.Others, playerCount, playerIndex);
            }
        }
        else if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            _owner = GetComponent<Player>();
            var playerCount = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Online Registered Players").playerControllers.Length;
            int playerIndex = GameManager.Instance.Players.IndexOf(GetComponent<Player>());
            mainScore = GameObject.Find(playerCount.ToString() + " Player").transform.Find(canvasName).transform.Find("Player " + (playerIndex + 1)).GetComponentInChildren<Text>();
            _an = mainScore.GetComponent<Animator>();

            _sighting = GetComponent<PlayerSightingHandler>();

            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
            {
                pv = GetComponent<PhotonView>();
                pv.RPC("RPCStart", RpcTarget.Others, playerCount, playerIndex);
            }

            pv = GetComponent<PhotonView>();
        }
    }

    [PunRPC]
    void RPCStart(int playerCount, int playerIndex)
    {
        _owner = GetComponent<Player>();
        mainScore = GameObject.Find(playerCount.ToString() + " Player").transform.Find(canvasName).transform.Find("Player " + (playerIndex + 1)).GetComponentInChildren<Text>();
        _an = mainScore.GetComponent<Animator>();

        _sighting = GetComponent<PlayerSightingHandler>();
    }

    public void SetScore(int main, int toAdd)
    {
        if (!PhotonNetwork.InRoom || PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            mainScore.text = main.ToString();

            var toPlay = toAdd < 0 ? _removeState : toAdd > 0 ? _addState : _idleState;

            if (main == GameManager.Instance.GetScoreToWin()) toPlay = _maxScoreReachedState;
            _an.Play(toPlay);

            GameManager.Instance.ScoreUpdate();

        }
        if(PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient) pv.RPC("RPCSetScore", RpcTarget.Others, main, toAdd, GameManager.Instance.GetScoreToWin());
    }

    [PunRPC]
    void RPCSetScore(int main, int toAdd, int toWin)
    {
        mainScore.text = main.ToString();

        var toPlay = toAdd < 0 ? _removeState : toAdd > 0 ? _addState : _idleState;

        if (main == toWin) toPlay = _maxScoreReachedState;
        _an.Play(toPlay);
    }

    public void SetLeadingPlayer(bool activate)
    {
        if (!PhotonNetwork.InRoom || PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            _sighting.SetBestPlayer(activate);

            _owner.LightsModule.SetBestPlayer(activate);
        }
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            pv.RPC("RPCSetLeadingPlayer", RpcTarget.Others, activate, GameManager.Instance.GetScoreToWin());
        }
    }

    [PunRPC]
    void RPCSetLeadingPlayer(bool activate)
    {
        _sighting.SetBestPlayer(activate);

        _owner.LightsModule.SetBestPlayer(activate);
    }
}

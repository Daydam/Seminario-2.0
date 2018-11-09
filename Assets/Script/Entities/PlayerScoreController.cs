using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PlayerScoreController : MonoBehaviour
{
    public string canvasName = "ScoreContainer";

    readonly string _idleState = "Idle";
    readonly string _addState = "Add";
    readonly string _removeState = "Remove";

    readonly string _bestPlayerShaderTag = "BestPlayerMaterial";

    public Text mainScore;
    Animator _an;
    Renderer _rend;
    int _rendQ;

    PlayerSightingHandler _sighting;

    void Start()
    {
        _rend = GetComponentsInChildren<Renderer>().Where(x => x.material.GetTag(_bestPlayerShaderTag, true, "Nothing") == "true").First();
        _rendQ = _rend.material.renderQueue;

        var playerCount = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players").playerControllers.Length;
        int playerIndex = GameManager.Instance.Players.IndexOf(GetComponent<Player>());
        mainScore = GameObject.Find(playerCount.ToString() + " Player").transform.Find(canvasName).transform.Find("Player " + (playerIndex + 1)).GetComponentInChildren<Text>();
        _an = mainScore.GetComponent<Animator>();

        _sighting = GetComponent<PlayerSightingHandler>();
    }

    public void SetScore(int main, int toAdd)
    {
        mainScore.text = main.ToString();

        var toPlay = toAdd < 0 ? _removeState : toAdd > 0 ? _addState : _idleState;
        _an.Play(toPlay);

        GameManager.Instance.ScoreUpdate();
    }

    public void SetLeadingPlayer(bool activate)
    {
        if (!_rend) _rend = GetComponentsInChildren<Renderer>().Where(x => x.material.GetTag(_bestPlayerShaderTag, true, "Nothing") == "true").First();

        _sighting.SetBestPlayer(activate);

        var value = activate ? 1 : 0;
        _rend.material.SetFloat("_isBest", value);

        _rend.material.renderQueue = activate ? _rendQ + 1 : _rendQ;

    }
}

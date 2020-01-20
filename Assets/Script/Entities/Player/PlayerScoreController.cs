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
    readonly string _maxScoreReachedState = "MaxScoreReached";

    readonly string _bestPlayerShaderTag = "BestPlayerMaterial";

    public Text mainScore;
    Animator _an;
    Player _owner;

    PlayerSightingHandler _sighting;

    void Start()
    {
        _owner = GetComponent<Player>();
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

        if (main == GameManager.Instance.GetScoreToWin()) toPlay = _maxScoreReachedState;
        _an.Play(toPlay);

        GameManager.Instance.ScoreUpdate();
    }

    public void SetLeadingPlayer(bool activate)
    {
        _sighting.SetBestPlayer(activate);

        _owner.LightsModule.SetBestPlayer(activate);
    }
}

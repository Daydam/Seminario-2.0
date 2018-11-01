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

    void Start()
    {
        

        var playerCount = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players").playerControllers.Length;
        int playerIndex = GameManager.Instance.Players.IndexOf(GetComponent<Player>());
        mainScore = GameObject.Find(playerCount.ToString() + " Player").transform.Find(canvasName).transform.Find("Player " + (playerIndex + 1)).GetComponentInChildren<Text>();
        _an = mainScore.GetComponent<Animator>();
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
        if (!_rend) _rend = GetComponentsInChildren<Renderer>().Where(x => x.materials.Where(y => y.GetTag(_bestPlayerShaderTag, true, "Nothing") == "true").First()).First();

        var value = activate ? 1 : 0;
        _rend.materials.Where(x => x.GetTag(_bestPlayerShaderTag, true, "Nothing") == "true").First().SetFloat("_isBest", value);

    }
}

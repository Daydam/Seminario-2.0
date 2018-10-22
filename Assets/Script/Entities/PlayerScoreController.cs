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

    public Text mainScore;
    Animator _an;

    void Start()
    {
        var id = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players").playerControllers.Length;
        //var canvas = GameObject.Find(id.ToString() + " Player").transform.Find(canvasName).transform.Find(gameObject.tag);

        mainScore = GameObject.Find(id.ToString() + " Player").transform.Find(canvasName).transform.Find(gameObject.tag).GetComponentInChildren<Text>();
        _an = mainScore.GetComponent<Animator>();
    }

    public void SetScore(int main, int toAdd)
    {
        mainScore.text = main.ToString();

        var toPlay = toAdd < 0 ? _removeState : toAdd > 0 ? _addState : _idleState;

        _an.Play(toPlay);
    }
}

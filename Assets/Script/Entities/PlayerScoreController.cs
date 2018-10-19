using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PlayerScoreController : MonoBehaviour
{
    public string canvasName = "ScoreCanvas";
    public string mainScoreName = "MainScore";
    public string addedScoreName = "AddScore";

    public Text mainScore;
    public ScoreObject[] addedScore;
    public int maxIndexesToAdd = 4;

    int _actualAddIndex = 0;

    int ActualAddIndex
    {
        get { return _actualAddIndex; }

        set
        {
            _actualAddIndex = value >= maxIndexesToAdd ? 0 : value;
        }
    }

    void Start()
    {
        var canvas = GameObject.Find(canvasName).transform.Find(gameObject.tag);

        mainScore = canvas.Find(mainScoreName).GetComponentInChildren<Text>();

        addedScore = canvas.Find(addedScoreName).GetComponentsInChildren<ScoreObject>();

    }

    public void SetScore(int main, int toAdd)
    {
        mainScore.text = main.ToString();

        addedScore[_actualAddIndex].SetScore(toAdd);
        ActualAddIndex++;

    }
}

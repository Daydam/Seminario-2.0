using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ScoreObject : MonoBehaviour
{
    Animator _an;
    Text _tx;

    void Start()
    {
        _an = GetComponent<Animator>();
        _tx = GetComponent<Text>();
    }

    public void SetScore(int score)
    {
        var prefix = score >= 0 ? "+ " : "- ";

        _tx.text = prefix + Mathf.Abs(score);
        _an.SetTrigger("In");
    }
}

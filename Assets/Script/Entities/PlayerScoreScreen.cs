using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerScoreScreen : MonoBehaviour
{
    public enum ScoreType { MAIN, ADDED };
    public ScoreType texType;

    public RenderTexture scoreTexture;

    Renderer _rend;
    string mainPath = "Art/ScoreRenderTex/MainScoreRenderTexture_P";
    string addScorePath = "Art/ScoreRenderTex/AddScoreRenderTexture_P";

    void Start()
    {
        _rend = GetComponent<Renderer>();
        if (texType == ScoreType.MAIN)
        {
            scoreTexture = (RenderTexture)Resources.Load(mainPath + (GetComponentInParent<Player>().myID+1));
        }
        else if (texType == ScoreType.ADDED)
        {
            scoreTexture = (RenderTexture)Resources.Load(addScorePath + (GetComponentInParent<Player>().myID+1));
        }

        _rend.material.EnableKeyword("_Score");
        _rend.material.SetTexture("_Score", scoreTexture);
    }
}

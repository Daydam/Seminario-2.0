using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "new StageSelect File", menuName = "Scriptable Objects/Configuration/StageSelect File")]
public class SO_StageSelect : ScriptableObject
{
    public string description;
    public Sprite stageImage;
    public Scene stageScene;
}

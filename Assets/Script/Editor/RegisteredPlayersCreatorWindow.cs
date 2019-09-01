using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;

public class RegisteredPlayersCreatorWindow : EditorWindow
{
    bool showRegHelp;
    [Range(2, 4)]
    int amountOfPlayers = 2;
    RegisteredPlayers reg;
    List<int> regPlayers;
    int currentModeIndex;
    int[] modeIndexes;

    string[] allModes;

    int[] stageIndexes;
    string[] allStages;
    int currentStageIndex;

    GUIStyle titleStyle;

    [MenuItem("Assistants/Data Creators/Registered Players")]
    static void CreateWindow()
    {
        GetWindow(typeof(RegisteredPlayersCreatorWindow), false, "Game Configuration Creator").Show();
    }

    void OnGUI()
    {
        if (titleStyle == null) titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;

        if(reg == null)
        {
            //RegisteredPlayers
            reg = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players");
            if (reg == null || reg.fileRegVersion < RegisteredPlayers.latestRegVersion) reg = new RegisteredPlayers();
            else
            {
                regPlayers = reg.playerControllers.ToList();

                allModes = Resources.LoadAll<SO_GameRules>("Scriptable Objects").Select(a => a.name.Substring(9)).ToArray();
                int sceneCount = SceneManager.sceneCountInBuildSettings;
                allStages = new string[sceneCount];
                for (int i = 0; i < sceneCount; i++)
                {
                    allStages[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                }

                allStages = allStages.Where(a => a.Split('_')[0] == "Stage").Select(a => a.Split('_')[1]).ToArray();

                currentModeIndex = Mathf.Max(System.Array.IndexOf(allModes, reg.gameMode), 0);
                //currentStageIndex = Mathf.Max(System.Array.IndexOf(allStages, reg.stage.Split('_')[1]), 0);
            }
        }

        EditorGUILayout.LabelField("Game Configuration", titleStyle);
        EditorGUILayout.LabelField("Current Reg version: " + RegisteredPlayers.latestRegVersion);
        if (GUILayout.Button("Show Help")) showRegHelp = !showRegHelp;
        if (showRegHelp) EditorGUILayout.HelpBox("This is the Game Configuration Window. Here, you can edit the amount of players and their respective controllers, as well as the game mode. Please remember that, when setting up the controllers, you must substract 1 from the actual controller number. For example, write 0 for controller 1", MessageType.Info);
        if (regPlayers == null) regPlayers = new List<int> { 0, 0 };
        for (int i = 0; i < regPlayers.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            regPlayers[i] = EditorGUILayout.IntField("Player " + (i+1)  +  " Controller", regPlayers[i]);
            if (regPlayers.Count > 2)
            {
                if (GUILayout.Button("Remove")) regPlayers.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (regPlayers.Count < 4)
            if (GUILayout.Button("Add")) regPlayers.Add(0);

        if (allModes == null)
        {
            allModes = Resources.LoadAll<SO_GameRules>("Scriptable Objects").Select(a => a.name.Substring(9)).ToArray();
        }
        if (modeIndexes == null)
        {
            modeIndexes = new int[allModes.Length];
            for (int i = 0; i < modeIndexes.Length; i++)
            {
                modeIndexes[i] = i;
            }
        }
        
        currentModeIndex = EditorGUILayout.IntPopup("Mode:", currentModeIndex, allModes, modeIndexes);

        if(allStages == null)
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            allStages = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                allStages[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }
            
            allStages = allStages.Where(a => a.Split('_')[0] == "Stage").Select(a => a.Split('_')[1]).ToArray();
        }
        if (stageIndexes == null)
        {
            stageIndexes = new int[allStages.Length];
            for (int i = 0; i < stageIndexes.Length; i++)
            {
                stageIndexes[i] = i;
            }
        }

        currentStageIndex = EditorGUILayout.IntPopup("Stage:", currentStageIndex, allStages, stageIndexes);
        Debug.Log("stage: " + currentStageIndex);
        
        if (GUILayout.Button("Save Game Configuration"))
        {
            reg.playerControllers = new int[regPlayers.Count];
            for (int i = 0; i < regPlayers.Count; i++)
            {
                reg.playerControllers[i] = regPlayers[i];
            }
            reg.gameMode = allModes[currentModeIndex];
            //Fix
            //reg.stage = "Stage_" + allStages[currentStageIndex];

            Serializacion.SaveJsonToDisk(reg, "Registered Players");
        }
    }
}
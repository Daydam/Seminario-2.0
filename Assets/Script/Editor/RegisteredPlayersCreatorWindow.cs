using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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

        //RegisteredPlayers
        EditorGUILayout.LabelField("Game Configuration", titleStyle);
        if (GUILayout.Button("Show Help")) showRegHelp = !showRegHelp;
        if (showRegHelp) EditorGUILayout.HelpBox("This is the Game Configuration Window. Here, you can edit the amount of players and their respective controllers, as well as the game mode. Please remember that, when setting up the controllers, you must substract 1 from the actual controller number. For example, write 0 for controller 1", MessageType.Info);
        if (reg == null) reg = new RegisteredPlayers();
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
            reg.gameMode = allModes[0];
        }
        if (modeIndexes == null)
        {
            modeIndexes = new int[allModes.Length];
            for (int i = 0; i < modeIndexes.Length; i++)
            {
                modeIndexes[i] = i;
            }
        }
        
        currentModeIndex = EditorGUILayout.IntPopup(currentModeIndex, allModes, modeIndexes);
        reg.gameMode = allModes[currentModeIndex];

        if(GUILayout.Button("Save Game Configuration"))
        {
            reg.playerControllers = new int[regPlayers.Count];
            for (int i = 0; i < regPlayers.Count; i++)
            {
                reg.playerControllers[i] = regPlayers[i];
            }

            Serializacion.SaveJsonToDisk(reg, "Registered Players");
        }
    }
}

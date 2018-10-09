using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataCreatorWindow : EditorWindow
{
    [Range(1, 4)]
    int player = 1;
    CharacterURLs character;

    bool showRegHelp;
    [Range(2, 4)]
    int amountOfPlayers = 2;
    RegisteredPlayers reg;
    List<int> regPlayers;

    GUIStyle titleStyle;
    GUIStyle subtitleStyle;

    [MenuItem("Assistants/Data Creator")]
    static void CreateWindow()
    {
        GetWindow(typeof(DataCreatorWindow), false, "Data Creator").Show();
    }

    private void OnGUI()
    {
        if (titleStyle == null) titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;

        if (character == null) character = new CharacterURLs();
        EditorGUILayout.LabelField("Character URLs", titleStyle);
        EditorGUILayout.LabelField("This feature is under development. Please be patient, for my creator is pretending to be an artist right now. Sorry!");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Game Configuration", titleStyle);
        if (GUILayout.Button("Show Help")) showRegHelp = !showRegHelp;
        if (showRegHelp) EditorGUILayout.HelpBox("This is the Game Configuration section. Here, you can edit the amount of players and their respective controllers, as well as the game mode. Please remember that, when setting up the controllers, you must substract 1 from the actual controller number. For example, write 0 for controller 1", MessageType.Info);
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
        reg.gameMode = EditorGUILayout.TextField("Game Mode", reg.gameMode);
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

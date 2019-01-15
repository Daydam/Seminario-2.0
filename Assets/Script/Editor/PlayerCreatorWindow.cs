using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class PlayerCreatorWindow : EditorWindow
{
    [Range(1, 4)]
    int player = 1;
    CharacterURLs character;

    int currentBodyIndex;
    int[] bodyIndexes;
    string[] allBodies;

    int currentWeaponIndex;
    int[] weaponIndexes;
    string[] allWeapons;

    int currentDefIndex;
    int[] defIndexes;
    string[] allDefs;

    int currentComp1Index;
    int[] comp1Indexes;
    string[] allComp1s;

    int currentComp2Index;
    int[] comp2Indexes;
    string[] allComp2s;

    GUIStyle titleStyle;
    GUIStyle subtitleStyle;

    [MenuItem("Assistants/Data Creators/Player URLs")]
    static void CreateWindow()
    {
        GetWindow(typeof(PlayerCreatorWindow), false, "Player URLs Creator").Show();
    }

    void OnGUI()
    {
        if (titleStyle == null) titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;

        EditorGUILayout.LabelField("Player URLs", titleStyle);

        player = EditorGUILayout.IntSlider(player, 1, 4);

        if (allBodies == null)
        {
            allBodies = Resources.LoadAll<GameObject>("Prefabs/Bodies").Select(a => a.name).ToArray();
        }
        if (bodyIndexes == null)
        {
            bodyIndexes = new int[allBodies.Length];
            for (int i = 0; i < bodyIndexes.Length; i++)
            {
                bodyIndexes[i] = i;
            }
        }

        if (allWeapons == null)
        {
            allWeapons = Resources.LoadAll<GameObject>("Prefabs/Weapons").Select(a => a.name).ToArray();
        }
        if (weaponIndexes == null)
        {
            weaponIndexes = new int[allWeapons.Length];
            for (int i = 0; i < weaponIndexes.Length; i++)
            {
                weaponIndexes[i] = i;
            }
        }

        if (allDefs == null)
        {
            allDefs = Resources.LoadAll<GameObject>("Prefabs/Skills/Defensive").Select(a => a.name).ToArray();
        }
        if (defIndexes == null)
        {
            defIndexes = new int[allDefs.Length];
            for (int i = 0; i < defIndexes.Length; i++)
            {
                defIndexes[i] = i;
            }
        }

        if (allComp1s == null)
        {
            allComp1s = Resources.LoadAll<GameObject>("Prefabs/Skills/Complementary 1").Select(a => a.name).ToArray();
        }
        if (comp1Indexes == null)
        {
            comp1Indexes = new int[allComp1s.Length];
            for (int i = 0; i < comp1Indexes.Length; i++)
            {
                comp1Indexes[i] = i;
            }
        }

        if (allComp2s == null)
        {
            allComp2s = Resources.LoadAll<GameObject>("Prefabs/Skills/Complementary 2").Select(a => a.name).ToArray();
        }
        if (comp2Indexes == null)
        {
            comp2Indexes = new int[allComp2s.Length];
            for (int i = 0; i < comp2Indexes.Length; i++)
            {
                comp2Indexes[i] = i;
            }
        }

        if (character == null)
        {
            character = new CharacterURLs();
            character.complementaryURL = new string[2];
        }

        currentBodyIndex = EditorGUILayout.IntPopup(currentBodyIndex, allBodies, bodyIndexes);
        character.bodyURL = allBodies[currentBodyIndex];

        currentWeaponIndex = EditorGUILayout.IntPopup(currentWeaponIndex, allWeapons, weaponIndexes);
        character.weaponURL = allWeapons[currentWeaponIndex];
        
        currentDefIndex = EditorGUILayout.IntPopup(currentDefIndex, allDefs, defIndexes);
        character.defensiveURL = allDefs[currentDefIndex];
        
        currentComp1Index = EditorGUILayout.IntPopup(currentComp1Index, allComp1s, comp1Indexes);
        character.complementaryURL[0] = allComp1s[currentComp1Index];
        
        currentComp2Index = EditorGUILayout.IntPopup(currentComp2Index, allComp2s, comp2Indexes);
        character.complementaryURL[1] = allComp2s[currentComp2Index];

        if (GUILayout.Button("Save as Player " + player))
        {
            Serializacion.SaveJsonToDisk(character, "Player " + player);
        }
    }
}
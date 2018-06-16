using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using XInputDotNetPure;

public class CharacterSelectionManager : MonoBehaviour
{
    private static CharacterSelectionManager instance;
    public static CharacterSelectionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterSelectionManager>();
                if (instance == null)
                {
                    instance = new GameObject("new CharacterSelectionManager Object").AddComponent<CharacterSelectionManager>().GetComponent<CharacterSelectionManager>();
                }
            }
            return instance;
        }
    }

    public GameObject[] playerSpawnPoints;
    public GameObject[] blackScreens;
    public Text[] selectionTexts;

    Player[] players;
    Weapon[] currentWeapons;
    ComplementarySkillBase[,] currentComplementary;
    DefensiveSkillBase[] currentDefensive;
    CharacterURLs[] URLs;
    List<Weapon> weapons;
    int[] weaponIndexes;
    List<ComplementarySkillBase> complementarySkills;
    int[,] complementaryIndexes;
    List<DefensiveSkillBase> defensiveSkills;
    int[] defensiveIndexes;

    //A 4 index array that has values set for 0, 1, 2 or 3, 0 being weapon, 1 first complementary, 2 second comp, and 3 defensive.
    int[] selectedModifier;
    Vector2[] lastAnalogValue;

    //XInput
    PlayerIndex[] playerIndexes;
    GamePadState[] previousGamePads;
    GamePadState[] currentGamePads;



    void Start()
    {
        players = new Player[4];
        URLs = new CharacterURLs[4];

        currentWeapons = new Weapon[4];
        weaponIndexes = new int[4] { 0, 0, 0, 0 };

        currentComplementary = new ComplementarySkillBase[4, 2];
        complementaryIndexes = new int[4, 2] { { 0, 1 }, { 0, 1 }, { 0, 1 }, { 0, 1 } };

        currentDefensive = new DefensiveSkillBase[4];
        defensiveIndexes = new int[4] { 0, 0, 0, 0 };

        selectedModifier = new int[4] { 0, 0, 0, 0 };
        lastAnalogValue = new Vector2[4] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };

        weapons = new List<Weapon>();
        var filterList = Resources.LoadAll("Prefabs/Weapons").Select(x => (GameObject)x);
        weapons = filterList.Select(x => x.GetComponent<Weapon>()).ToList();

        complementarySkills = new List<ComplementarySkillBase>();
        var complementaryFilterList = Resources.LoadAll("Prefabs/Skills/Complementary").Select(x => (GameObject)x);
        complementarySkills = complementaryFilterList.Select(x => x.GetComponent<ComplementarySkillBase>()).ToList();

        defensiveSkills = new List<DefensiveSkillBase>();
        var defensiveFilterList = Resources.LoadAll("Prefabs/Skills/Defensive").Select(x => (GameObject)x);
        defensiveSkills = defensiveFilterList.Select(x => x.GetComponent<DefensiveSkillBase>()).ToList();


        playerIndexes = new PlayerIndex[4];
        previousGamePads = new GamePadState[4];
        currentGamePads = new GamePadState[4];
        for (int i = 0; i < currentGamePads.Length; i++)
        {
            currentGamePads[i] = GamePad.GetState((PlayerIndex)i);
        }
    }

    void Update()
    {
        for (int i = 0; i < currentGamePads.Length; i++)
        {
            previousGamePads[i] = currentGamePads[i];
            currentGamePads[i] = GamePad.GetState((PlayerIndex)i);
        }

        for (int i = 0; i < 4; i++)
        {
            if (players[i] == null) CheckStart(i);
            else
            {
                CheckSelect(i);
                if (selectedModifier[i] == 0) SelectWeapon(i);
                if (selectedModifier[i] == 1) SelectComplementary(i, 0);
                if (selectedModifier[i] == 2) SelectComplementary(i, 1);
                if (selectedModifier[i] == 3) SelectDefensive(i);
            }
            lastAnalogValue[i] = JoystickInput.LeftAnalog(currentGamePads[i]);


            if (JoystickInput.allKeys[JoystickKey.START](previousGamePads[i], currentGamePads[i]))
            {
                var regPlayers = players.Where(a => a != default(Player)).ToArray();

                if (regPlayers.Length >= 2)
                {
                    var reg = new RegisteredPlayers()
                    {
                        playerControllers = regPlayers.Select(a => System.Array.IndexOf(players, a)).ToArray()
                    };

                    reg.SaveDataToDisk("Assets/Resources/Save Files/Registered Players.dat" /*ass*/);

                    for (int j = 0; j < players.Length; j++)
                    {
                        if (players[j] != null)
                        {
                            URLs[j].SaveDataToDisk("Assets/Resources/Save Files/Player " + (j + 1) + ".dat");
                        }
                    }
                    SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                }
            }
        }
    }

    void CheckStart(int player)
    {
        if (JoystickInput.allKeys[JoystickKey.A](previousGamePads[player], currentGamePads[player]))
        {
            string path = "Assets/Resources/Save Files/Player " + player + 1 + ".dat";

            URLs[player] = Serializacion.LoadDataFromDisk<CharacterURLs>(path);
            if (URLs[player] == default(CharacterURLs))
            {
                var weaponNameChars = weapons[weaponIndexes[player]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string weaponName = new string(weaponNameChars);

                var compNameChars1 = complementarySkills[complementaryIndexes[player, 0]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string compName1 = new string(compNameChars1);

                var compNameChars2 = complementarySkills[complementaryIndexes[player, 1]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string compName2 = new string(compNameChars2);

                var defNameChars = defensiveSkills[defensiveIndexes[player]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string defName = new string(defNameChars);

                URLs[player] = new CharacterURLs
                {
                    bodyURL = "Prefabs/Bodies/Body " + (player + 1),
                    weaponURL = "Prefabs/Weapons/" + weaponName,
                    complementaryURL = new string[2] { "Prefabs/Skills/Complementary/" + compName1, "Prefabs/Skills/Complementary/" + compName2 },
                    defensiveURL = "Prefabs/Skills/Defensive/" + defName
                };
            }
            else
            {
                weaponIndexes[player] = weapons.IndexOf(Resources.Load<GameObject>(URLs[player].weaponURL).GetComponent<Weapon>());
                complementaryIndexes[player, 0] = complementarySkills.IndexOf(Resources.Load<GameObject>(URLs[player].complementaryURL[0]).GetComponent<ComplementarySkillBase>());
                complementaryIndexes[player, 1] = complementarySkills.IndexOf(Resources.Load<GameObject>(URLs[player].complementaryURL[1]).GetComponent<ComplementarySkillBase>());
                defensiveIndexes[player] = defensiveSkills.IndexOf(Resources.Load<GameObject>(URLs[player].defensiveURL).GetComponent<DefensiveSkillBase>());
            }

            players[player] = Instantiate(Resources.Load<GameObject>(URLs[player].bodyURL), playerSpawnPoints[player].transform.position, Quaternion.identity).GetComponent<Player>();
            currentWeapons[player] = Instantiate(Resources.Load<GameObject>(URLs[player].weaponURL), players[player].transform.position, Quaternion.identity, players[player].transform).GetComponent<Weapon>();
            currentComplementary[player, 0] = Instantiate(Resources.Load<GameObject>(URLs[player].complementaryURL[0]), players[player].transform.position, Quaternion.identity, players[player].transform).GetComponent<ComplementarySkillBase>();
            currentComplementary[player, 1] = Instantiate(Resources.Load<GameObject>(URLs[player].complementaryURL[1]), players[player].transform.position, Quaternion.identity, players[player].transform).GetComponent<ComplementarySkillBase>();
            currentDefensive[player] = Instantiate(Resources.Load<GameObject>(URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity, players[player].transform).GetComponent<DefensiveSkillBase>();

            blackScreens[player].gameObject.SetActive(false);
        }
    }

    void CheckSelect(int player)
    {
        if (-0.3f < lastAnalogValue[player].y && lastAnalogValue[player].y < 0.3f)
        {
            if (JoystickInput.LeftAnalog(currentGamePads[player]).y <= -0.3f)
            {
                selectedModifier[player] = selectedModifier[player] + 1 > 3 ? 0 : selectedModifier[player] + 1;
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).y >= 0.3f)
            {
                selectedModifier[player] = selectedModifier[player] - 1 < 0 ? 3 : selectedModifier[player] - 1;
            }
        }
    }

    void SelectWeapon(int player)
    {
        if (-0.3f < lastAnalogValue[player].x && lastAnalogValue[player].x < 0.3f)
        {
            if (JoystickInput.LeftAnalog(currentGamePads[player]).x >= 0.3f)
            {
                weaponIndexes[player]++;
                if (weaponIndexes[player] >= weapons.Count) weaponIndexes[player] = 0;

                var weaponName = weapons[weaponIndexes[player]].gameObject.name;
                URLs[player].weaponURL = "Prefabs/Weapons/" + weaponName;

                Destroy(currentWeapons[player].gameObject);
                currentWeapons[player] = Instantiate(Resources.Load<GameObject>(URLs[player].weaponURL), players[player].transform.position, Quaternion.identity, players[player].transform).GetComponent<Weapon>();
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).x <= -0.3f)
            {
                weaponIndexes[player]--;
                if (weaponIndexes[player] < 0) weaponIndexes[player] = weapons.Count - 1;

                var weaponName = weapons[weaponIndexes[player]].gameObject.name;
                URLs[player].weaponURL = "Prefabs/Weapons/" + weaponName;

                Destroy(currentWeapons[player].gameObject);
                currentWeapons[player] = Instantiate(Resources.Load<GameObject>(URLs[player].weaponURL), players[player].transform.position, Quaternion.identity, players[player].transform).GetComponent<Weapon>();
            }
        }

        var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
        selectionTexts[player].text = "<   Weapon: " + finalWeapon + "   >";
    }

    void SelectComplementary(int player, int compIndex)
    {
        if (-0.3f < lastAnalogValue[player].x && lastAnalogValue[player].x < 0.3f)
        {
            if (JoystickInput.LeftAnalog(currentGamePads[player]).x >= 0.3f)
            {
                do
                {
                    complementaryIndexes[player, compIndex]++;
                    if (complementaryIndexes[player, compIndex] >= complementarySkills.Count) complementaryIndexes[player, compIndex] = 0;
                } while (complementaryIndexes[player, 0] == complementaryIndexes[player, 1]);

                var complementaryName = complementarySkills[complementaryIndexes[player, compIndex]].gameObject.name;
                URLs[player].complementaryURL[compIndex] = "Prefabs/Skills/Complementary/" + complementaryName;

                Destroy(currentComplementary[player, compIndex].gameObject);
                currentComplementary[player, compIndex] = Instantiate(Resources.Load<GameObject>(URLs[player].complementaryURL[compIndex]), players[player].transform.position, Quaternion.identity, players[player].transform).GetComponent<ComplementarySkillBase>();
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).x <= -0.3f)
            {
                do
                {
                    complementaryIndexes[player, compIndex]--;
                    if (complementaryIndexes[player, compIndex] < 0) complementaryIndexes[player, compIndex] = complementarySkills.Count - 1;
                } while (complementaryIndexes[player, 0] == complementaryIndexes[player, 1]);

                var complementaryName = complementarySkills[complementaryIndexes[player, compIndex]].gameObject.name;
                URLs[player].complementaryURL[compIndex] = "Prefabs/Skills/Complementary/" + complementaryName;

                Destroy(currentComplementary[player, compIndex].gameObject);
                currentComplementary[player, compIndex] = Instantiate(Resources.Load<GameObject>(URLs[player].complementaryURL[compIndex]), players[player].transform.position, Quaternion.identity, players[player].transform).GetComponent<ComplementarySkillBase>();
            }
        }

        var finalComplementary = complementarySkills[complementaryIndexes[player, compIndex]].gameObject.name;
        selectionTexts[player].text = "<   Complementary " + (compIndex + 1) + ": " + finalComplementary + "   >";
    }

    void SelectDefensive(int player)
    {
        if (-0.3f < lastAnalogValue[player].x && lastAnalogValue[player].x < 0.3f)
        {
            if (JoystickInput.LeftAnalog(currentGamePads[player]).x >= 0.3f)
            {
                defensiveIndexes[player]++;
                if (defensiveIndexes[player] >= defensiveSkills.Count) defensiveIndexes[player] = 0;

                var defName = defensiveSkills[defensiveIndexes[player]].gameObject.name;
                URLs[player].defensiveURL = "Prefabs/Skills/Defensive/" + defName;

                Destroy(currentDefensive[player].gameObject);
                currentDefensive[player] = Instantiate(Resources.Load<GameObject>(URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity, players[player].transform).GetComponent<DefensiveSkillBase>();
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).x <= -0.3f)
            {
                defensiveIndexes[player]--;
                if (defensiveIndexes[player] < 0) defensiveIndexes[player] = defensiveSkills.Count - 1;

                var defName = defensiveSkills[defensiveIndexes[player]].gameObject.name;
                URLs[player].defensiveURL = "Prefabs/Skills/Defensive/" + defName;

                Destroy(currentDefensive[player].gameObject);
                currentDefensive[player] = Instantiate(Resources.Load<GameObject>(URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity, players[player].transform).GetComponent<DefensiveSkillBase>();
            }
        }

        var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
        selectionTexts[player].text = "<   Defensive: " + finalDefensive + "   >";
    }
}

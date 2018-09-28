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
    public Text[] weaponTexts;
    public Text[] defensiveTexts;
    public Text[] complementary1Texts;
    public Text[] complementary2Texts;


    public GameObject[] readyScreens;
    bool[] ready;
    GameObject[] players;
    GameObject[] currentWeapons;
    GameObject[,] currentComplementary;
    GameObject[] currentDefensive;
    CharacterURLs[] URLs;
    List<GameObject> weapons;
    int[] weaponIndexes;
    List<GameObject>[] complementarySkills;
    int[,] complementaryIndexes;
    List<GameObject> defensiveSkills;
    int[] defensiveIndexes;

    //A 4 index array that has values set for 0, 1, 2 or 3, 0 being weapon, 1 defensive, 2 first complementary, 3 second comp
    int[] selectedModifier;
    Vector2[] lastAnalogValue;

    //XInput
    PlayerIndex[] playerIndexes;
    GamePadState[] previousGamePads;
    GamePadState[] currentGamePads;

    void Start()
    {
        ready = new bool[4] { false, false, false, false };

        players = new GameObject[4];
        URLs = new CharacterURLs[4];

        currentWeapons = new GameObject[4];
        weaponIndexes = new int[4] { 0, 0, 0, 0 };

        currentComplementary = new GameObject[4, 2];
        complementaryIndexes = new int[4, 2] { { 0, 1 }, { 0, 1 }, { 0, 1 }, { 0, 1 } };

        currentDefensive = new GameObject[4];
        defensiveIndexes = new int[4] { 0, 0, 0, 0 };

        selectedModifier = new int[4] { 0, 0, 0, 0 };
        lastAnalogValue = new Vector2[4] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };

        weapons = new List<GameObject>();
        weapons = Resources.LoadAll("Prefabs/CharacterSelection/Weapons").Select(x => (GameObject)x).ToList();

        complementarySkills = new List<GameObject>[2];
        complementarySkills[0] = new List<GameObject>();
        complementarySkills[0] = Resources.LoadAll("Prefabs/CharacterSelection/Skills/Complementary 1").Select(x => (GameObject)x).ToList();
        complementarySkills[1] = new List<GameObject>();
        complementarySkills[1] = Resources.LoadAll("Prefabs/CharacterSelection/Skills/Complementary 2").Select(x => (GameObject)x).ToList();

        defensiveSkills = new List<GameObject>();
        defensiveSkills = Resources.LoadAll("Prefabs/CharacterSelection/Skills/Defensive").Select(x => (GameObject)x).ToList();


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
            else if(!ready[i])
            {
                CheckSelect(i);
                if (selectedModifier[i] == 0) SelectWeapon(i);
                if (selectedModifier[i] == 1) SelectDefensive(i);
                if (selectedModifier[i] == 2) SelectComplementary(i, 0);
                if (selectedModifier[i] == 3) SelectComplementary(i, 1);
            }
            lastAnalogValue[i] = JoystickInput.LeftAnalog(currentGamePads[i]);


            if (JoystickInput.allKeys[JoystickKey.START](previousGamePads[i], currentGamePads[i]))
            {
                ready[i] = !ready[i];
                readyScreens[i].gameObject.SetActive(ready[i]);

                if(ready[i])
                {
                    var regPlayers = players.Where(a => a != default(Player)).ToArray();
                    bool allReady = true;
                    for (int f = 0; f < regPlayers.Length; f++)
                    {
                        int playerIndex = System.Array.IndexOf(players, regPlayers[f]);
                        URLs[playerIndex].SaveJsonToDisk("Player " + (playerIndex + 1));
                        if (!ready[playerIndex]) allReady = false;
                    }

                    if (regPlayers.Length >= 2 && allReady)
                    {
                        var reg = new RegisteredPlayers();
                        reg.playerControllers = regPlayers.Select(a => System.Array.IndexOf(players, a)).ToArray();
                        Serializacion.SaveJsonToDisk(reg, "Registered Players");
                        StartCoroutine(StartGameCoroutine());
                    }
                }
            }
        }
    }

    IEnumerator StartGameCoroutine()
    {
        var asyncOp = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        asyncOp.allowSceneActivation = true;

        while (asyncOp.progress <= .99f)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    void CheckStart(int player)
    {
        if (JoystickInput.allKeys[JoystickKey.A](previousGamePads[player], currentGamePads[player]))
        {
            URLs[player] = Serializacion.LoadJsonFromDisk<CharacterURLs>("Player " + (player + 1));
            if (URLs[player] == default(CharacterURLs))
            {
                URLs[player] = new CharacterURLs();

                var weaponNameChars = weapons[weaponIndexes[player]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string weaponName = new string(weaponNameChars);

                var compNameChars1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string compName1 = new string(compNameChars1);

                var compNameChars2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string compName2 = new string(compNameChars2);

                var defNameChars = defensiveSkills[defensiveIndexes[player]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string defName = new string(defNameChars);
                URLs[player].bodyURL = "Beetledrone";
                URLs[player].weaponURL = weaponName;
                URLs[player].complementaryURL = new string[2] { compName1, compName2 };
                URLs[player].defensiveURL = defName;
            }

            weaponIndexes[player] = weapons.IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL));
            complementaryIndexes[player, 0] = complementarySkills[0].IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 1/" + URLs[player].complementaryURL[0]));
            complementaryIndexes[player, 1] = complementarySkills[1].IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 2/" + URLs[player].complementaryURL[1]));
            defensiveIndexes[player] = defensiveSkills.IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL));

            players[player] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[player].bodyURL), playerSpawnPoints[player].transform.position, Quaternion.identity);
            currentWeapons[player] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL), players[player].transform.position, Quaternion.identity);
            currentComplementary[player, 0] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 1/" + URLs[player].complementaryURL[0]), players[player].transform.position, Quaternion.identity);
            currentComplementary[player, 1] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 2/" + URLs[player].complementaryURL[1]), players[player].transform.position, Quaternion.identity);
            currentDefensive[player] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity);

            CharacterAssembler.Assemble(players[player], currentDefensive[player], currentComplementary[player, 0], currentComplementary[player, 1], currentWeapons[player]);

            var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
            weaponTexts[player].text = "<   Weapon: " + finalWeapon + "   >";
            var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
            defensiveTexts[player].text = "Defensive: " + finalDefensive;
            var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
            complementary1Texts[player].text = "Skill 1: " + finalComplementary1;
            var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
            complementary2Texts[player].text = "Skill 2: " + finalComplementary2;

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
                URLs[player].weaponURL = weaponName;

                Destroy(currentWeapons[player].gameObject);
                currentWeapons[player] = CharacterAssembler.ChangePart(currentWeapons[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL), players[player].transform.position, Quaternion.identity));
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).x <= -0.3f)
            {
                weaponIndexes[player]--;
                if (weaponIndexes[player] < 0) weaponIndexes[player] = weapons.Count - 1;

                var weaponName = weapons[weaponIndexes[player]].gameObject.name;
                URLs[player].weaponURL = weaponName;

                Destroy(currentWeapons[player].gameObject);
                currentWeapons[player] = CharacterAssembler.ChangePart(currentWeapons[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL), players[player].transform.position, Quaternion.identity));
            }
        }

        var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
        weaponTexts[player].text = "<   Weapon: " + finalWeapon + "   >";
        var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
        defensiveTexts[player].text = "Defensive: " + finalDefensive;
        var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
        complementary1Texts[player].text = "Skill 1: " + finalComplementary1;
        var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
        complementary2Texts[player].text = "Skill 2: " + finalComplementary2;

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
                    if (complementaryIndexes[player, compIndex] >= complementarySkills[compIndex].Count) complementaryIndexes[player, compIndex] = 0;
                } while (complementaryIndexes[player, 0] == complementaryIndexes[player, 1]);

                var complementaryName = complementarySkills[compIndex][complementaryIndexes[player, compIndex]].gameObject.name;
                URLs[player].complementaryURL[compIndex] = complementaryName;

                Destroy(currentComplementary[player, compIndex].gameObject);
                currentComplementary[player, compIndex] = CharacterAssembler.ChangePart(currentComplementary[player, compIndex], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary " + (compIndex + 1) + "/" + URLs[player].complementaryURL[compIndex]), players[player].transform.position, Quaternion.identity));
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).x <= -0.3f)
            {
                do
                {
                    complementaryIndexes[player, compIndex]--;
                    if (complementaryIndexes[player, compIndex] < 0) complementaryIndexes[player, compIndex] = complementarySkills[compIndex].Count - 1;
                } while (complementaryIndexes[player, 0] == complementaryIndexes[player, 1]);

                var complementaryName = complementarySkills[compIndex][complementaryIndexes[player, compIndex]].gameObject.name;
                URLs[player].complementaryURL[compIndex] = complementaryName;

                Destroy(currentComplementary[player, compIndex].gameObject);
                currentComplementary[player, compIndex] = CharacterAssembler.ChangePart(currentComplementary[player, compIndex], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary " + (compIndex + 1) + "/" + URLs[player].complementaryURL[compIndex]), players[player].transform.position, Quaternion.identity));
            }
        }

        var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
        weaponTexts[player].text = "Weapon: " + finalWeapon;
        var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
        defensiveTexts[player].text = "Defensive: " + finalDefensive;
        
        var finalComplementary = complementarySkills[compIndex][complementaryIndexes[player, compIndex]].gameObject.name;
        if (compIndex == 0)
        {
            complementary1Texts[player].text = "<   Skill " + (compIndex + 1) + ": " + finalComplementary + "   >";
            var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
            complementary2Texts[player].text = "Skill 2: " + finalComplementary2;
        }
        if (compIndex == 1)
        {
            var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
            complementary1Texts[player].text = "Skill 1: " + finalComplementary1;
            complementary2Texts[player].text = "<   Skill " + (compIndex + 1) + ": " + finalComplementary + "   >";
        }
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
                URLs[player].defensiveURL = defName;

                Destroy(currentDefensive[player].gameObject);
                currentDefensive[player] = CharacterAssembler.ChangePart(currentDefensive[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity));
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).x <= -0.3f)
            {
                defensiveIndexes[player]--;
                if (defensiveIndexes[player] < 0) defensiveIndexes[player] = defensiveSkills.Count - 1;

                var defName = defensiveSkills[defensiveIndexes[player]].gameObject.name;
                URLs[player].defensiveURL = defName;

                Destroy(currentDefensive[player].gameObject);
                currentDefensive[player] = CharacterAssembler.ChangePart(currentDefensive[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity));
            }
        }

        var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
        weaponTexts[player].text = "Weapon: " + finalWeapon;
        var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
        defensiveTexts[player].text = "<   Defensive: " + finalDefensive + "   >";
        var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
        complementary1Texts[player].text = "Skill 1: " + finalComplementary1;
        var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
        complementary2Texts[player].text = "Skill 2: " + finalComplementary2;
    }
}

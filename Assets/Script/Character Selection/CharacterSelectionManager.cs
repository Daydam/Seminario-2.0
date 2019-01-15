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
    public Text[] bodyTexts;
    public Text[] weaponTexts;
    public Text[] defensiveTexts;
    public Text[] complementary1Texts;
    public Text[] complementary2Texts;
    public Text startWhenReadyText;


    public GameObject[] readyScreens;
    bool[] ready;
    public bool[] Ready { get { return ready; } }
    GameObject[] players;
    GameObject[] currentBodies;
    GameObject[] currentWeapons;
    GameObject[,] currentComplementary;
    GameObject[] currentDefensive;
    CharacterURLs[] URLs;
    List<GameObject> bodies;
    int[] bodyIndexes;
    List<GameObject> weapons;
    int[] weaponIndexes;
    List<GameObject>[] complementarySkills;
    int[,] complementaryIndexes;
    List<GameObject> defensiveSkills;
    int[] defensiveIndexes;

    //A 4 index array that has values set for 0, 1, 2, 3 or 4, 0 being body, 1 weapon, 2 defensive, 3 first complementary, 4 second comp
    int[] selectedModifier;
    public int[] SelectedModifier { get { return selectedModifier; } }
    Vector2[] lastAnalogValue;

    //XInput
    PlayerIndex[] playerIndexes;
    GamePadState[] previousGamePads;
    GamePadState[] currentGamePads;

    public Color[] playerColors;

    void Start()
    {
        ready = new bool[4] { false, false, false, false };

        players = new GameObject[4];
        URLs = new CharacterURLs[4];

        currentBodies = new GameObject[4];
        bodyIndexes = new int[4] { 0, 0, 0, 0 };

        currentWeapons = new GameObject[4];
        weaponIndexes = new int[4] { 0, 0, 0, 0 };

        currentComplementary = new GameObject[4, 2];
        complementaryIndexes = new int[4, 2] { { 0, 1 }, { 0, 1 }, { 0, 1 }, { 0, 1 } };

        currentDefensive = new GameObject[4];
        defensiveIndexes = new int[4] { 0, 0, 0, 0 };

        selectedModifier = new int[4] { 0, 0, 0, 0 };
        lastAnalogValue = new Vector2[4] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero };

        bodies = new List<GameObject>();
        bodies = Resources.LoadAll("Prefabs/CharacterSelection/Bodies").Select(x => (GameObject)x).ToList();

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
            else
            {
                if (!ready[i])
                {
                    CheckSelect(i);
                    if (selectedModifier[i] == 0) SelectBody(i);
                    if (selectedModifier[i] == 1) SelectWeapon(i);
                    if (selectedModifier[i] == 2) SelectDefensive(i);
                    if (selectedModifier[i] == 3) SelectComplementary(i, 0);
                    if (selectedModifier[i] == 4) SelectComplementary(i, 1);
                }
                lastAnalogValue[i] = JoystickInput.LeftAnalog(currentGamePads[i]);


                if (JoystickInput.allKeys[JoystickKey.START](previousGamePads[i], currentGamePads[i]))
                {
                    ready[i] = !ready[i];
                    //readyScreens[i].gameObject.SetActive(ready[i]);
                    readyScreens[i].GetComponentInChildren<Text>().text = ready[i] ? "Player " + (i + 1) + " Ready" : "Player " + (i + 1);
                    if (ready[i])
                    {
                        //Set the text!
                        var finalBody = bodies[bodyIndexes[i]].gameObject.name;
                        bodyTexts[i].text = "Body: " + finalBody;
                        var finalWeapon = weapons[weaponIndexes[i]].gameObject.name;
                        weaponTexts[i].text = "Weapon: " + finalWeapon;
                        var finalDefensive = defensiveSkills[defensiveIndexes[i]].gameObject.name;
                        defensiveTexts[i].text = "Defensive: " + finalDefensive;
                        var finalComplementary1 = complementarySkills[0][complementaryIndexes[i, 0]].gameObject.name;
                        complementary1Texts[i].text = "Skill 1: " + finalComplementary1;
                        var finalComplementary2 = complementarySkills[1][complementaryIndexes[i, 1]].gameObject.name;
                        complementary2Texts[i].text = "Skill 2: " + finalComplementary2;

                        //Check if they're all ready
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
                else if (JoystickInput.allKeys[JoystickKey.B](previousGamePads[i], currentGamePads[i])
                || JoystickInput.allKeys[JoystickKey.BACK](previousGamePads[i], currentGamePads[i]))
                {
                    ready[i] = false;
                    //readyScreens[i].gameObject.SetActive(false);
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
            if (!startWhenReadyText.gameObject.activeSelf) startWhenReadyText.gameObject.SetActive(true);
            URLs[player] = Serializacion.LoadJsonFromDisk<CharacterURLs>("Player " + (player + 1));
            if (URLs[player] == default(CharacterURLs))
            {
                URLs[player] = new CharacterURLs();

                var bodyNameChars = bodies[bodyIndexes[player]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string bodyName = new string(bodyNameChars);

                var weaponNameChars = weapons[weaponIndexes[player]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string weaponName = new string(weaponNameChars);

                var compNameChars1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string compName1 = new string(compNameChars1);

                var compNameChars2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string compName2 = new string(compNameChars2);

                var defNameChars = defensiveSkills[defensiveIndexes[player]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string defName = new string(defNameChars);

                URLs[player].bodyURL = bodyName;
                URLs[player].weaponURL = weaponName;
                URLs[player].complementaryURL = new string[2] { compName1, compName2 };
                URLs[player].defensiveURL = defName;
            }

            bodyIndexes[player] = bodies.IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[player].bodyURL));
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
            players[player].GetComponentsInChildren<Renderer>().Where(x => x.material.GetTag("SkillStateColor", true, "Nothing") != "Nothing").First().material.SetColor("_PlayerColor", playerColors[player]);

            var finalBody = bodies[bodyIndexes[player]].gameObject.name;
            bodyTexts[player].text = "<   Body: " + finalBody + "   >";
            var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
            weaponTexts[player].text = "Weapon: " + finalWeapon;
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
            if (JoystickInput.LeftAnalog(currentGamePads[player]).y <= -0.3f
            || JoystickInput.allKeys[JoystickKey.DPAD_DOWN](previousGamePads[player], currentGamePads[player]))
            {
                selectedModifier[player] = selectedModifier[player] + 1 > 4 ? 0 : selectedModifier[player] + 1;
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).y >= 0.3f
            || JoystickInput.allKeys[JoystickKey.DPAD_UP](previousGamePads[player], currentGamePads[player]))
            {
                selectedModifier[player] = selectedModifier[player] - 1 < 0 ? 4 : selectedModifier[player] - 1;
            }
        }

        if (JoystickInput.allKeys[JoystickKey.B](previousGamePads[player], currentGamePads[player])
            || JoystickInput.allKeys[JoystickKey.BACK](previousGamePads[player], currentGamePads[player]))
            CancelPlayer(player);
    }

    void SelectBody(int player)
    {
        if (-0.3f < lastAnalogValue[player].x && lastAnalogValue[player].x < 0.3f)
        {
            if (JoystickInput.LeftAnalog(currentGamePads[player]).x >= 0.3f
            || JoystickInput.allKeys[JoystickKey.DPAD_RIGHT](previousGamePads[player], currentGamePads[player]))
            {
                bodyIndexes[player]++;
                if (bodyIndexes[player] >= bodies.Count) bodyIndexes[player] = 0;

                var bodyName = bodies[bodyIndexes[player]].gameObject.name;
                URLs[player].bodyURL = bodyName;
                
                currentBodies[player] = CharacterAssembler.ChangeBody(
                    Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[player].bodyURL), players[player].transform.position, Quaternion.identity), 
                    currentBodies[player], 
                    currentDefensive[player], 
                    currentComplementary[player, 0], 
                    currentComplementary[player, 1], 
                    currentWeapons[player]);
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).x <= -0.3f
            || JoystickInput.allKeys[JoystickKey.DPAD_LEFT](previousGamePads[player], currentGamePads[player]))
            {
                bodyIndexes[player]--;
                if (bodyIndexes[player] < 0) bodyIndexes[player] = bodies.Count - 1;

                var bodyName = bodies[bodyIndexes[player]].gameObject.name;
                URLs[player].bodyURL = bodyName;

                currentBodies[player] = CharacterAssembler.ChangeBody(
                    Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[player].bodyURL), players[player].transform.position, Quaternion.identity),
                    currentBodies[player],
                    currentDefensive[player],
                    currentComplementary[player, 0],
                    currentComplementary[player, 1],
                    currentWeapons[player]);
            }
        }

        var finalBody = bodies[bodyIndexes[player]].gameObject.name;
        bodyTexts[player].text = "<   Body: " + finalBody + "   >";
        var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
        weaponTexts[player].text = "Weapon: " + finalWeapon;
        var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
        defensiveTexts[player].text = "Defensive: " + finalDefensive;
        var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
        complementary1Texts[player].text = "Skill 1: " + finalComplementary1;
        var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
        complementary2Texts[player].text = "Skill 2: " + finalComplementary2;

    }

    void SelectWeapon(int player)
    {
        if (-0.3f < lastAnalogValue[player].x && lastAnalogValue[player].x < 0.3f)
        {
            if (JoystickInput.LeftAnalog(currentGamePads[player]).x >= 0.3f
            || JoystickInput.allKeys[JoystickKey.DPAD_RIGHT](previousGamePads[player], currentGamePads[player]))
            {
                weaponIndexes[player]++;
                if (weaponIndexes[player] >= weapons.Count) weaponIndexes[player] = 0;

                var weaponName = weapons[weaponIndexes[player]].gameObject.name;
                URLs[player].weaponURL = weaponName;

                Destroy(currentWeapons[player].gameObject);
                currentWeapons[player] = CharacterAssembler.ChangePart(currentWeapons[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL), players[player].transform.position, Quaternion.identity));
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).x <= -0.3f
            || JoystickInput.allKeys[JoystickKey.DPAD_LEFT](previousGamePads[player], currentGamePads[player]))
            {
                weaponIndexes[player]--;
                if (weaponIndexes[player] < 0) weaponIndexes[player] = weapons.Count - 1;

                var weaponName = weapons[weaponIndexes[player]].gameObject.name;
                URLs[player].weaponURL = weaponName;

                Destroy(currentWeapons[player].gameObject);
                currentWeapons[player] = CharacterAssembler.ChangePart(currentWeapons[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL), players[player].transform.position, Quaternion.identity));
            }
        }

        var finalBody = bodies[bodyIndexes[player]].gameObject.name;
        bodyTexts[player].text = "Body: " + finalBody;
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
            if (JoystickInput.LeftAnalog(currentGamePads[player]).x >= 0.3f
            || JoystickInput.allKeys[JoystickKey.DPAD_RIGHT](previousGamePads[player], currentGamePads[player]))
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

            if (JoystickInput.LeftAnalog(currentGamePads[player]).x <= -0.3f
            || JoystickInput.allKeys[JoystickKey.DPAD_LEFT](previousGamePads[player], currentGamePads[player]))
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

        var finalBody = bodies[bodyIndexes[player]].gameObject.name;
        bodyTexts[player].text = "Body: " + finalBody;
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
            if (JoystickInput.LeftAnalog(currentGamePads[player]).x >= 0.3f
            || JoystickInput.allKeys[JoystickKey.DPAD_RIGHT](previousGamePads[player], currentGamePads[player]))
            {
                defensiveIndexes[player]++;
                if (defensiveIndexes[player] >= defensiveSkills.Count) defensiveIndexes[player] = 0;

                var defName = defensiveSkills[defensiveIndexes[player]].gameObject.name;
                URLs[player].defensiveURL = defName;

                Destroy(currentDefensive[player].gameObject);
                currentDefensive[player] = CharacterAssembler.ChangePart(currentDefensive[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity));
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).x <= -0.3f
            || JoystickInput.allKeys[JoystickKey.DPAD_LEFT](previousGamePads[player], currentGamePads[player]))
            {
                defensiveIndexes[player]--;
                if (defensiveIndexes[player] < 0) defensiveIndexes[player] = defensiveSkills.Count - 1;

                var defName = defensiveSkills[defensiveIndexes[player]].gameObject.name;
                URLs[player].defensiveURL = defName;

                Destroy(currentDefensive[player].gameObject);
                currentDefensive[player] = CharacterAssembler.ChangePart(currentDefensive[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity));
            }
        }

        var finalBody = bodies[bodyIndexes[player]].gameObject.name;
        bodyTexts[player].text = "Body: " + finalBody;
        var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
        weaponTexts[player].text = "Weapon: " + finalWeapon;
        var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
        defensiveTexts[player].text = "<   Defensive: " + finalDefensive + "   >";
        var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
        complementary1Texts[player].text = "Skill 1: " + finalComplementary1;
        var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
        complementary2Texts[player].text = "Skill 2: " + finalComplementary2;
    }

    void CancelPlayer(int player)
    {
        Destroy(players[player]);
        players[player] = null;
        if(players.Where(a => a != null).ToArray().Length <= 0) startWhenReadyText.gameObject.SetActive(false);
        blackScreens[player].gameObject.SetActive(true);
    }
}

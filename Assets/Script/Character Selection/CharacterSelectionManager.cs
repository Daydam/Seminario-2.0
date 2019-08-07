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
    #region Cambios Iván
    public ModuleTooltip[] bodyTexts;
    public ModuleTooltip[] weaponTexts;
    public ModuleTooltip[] defensiveTexts;
    public ModuleTooltip[] complementary1Texts;
    public ModuleTooltip[] complementary2Texts;
    public GameObject splashScreen;

    public bool InputAllowed { get { return !splashScreen.activeInHierarchy; } }

    #endregion
    public Text startWhenReadyText;


    public GameObject[] readyScreens;
    bool[] ready;
    public bool[] Ready { get { return ready; } }
    GameObject[] players;
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

    RegisteredPlayers playerInfo;

    void Start()
    {
        ready = new bool[4] { false, false, false, false };

        players = new GameObject[4];
        URLs = new CharacterURLs[4];

        #region Cambios Iván
        if (!Application.isEditor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        bodyTexts = new ModuleTooltip[4];
        weaponTexts = new ModuleTooltip[4];
        defensiveTexts = new ModuleTooltip[4];
        complementary1Texts = new ModuleTooltip[4];
        complementary2Texts = new ModuleTooltip[4];

        for (int i = 0; i < players.Length; i++)
        {
            bodyTexts[i] = new ModuleTooltip("Player " + (i + 1).ToString(), ModuleTooltip.ModuleType.Body);

            weaponTexts[i] = new ModuleTooltip("Player " + (i + 1).ToString(), ModuleTooltip.ModuleType.Weapon);

            defensiveTexts[i] = new ModuleTooltip("Player " + (i + 1).ToString(), ModuleTooltip.ModuleType.Defensive);

            complementary1Texts[i] = new ModuleTooltip("Player " + (i + 1).ToString(), ModuleTooltip.ModuleType.Comp1);

            complementary2Texts[i] = new ModuleTooltip("Player " + (i + 1).ToString(), ModuleTooltip.ModuleType.Comp2);
        }
        #endregion

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
        complementarySkills[0] = Resources.LoadAll("Prefabs/CharacterSelection/Skills/Complementary").Select(x => (GameObject)x).ToList();
        complementarySkills[1] = new List<GameObject>();
        complementarySkills[1] = Resources.LoadAll("Prefabs/CharacterSelection/Skills/Complementary").Select(x => (GameObject)x).ToList();

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
        if (InputAllowed)
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

                        //Cambio Iván
                        SetVisibleModuleInfo(i);
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
                            bodyTexts[i].SetModuleName(finalBody);
                            var finalWeapon = weapons[weaponIndexes[i]].gameObject.name;
                            weaponTexts[i].SetModuleName(finalWeapon);
                            var finalDefensive = defensiveSkills[defensiveIndexes[i]].gameObject.name;
                            defensiveTexts[i].SetModuleName(finalDefensive);
                            var finalComplementary1 = complementarySkills[0][complementaryIndexes[i, 0]].gameObject.name;
                            complementary1Texts[i].SetModuleName(finalComplementary1);
                            var finalComplementary2 = complementarySkills[1][complementaryIndexes[i, 1]].gameObject.name;
                            complementary2Texts[i].SetModuleName(finalComplementary2);

                            DeactivateModuleTooltips(i);

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
                                playerInfo = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players");
                                if (playerInfo == null || playerInfo.fileRegVersion < RegisteredPlayers.latestRegVersion)
                                {
                                    playerInfo = new RegisteredPlayers();
                                    playerInfo.playerControllers = regPlayers.Select(a => System.Array.IndexOf(players, a)).ToArray();
                                    Serializacion.SaveJsonToDisk(playerInfo, "Registered Players");
                                    StartCoroutine(StartGameCoroutine());
                                }
                                else
                                {
                                    playerInfo.playerControllers = regPlayers.Select(a => System.Array.IndexOf(players, a)).ToArray();
                                    Serializacion.SaveJsonToDisk(playerInfo, "Registered Players");
                                    StartCoroutine(StartGameCoroutine());
                                }
                            }
                        }
                    }
                    else if (JoystickInput.allKeys[JoystickKey.B](previousGamePads[i], currentGamePads[i])
                    || JoystickInput.allKeys[JoystickKey.BACK](previousGamePads[i], currentGamePads[i]))
                    {
                        ready[i] = false;
                        DeactivateModuleTooltips(i);
                        //readyScreens[i].gameObject.SetActive(false);
                    }

                    #region KEYBOARD IMPLEMENTATION
                    if (players[3] != null && i == 3)
                    {
                        if (Input.GetKeyDown(KeyCode.Return) && Input.GetKey(KeyCode.F))
                        {
                            if (Application.isEditor)
                            {
                                Cursor.visible = false;
                                Cursor.lockState = CursorLockMode.Locked;
                            }

                            ready[3] = !ready[3];
                            //readyScreens[3].gameObject.SetActive(ready[3]);
                            readyScreens[3].GetComponentInChildren<Text>().text = ready[3] ? "Player " + 4 + " Ready" : "Player " + 4;
                            if (ready[3])
                            {
                                //Set the text!
                                var finalBody = bodies[bodyIndexes[3]].gameObject.name;
                                bodyTexts[3].SetModuleName(finalBody);
                                var finalWeapon = weapons[weaponIndexes[3]].gameObject.name;
                                weaponTexts[3].SetModuleName(finalWeapon);
                                var finalDefensive = defensiveSkills[defensiveIndexes[3]].gameObject.name;
                                defensiveTexts[3].SetModuleName(finalDefensive);
                                var finalComplementary1 = complementarySkills[0][complementaryIndexes[3, 0]].gameObject.name;
                                complementary1Texts[3].SetModuleName(finalComplementary1);
                                var finalComplementary2 = complementarySkills[1][complementaryIndexes[3, 1]].gameObject.name;
                                complementary2Texts[3].SetModuleName(finalComplementary2);

                                DeactivateModuleTooltips(3);

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
                                    playerInfo = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players");
                                    if (playerInfo == null || playerInfo.fileRegVersion < RegisteredPlayers.latestRegVersion)
                                    {
                                        playerInfo = new RegisteredPlayers();
                                        playerInfo.playerControllers = regPlayers.Select(a => System.Array.IndexOf(players, a)).ToArray();
                                        Serializacion.SaveJsonToDisk(playerInfo, "Registered Players");
                                        StartCoroutine(StartGameCoroutine());
                                    }
                                    else
                                    {
                                        playerInfo.playerControllers = regPlayers.Select(a => System.Array.IndexOf(players, a)).ToArray();
                                        Serializacion.SaveJsonToDisk(playerInfo, "Registered Players");
                                        StartCoroutine(StartGameCoroutine());
                                    }

                                    Debug.Log("MARTÍN HARDCODEE EL NOMBRE DE LA ESCENA EN EL CAMBIO DE ESCENAS PORQUE NO ME GUSTA TENER EL NUMERITO");
                                    Debug.LogWarning("MARTÍN HARDCODEE EL NOMBRE DE LA ESCENA EN EL CAMBIO DE ESCENAS PORQUE NO ME GUSTA TENER EL NUMERITO");
                                }
                            }
                        }
                        if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.F))
                        {
                            ready[3] = false;
                            DeactivateModuleTooltips(3);

                            if (Application.isEditor)
                            {
                                Cursor.visible = true;
                                Cursor.lockState = CursorLockMode.None;
                            }

                            //readyScreens[3].gameObject.SetActive(false);
                        }
                    }
                    #endregion
                }
            }
        }
        else
        {
            if(Input.anyKeyDown)
            {
                splashScreen.SetActive(false);
            }
        }
    }

    #region Cambios Iván
    void SetVisibleModuleInfo(int playerIndex)
    {
        if (players[playerIndex] == null) return;

        var modifier = new ModuleTooltip[5]{ bodyTexts[playerIndex],
                                             weaponTexts[playerIndex],
                                             defensiveTexts[playerIndex],
                                             complementary1Texts[playerIndex],
                                             complementary2Texts[playerIndex] };

        Transform tranf = null;

        switch (selectedModifier[playerIndex])
        {
            case 0:
                tranf = players[playerIndex].transform;
                break;
            case 1:
                tranf = currentWeapons[playerIndex].transform;
                break;
            case 2:
                tranf = currentDefensive[playerIndex].transform;
                break;
            case 3:
                tranf = currentComplementary[playerIndex, 0].transform;
                break;
            case 4:
                tranf = currentComplementary[playerIndex, 1].transform;
                break;
        }

        for (int i = 0; i < modifier.Length; i++)
        {
            if (i == selectedModifier[playerIndex]) modifier[i].EnableViewing(tranf);
            else modifier[i].DisableViewing();
        }
    }

    void DeactivateModuleTooltips(int playerIndex)
    {
        var modifier = new ModuleTooltip[5]{ bodyTexts[playerIndex],
                                             weaponTexts[playerIndex],
                                             defensiveTexts[playerIndex],
                                             complementary1Texts[playerIndex],
                                             complementary2Texts[playerIndex] };

        for (int i = 0; i < modifier.Length; i++)
        {
            modifier[i].DisableViewing();
        }

    }
    #endregion

    IEnumerator StartGameCoroutine()
    {
        var asyncOp = SceneManager.LoadSceneAsync("Stage_CrystalPyramid"/*playerInfo.stage*/, LoadSceneMode.Single);
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
            complementaryIndexes[player, 0] = complementarySkills[0].IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[player].complementaryURL[0]));
            complementaryIndexes[player, 1] = complementarySkills[1].IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[player].complementaryURL[1]));
            defensiveIndexes[player] = defensiveSkills.IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL));

            players[player] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[player].bodyURL), playerSpawnPoints[player].transform.position, Quaternion.identity);
            currentWeapons[player] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL), players[player].transform.position, Quaternion.identity);
            currentComplementary[player, 0] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[player].complementaryURL[0]), players[player].transform.position, Quaternion.identity);
            currentComplementary[player, 1] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[player].complementaryURL[1]), players[player].transform.position, Quaternion.identity);
            currentDefensive[player] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity);

            CharacterAssembler.Assemble(players[player], currentDefensive[player], currentComplementary[player, 0], currentComplementary[player, 1], currentWeapons[player]);
            players[player].GetComponentsInChildren<Renderer>().Where(x => x.material.GetTag("SkillStateColor", true, "Nothing") != "Nothing").First().material.SetColor("_PlayerColor", playerColors[player]);

            var finalBody = bodies[bodyIndexes[player]].gameObject.name;
            bodyTexts[player].SetModuleName(finalBody);
            var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
            weaponTexts[player].SetModuleName(finalWeapon);
            var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
            defensiveTexts[player].SetModuleName(finalDefensive);
            var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
            complementary1Texts[player].SetModuleName(finalComplementary1);
            var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
            complementary2Texts[player].SetModuleName(finalComplementary2);

            blackScreens[player].gameObject.SetActive(false);
        }

        #region KEYBOARD IMPLEMENTATION
        if (player == 3 && Input.GetKeyDown(KeyCode.Return) && Input.GetKey(KeyCode.F))
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
            complementaryIndexes[player, 0] = complementarySkills[0].IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[player].complementaryURL[0]));
            complementaryIndexes[player, 1] = complementarySkills[1].IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[player].complementaryURL[1]));
            defensiveIndexes[player] = defensiveSkills.IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL));

            players[player] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[player].bodyURL), playerSpawnPoints[player].transform.position, Quaternion.identity);
            currentWeapons[player] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL), players[player].transform.position, Quaternion.identity);
            currentComplementary[player, 0] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[player].complementaryURL[0]), players[player].transform.position, Quaternion.identity);
            currentComplementary[player, 1] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[player].complementaryURL[1]), players[player].transform.position, Quaternion.identity);
            currentDefensive[player] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity);

            CharacterAssembler.Assemble(players[player], currentDefensive[player], currentComplementary[player, 0], currentComplementary[player, 1], currentWeapons[player]);
            players[player].GetComponentsInChildren<Renderer>().Where(x => x.material.GetTag("SkillStateColor", true, "Nothing") != "Nothing").First().material.SetColor("_PlayerColor", playerColors[player]);

            var finalBody = bodies[bodyIndexes[player]].gameObject.name;
            bodyTexts[player].SetModuleName(finalBody);
            var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
            weaponTexts[player].SetModuleName(finalWeapon);
            var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
            defensiveTexts[player].SetModuleName(finalDefensive);
            var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
            complementary1Texts[player].SetModuleName(finalComplementary1);
            var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
            complementary2Texts[player].SetModuleName(finalComplementary2);

            blackScreens[player].gameObject.SetActive(false);
        }
        #endregion
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

        #region KEYBOARD IMPLEMENTATION
        if (player == 3)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedModifier[player] = selectedModifier[player] + 1 > 4 ? 0 : selectedModifier[player] + 1;
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedModifier[player] = selectedModifier[player] - 1 < 0 ? 4 : selectedModifier[player] - 1;
            }

            if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.F))
                CancelPlayer(player);
        }
        #endregion
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

                players[player] = CharacterAssembler.ChangeBody(
                    Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[player].bodyURL), players[player].transform.position, Quaternion.identity),
                    players[player],
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

                players[player] = CharacterAssembler.ChangeBody(
                    Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[player].bodyURL), players[player].transform.position, Quaternion.identity),
                    players[player],
                    currentDefensive[player],
                    currentComplementary[player, 0],
                    currentComplementary[player, 1],
                    currentWeapons[player]);
            }
        }

        #region KEYBOARD IMPLEMENTATION
        if (player == 3)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                bodyIndexes[player]++;
                if (bodyIndexes[player] >= bodies.Count) bodyIndexes[player] = 0;

                var bodyName = bodies[bodyIndexes[player]].gameObject.name;
                URLs[player].bodyURL = bodyName;

                players[player] = CharacterAssembler.ChangeBody(
                    Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[player].bodyURL), players[player].transform.position, Quaternion.identity),
                    players[player],
                    currentDefensive[player],
                    currentComplementary[player, 0],
                    currentComplementary[player, 1],
                    currentWeapons[player]);
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                bodyIndexes[player]--;
                if (bodyIndexes[player] < 0) bodyIndexes[player] = bodies.Count - 1;

                var bodyName = bodies[bodyIndexes[player]].gameObject.name;
                URLs[player].bodyURL = bodyName;

                players[player] = CharacterAssembler.ChangeBody(
                    Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[player].bodyURL), players[player].transform.position, Quaternion.identity),
                    players[player],
                    currentDefensive[player],
                    currentComplementary[player, 0],
                    currentComplementary[player, 1],
                    currentWeapons[player]);
            }
        }
        #endregion

        var finalBody = bodies[bodyIndexes[player]].gameObject.name;
        bodyTexts[player].SetModuleName(finalBody);
        var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
        weaponTexts[player].SetModuleName(finalWeapon);
        var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
        defensiveTexts[player].SetModuleName(finalDefensive);
        var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
        complementary1Texts[player].SetModuleName(finalComplementary1);
        var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
        complementary2Texts[player].SetModuleName(finalComplementary2);

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

        #region KEYBOARD IMPLEMENTATION
        if (player == 3)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                weaponIndexes[player]++;
                if (weaponIndexes[player] >= weapons.Count) weaponIndexes[player] = 0;

                var weaponName = weapons[weaponIndexes[player]].gameObject.name;
                URLs[player].weaponURL = weaponName;

                Destroy(currentWeapons[player].gameObject);
                currentWeapons[player] = CharacterAssembler.ChangePart(currentWeapons[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL), players[player].transform.position, Quaternion.identity));
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                weaponIndexes[player]--;
                if (weaponIndexes[player] < 0) weaponIndexes[player] = weapons.Count - 1;

                var weaponName = weapons[weaponIndexes[player]].gameObject.name;
                URLs[player].weaponURL = weaponName;

                Destroy(currentWeapons[player].gameObject);
                currentWeapons[player] = CharacterAssembler.ChangePart(currentWeapons[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL), players[player].transform.position, Quaternion.identity));
            }
        }
        #endregion

        var finalBody = bodies[bodyIndexes[player]].gameObject.name;
        bodyTexts[player].SetModuleName(finalBody);
        var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
        weaponTexts[player].SetModuleName(finalWeapon);
        var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
        defensiveTexts[player].SetModuleName(finalDefensive);
        var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
        complementary1Texts[player].SetModuleName(finalComplementary1);
        var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
        complementary2Texts[player].SetModuleName(finalComplementary2);

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
                currentComplementary[player, compIndex] = CharacterAssembler.ChangePart(currentComplementary[player, compIndex], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary" + /*(compIndex + 1) +*/ "/" + URLs[player].complementaryURL[compIndex]), players[player].transform.position, Quaternion.identity));
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
                currentComplementary[player, compIndex] = CharacterAssembler.ChangePart(currentComplementary[player, compIndex], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary" + /*(compIndex + 1) +*/ "/" + URLs[player].complementaryURL[compIndex]), players[player].transform.position, Quaternion.identity));
            }
        }

        #region KEYBOARD IMPLEMENTATION
        if (player == 3)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                do
                {
                    complementaryIndexes[player, compIndex]++;
                    if (complementaryIndexes[player, compIndex] >= complementarySkills[compIndex].Count) complementaryIndexes[player, compIndex] = 0;
                } while (complementaryIndexes[player, 0] == complementaryIndexes[player, 1]);

                var complementaryName = complementarySkills[compIndex][complementaryIndexes[player, compIndex]].gameObject.name;
                URLs[player].complementaryURL[compIndex] = complementaryName;

                Destroy(currentComplementary[player, compIndex].gameObject);
                currentComplementary[player, compIndex] = CharacterAssembler.ChangePart(currentComplementary[player, compIndex], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary" + /*(compIndex + 1) +*/ "/" + URLs[player].complementaryURL[compIndex]), players[player].transform.position, Quaternion.identity));
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                do
                {
                    complementaryIndexes[player, compIndex]--;
                    if (complementaryIndexes[player, compIndex] < 0) complementaryIndexes[player, compIndex] = complementarySkills[compIndex].Count - 1;
                } while (complementaryIndexes[player, 0] == complementaryIndexes[player, 1]);

                var complementaryName = complementarySkills[compIndex][complementaryIndexes[player, compIndex]].gameObject.name;
                URLs[player].complementaryURL[compIndex] = complementaryName;

                Destroy(currentComplementary[player, compIndex].gameObject);
                currentComplementary[player, compIndex] = CharacterAssembler.ChangePart(currentComplementary[player, compIndex], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary" + /*(compIndex + 1) +*/ "/" + URLs[player].complementaryURL[compIndex]), players[player].transform.position, Quaternion.identity));
            }
        }
        #endregion

        var finalBody = bodies[bodyIndexes[player]].gameObject.name;
        bodyTexts[player].SetModuleName(finalBody);
        var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
        weaponTexts[player].SetModuleName(finalWeapon);
        var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
        defensiveTexts[player].SetModuleName(finalDefensive);

        var finalComplementary = complementarySkills[compIndex][complementaryIndexes[player, compIndex]].gameObject.name;
        if (compIndex == 0)
        {
            complementary1Texts[player].SetModuleName(finalComplementary);
            var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
            complementary2Texts[player].SetModuleName(finalComplementary2);
        }
        if (compIndex == 1)
        {
            var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
            complementary1Texts[player].SetModuleName(finalComplementary1);
            complementary2Texts[player].SetModuleName(finalComplementary);
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

        #region KEYBOARD IMPLEMENTATION
        if (player == 3)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                defensiveIndexes[player]++;
                if (defensiveIndexes[player] >= defensiveSkills.Count) defensiveIndexes[player] = 0;

                var defName = defensiveSkills[defensiveIndexes[player]].gameObject.name;
                URLs[player].defensiveURL = defName;

                Destroy(currentDefensive[player].gameObject);
                currentDefensive[player] = CharacterAssembler.ChangePart(currentDefensive[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity));
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                defensiveIndexes[player]--;
                if (defensiveIndexes[player] < 0) defensiveIndexes[player] = defensiveSkills.Count - 1;

                var defName = defensiveSkills[defensiveIndexes[player]].gameObject.name;
                URLs[player].defensiveURL = defName;

                Destroy(currentDefensive[player].gameObject);
                currentDefensive[player] = CharacterAssembler.ChangePart(currentDefensive[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity));
            }
        }
        #endregion

        var finalBody = bodies[bodyIndexes[player]].gameObject.name;
        bodyTexts[player].SetModuleName(finalBody);
        var finalWeapon = weapons[weaponIndexes[player]].gameObject.name;
        weaponTexts[player].SetModuleName(finalWeapon);
        var finalDefensive = defensiveSkills[defensiveIndexes[player]].gameObject.name;
        defensiveTexts[player].SetModuleName(finalDefensive);
        var finalComplementary1 = complementarySkills[0][complementaryIndexes[player, 0]].gameObject.name;
        complementary1Texts[player].SetModuleName(finalComplementary1);
        var finalComplementary2 = complementarySkills[1][complementaryIndexes[player, 1]].gameObject.name;
        complementary2Texts[player].SetModuleName(finalComplementary2);
    }

    void CancelPlayer(int player)
    {
        Destroy(players[player]);
        players[player] = null;
        if (players.Where(a => a != null).ToArray().Length <= 0) startWhenReadyText.gameObject.SetActive(false);
        blackScreens[player].gameObject.SetActive(true);
    }
}

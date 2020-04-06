using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using XInputDotNetPure;
using Photon.Pun;
using Photon.Realtime;

public class CharacterSelectionManager : MonoBehaviour, IPunObservable
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

    public Canvas canvas;
    public GameObject[] playerSpawnPoints;
    public GameObject[] blackScreens;

    public ModuleTooltip[] bodyTexts;
    public ModuleTooltip[] weaponTexts;
    public ModuleTooltip[] defensiveTexts;
    public ModuleTooltip[] complementary1Texts;
    public ModuleTooltip[] complementary2Texts;
    public GameObject splashScreen;

    public LoadingScreen loadingScreenPrefab;
    LoadingScreen _loadingScreen;

    public bool InputAllowed { get { return !splashScreen.activeInHierarchy; } }

    public Text startWhenReadyText;

    public GameObject[] readyScreens;
    bool[] ready;
    public bool[] Ready { get { return ready; } }
    GameObject[] players;
    public GameObject[] Players { get { return players; } set { players = value; } }
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

    Dictionary<string, string> descriptions;
    bool[] showingHelp;
    public Text[] descriptionText;
    public GameObject[] pressYForInfo;
    public float descriptionCamOffset = .035f;
    public float offsetSpeed = 2.5f;
    float[] offsetState;
    public Camera[] cameras;

    void Start()
    {
        _loadingScreen = GameObject.Instantiate(loadingScreenPrefab, new Vector3(8000, 8000, 8000), Quaternion.identity);
        _loadingScreen.gameObject.SetActive(false);

        ready = new bool[4] { false, false, false, false };

        players = new GameObject[4];
        URLs = new CharacterURLs[4];


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
        //We need to add online here, Tincho.

        //Description Work
        showingHelp = new bool[4];
        offsetState = new float[4] { 0,0,0,0 };
        descriptions = new Dictionary<string, string>()
        {
            { "Shotgun", "Short range, wide spread. Lacking precision? Get close and blast 'em all!" },
            { "Magnum", "Not too fast, but very powerful. Ready, aim, destroy!" },
            { "Assault Rifle", "Rapid-fire, long range. Unleash a maelstrom of bullets upon your enemies!" },
            { "Repulsive Battery", "A powerful blow that pushes away nearby enemies." },
            { "Scrambler Bot", "This cute little spider will pursue your enemies and disable their guns and skills!" },
            { "Plasma Wall", "Block the enemies with this wall, but be careful, it can be destroyed!" },
            { "Dash", "Short movement burst. Dodge, hide, approach, destroy!" },
            { "Blink", "Teleport a short distance away. Not even walls can stop you!" },
            { "Stun Missile", "Stuns enemies on impact. Useful both for defense and offense!" },
            { "Rocket Salvo", "Unleash a barrage of short-range missiles!" },
            { "Rocket Launcher", "Does this really need any description? Hasta la vista, baby!" },
            { "Implosive Charge", "Throw a black hole and attract nearby enemies!" },
            { "Hook", "Get over here!" },
            { "EMP Caltrop", "Leave a mine on the floor, and slow down anyone who comes near it!" },
            { "SpiderMech", "Afraid of spiders? No? What about a mechanic, fully loaded one?" },
            { "Fastdrone", "They see me rollin', they hatin'" },
            { "Cardrone", "No, it's not a car. I don't even know why we named it that way" },
            { "Beetledrone", "Because nothing beats the classics" },
            { "Quadro", "Four legged behemoth ready for combat" },
            { "Sphero", "WATCH out for my BATTERY ram" },
        };
    }

    void Update()
    {
        if (InputAllowed)
        {
            if (!PhotonNetwork.InRoom)
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

                            SetVisibleModuleInfo(i);

                            HelpCamPos(i);
                        }
                        lastAnalogValue[i] = JoystickInput.LeftAnalog(currentGamePads[i]);


                        if (JoystickInput.allKeys[JoystickKey.START](previousGamePads[i], currentGamePads[i]))
                        {
                            if (showingHelp[i])
                                ShowHelp(i);
                            ready[i] = !ready[i];
                            HelpOnStartPressed(i, ready[i]);
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
                int player = int.Parse(PhotonNetwork.NickName.Split(' ')[1]);
                previousGamePads[player] = currentGamePads[player];
                currentGamePads[player] = GamePad.GetState((PlayerIndex)player);

                if (players[player] == null) CheckStart(player);
                else
                {
                    if (!ready[player])
                    {
                        CheckSelect(player);
                        if (selectedModifier[player] == 0) SelectBody(player);
                        if (selectedModifier[player] == 1) SelectWeapon(player);
                        if (selectedModifier[player] == 2) SelectDefensive(player);
                        if (selectedModifier[player] == 3) SelectComplementary(player, 0);
                        if (selectedModifier[player] == 4) SelectComplementary(player, 1);

                        SetVisibleModuleInfo(player);

                        HelpCamPos(player);
                    }
                    lastAnalogValue[player] = JoystickInput.LeftAnalog(currentGamePads[player]);


                    if (JoystickInput.allKeys[JoystickKey.START](previousGamePads[player], currentGamePads[player]))
                    {
                        if (showingHelp[player])
                            ShowHelp(player);
                        ready[player] = !ready[player];

                        if (PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

                        HelpOnStartPressed(player, ready[player]);
                        //readyScreens[i].gameObject.SetActive(ready[i]);
                        readyScreens[player].GetComponentInChildren<Text>().text = ready[player] ? "Player " + (player + 1) + " Ready" : "Player " + (player + 1);
                    }

                    else if (JoystickInput.allKeys[JoystickKey.B](previousGamePads[player], currentGamePads[player])
                    || JoystickInput.allKeys[JoystickKey.BACK](previousGamePads[player], currentGamePads[player]))
                    {
                        ready[player] = false;

                        if (PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

                        DeactivateModuleTooltips(player);
                        //readyScreens[i].gameObject.SetActive(false);
                    }

                    #region KEYBOARD IMPLEMENTATION
                    if (Input.GetKeyDown(KeyCode.Return) && Input.GetKey(KeyCode.F))
                    {
                        if (Application.isEditor)
                        {
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked;
                        }

                        ready[player] = !ready[player];

                        if (PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

                        readyScreens[player].GetComponentInChildren<Text>().text = ready[player] ? "Player " + (player  + 1) + " Ready" : "Player " + (player + 1);
                    }
                    if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.F))
                    {
                        ready[player] = false;
                        DeactivateModuleTooltips(player);

                        if (PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);

                        if (Application.isEditor)
                        {
                            Cursor.visible = true;
                            Cursor.lockState = CursorLockMode.None;
                        }

                        //readyScreens[3].gameObject.SetActive(false);
                    }
                    #endregion

                    if (ready[player])
                    {
                        //Set the text!
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
                        URLs[player].SaveJsonToDisk("Online Player" + (player + 1));

                        DeactivateModuleTooltips(player);

                        //Check if they're all ready
                        if(PhotonNetwork.IsMasterClient)
                        {
                            var regPlayers = players.Where(a => a != default(Player)).ToArray();
                            bool allReady = true;
                            for (int f = 0; f < regPlayers.Length; f++)
                            {
                                int playerIndex = System.Array.IndexOf(players, regPlayers[f]);
                                URLs[playerIndex].SaveJsonToDisk("Online Player" + (playerIndex + 1));
                                if (!ready[playerIndex]) allReady = false;
                            }

                            if (regPlayers.Length >= 2 && allReady)
                            {
                                playerInfo = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Online Registered Players");
                                if (playerInfo == null || playerInfo.fileRegVersion < RegisteredPlayers.latestRegVersion)
                                {
                                    playerInfo = new RegisteredPlayers();
                                }
                                playerInfo.playerControllers = regPlayers.Select(a => System.Array.IndexOf(players, a)).ToArray();
                                Serializacion.SaveJsonToDisk(playerInfo, "Online Registered Players");
                                StartCoroutine(StartGameCoroutine());
                            }
                        }
                    }
                }
            }
        }
    }

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

    void CheckStart(int player)
    {
        if (JoystickInput.allKeys[JoystickKey.A](previousGamePads[player], currentGamePads[player]))
        {
            if (!startWhenReadyText.gameObject.activeSelf) startWhenReadyText.gameObject.SetActive(true);
            if(PhotonNetwork.InRoom) URLs[player] = Serializacion.LoadJsonFromDisk<CharacterURLs>("Online Player " + (player + 1));
            else URLs[player] = Serializacion.LoadJsonFromDisk<CharacterURLs>("Player " + (player + 1));
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

            if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        #region KEYBOARD IMPLEMENTATION
        if (((players[3] != null && player== 3 && !PhotonNetwork.InRoom) || (PhotonNetwork.InRoom && player == int.Parse(PhotonNetwork.NickName.Split(' ')[1])))&& Input.GetKeyDown(KeyCode.Return) && Input.GetKey(KeyCode.F))
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
            
            if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }

            if (JoystickInput.LeftAnalog(currentGamePads[player]).y >= 0.3f
            || JoystickInput.allKeys[JoystickKey.DPAD_UP](previousGamePads[player], currentGamePads[player]))
            {
                selectedModifier[player] = selectedModifier[player] - 1 < 0 ? 4 : selectedModifier[player] - 1;

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }

        if (JoystickInput.allKeys[JoystickKey.B](previousGamePads[player], currentGamePads[player])
            || JoystickInput.allKeys[JoystickKey.BACK](previousGamePads[player], currentGamePads[player]))
        {
            CancelPlayer(player);

            if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        #region KEYBOARD IMPLEMENTATION
        if ((players[3] != null && player== 3 && !PhotonNetwork.InRoom) || (PhotonNetwork.InRoom && player == int.Parse(PhotonNetwork.NickName.Split(' ')[1])))
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedModifier[player] = selectedModifier[player] + 1 > 4 ? 0 : selectedModifier[player] + 1;

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedModifier[player] = selectedModifier[player] - 1 < 0 ? 4 : selectedModifier[player] - 1;

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }

            if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.F))
            {
                CancelPlayer(player);

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }
        #endregion
    }

    void SelectBody(int player)
    {
        descriptionText[player].text = URLs[player].bodyURL + "\n\n" + descriptions[URLs[player].bodyURL];

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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }

        #region KEYBOARD IMPLEMENTATION
        if ((players[3] != null && player== 3 && !PhotonNetwork.InRoom) || (PhotonNetwork.InRoom && player == int.Parse(PhotonNetwork.NickName.Split(' ')[1])))
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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
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

        if (JoystickInput.allKeys[JoystickKey.Y](previousGamePads[player], currentGamePads[player])) ShowHelp(player);
    }

    void SelectWeapon(int player)
    {
        descriptionText[player].text = URLs[player].weaponURL + "\n\n" + descriptions[URLs[player].weaponURL];

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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }

        #region KEYBOARD IMPLEMENTATION
        if ((players[3] != null && player== 3 && !PhotonNetwork.InRoom) || (PhotonNetwork.InRoom && player == int.Parse(PhotonNetwork.NickName.Split(' ')[1])))
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                weaponIndexes[player]++;
                if (weaponIndexes[player] >= weapons.Count) weaponIndexes[player] = 0;

                var weaponName = weapons[weaponIndexes[player]].gameObject.name;
                URLs[player].weaponURL = weaponName;

                Destroy(currentWeapons[player].gameObject);
                currentWeapons[player] = CharacterAssembler.ChangePart(currentWeapons[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL), players[player].transform.position, Quaternion.identity));

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                weaponIndexes[player]--;
                if (weaponIndexes[player] < 0) weaponIndexes[player] = weapons.Count - 1;

                var weaponName = weapons[weaponIndexes[player]].gameObject.name;
                URLs[player].weaponURL = weaponName;

                Destroy(currentWeapons[player].gameObject);
                currentWeapons[player] = CharacterAssembler.ChangePart(currentWeapons[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[player].weaponURL), players[player].transform.position, Quaternion.identity));

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
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

        if (JoystickInput.allKeys[JoystickKey.Y](previousGamePads[player], currentGamePads[player])) ShowHelp(player);
    }

    void SelectComplementary(int player, int compIndex)
    {
        descriptionText[player].text = URLs[player].complementaryURL[compIndex] + "\n\n" + descriptions[URLs[player].complementaryURL[compIndex]];

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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }

        #region KEYBOARD IMPLEMENTATION
        if ((players[3] != null && player== 3 && !PhotonNetwork.InRoom) || (PhotonNetwork.InRoom && player == int.Parse(PhotonNetwork.NickName.Split(' ')[1])))
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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
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

        if (JoystickInput.allKeys[JoystickKey.Y](previousGamePads[player], currentGamePads[player])) ShowHelp(player);
    }

    void SelectDefensive(int player)
    {
        descriptionText[player].text = URLs[player].defensiveURL + "\n\n" + descriptions[URLs[player].defensiveURL];

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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
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

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }

        #region KEYBOARD IMPLEMENTATION
        if ((players[3] != null && player== 3 && !PhotonNetwork.InRoom) || (PhotonNetwork.InRoom && player == int.Parse(PhotonNetwork.NickName.Split(' ')[1])))
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                defensiveIndexes[player]++;
                if (defensiveIndexes[player] >= defensiveSkills.Count) defensiveIndexes[player] = 0;

                var defName = defensiveSkills[defensiveIndexes[player]].gameObject.name;
                URLs[player].defensiveURL = defName;

                Destroy(currentDefensive[player].gameObject);
                currentDefensive[player] = CharacterAssembler.ChangePart(currentDefensive[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity));

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                defensiveIndexes[player]--;
                if (defensiveIndexes[player] < 0) defensiveIndexes[player] = defensiveSkills.Count - 1;

                var defName = defensiveSkills[defensiveIndexes[player]].gameObject.name;
                URLs[player].defensiveURL = defName;

                Destroy(currentDefensive[player].gameObject);
                currentDefensive[player] = CharacterAssembler.ChangePart(currentDefensive[player], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[player].defensiveURL), players[player].transform.position, Quaternion.identity));

                if(PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
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

        if (JoystickInput.allKeys[JoystickKey.Y](previousGamePads[player], currentGamePads[player])) ShowHelp(player);
    }

    public void CancelPlayer(int player)
    {
        Destroy(players[player]);
        players[player] = null;
        if (players.Where(a => a != null).ToArray().Length <= 0) startWhenReadyText.gameObject.SetActive(false);
        blackScreens[player].gameObject.SetActive(true);
        if (showingHelp[player])
            ShowHelp(player);
    }

    void ShowHelp(int player)
    {
        if(showingHelp[player])
        {
            descriptionText[player].transform.parent.gameObject.SetActive(false);
            pressYForInfo[player].gameObject.SetActive(true);
        }
        else
        {
            descriptionText[player].transform.parent.gameObject.SetActive(true);
            pressYForInfo[player].gameObject.SetActive(false);
        }
        showingHelp[player] = !showingHelp[player];
    }

    void HelpOnStartPressed(int player, bool forceHelp)
    {
        pressYForInfo[player].gameObject.SetActive(!forceHelp);
    }

    void HelpCamPos(int player)
    {
        if(showingHelp[player])
        {
            if (offsetState[player] < 1f)
                offsetState[player] = Mathf.Min(1, offsetState[player] + Time.deltaTime * offsetSpeed);
        }
        else
        {
            if (offsetState[player] > 0f)
                offsetState[player] = Mathf.Max(0, offsetState[player] - Time.deltaTime * offsetSpeed);
        }

        cameras[player].transform.localPosition = new Vector3(Mathf.Lerp(0, descriptionCamOffset, offsetState[player]), cameras[player].transform.localPosition.y, cameras[player].transform.localPosition.z);
    }

    IEnumerator StartGameCoroutine()
    {
        if(!PhotonNetwork.InRoom)
        {
            var asyncOp = SceneManager.LoadSceneAsync("Stage selection", LoadSceneMode.Single);
            asyncOp.allowSceneActivation = true;

            while (!asyncOp.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            if(PhotonNetwork.IsMasterClient)
            {
                var asyncOp = SceneManager.LoadSceneAsync("Stage selection", LoadSceneMode.Single);
                asyncOp.allowSceneActivation = true;

                while (!asyncOp.isDone)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }

    #region ONLINE PLAY
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //We have to send the part string and execute the CharacterAssembler every time we receive new ones. We cooooould use an RPC (they're done on CharSelectRPCs already),
        //and make it so the function is called every time we modify something.

        if (stream.IsWriting)
        {
            stream.SendNext(players[int.Parse(PhotonNetwork.NickName.Split(' ')[1])] != null);
            if(players[int.Parse(PhotonNetwork.NickName.Split(' ')[1])] != null)
            {
                stream.SendNext(URLs[int.Parse(PhotonNetwork.NickName.Split(' ')[1])].bodyURL);
                stream.SendNext(URLs[int.Parse(PhotonNetwork.NickName.Split(' ')[1])].weaponURL);
                stream.SendNext(URLs[int.Parse(PhotonNetwork.NickName.Split(' ')[1])].defensiveURL);
                stream.SendNext(URLs[int.Parse(PhotonNetwork.NickName.Split(' ')[1])].complementaryURL[0]);
                stream.SendNext(URLs[int.Parse(PhotonNetwork.NickName.Split(' ')[1])].complementaryURL[1]);
                stream.SendNext(ready[int.Parse(PhotonNetwork.NickName.Split(' ')[1])]);
            }
            else
            {
                stream.SendNext("AA");
                stream.SendNext("AA");
                stream.SendNext("AA");
                stream.SendNext("AA");
                stream.SendNext("AA");
                stream.SendNext(false);
            }
        }
        else
        {
            int sender = int.Parse(info.Sender.NickName.Split(' ')[1]);
            bool isOnline = (bool)stream.ReceiveNext();
            string bodyURL = (string)stream.ReceiveNext();
            string weaponURL = (string)stream.ReceiveNext();
            string defensiveURL = (string)stream.ReceiveNext();
            string[] complementaryURL = new string[2] { (string)stream.ReceiveNext(), (string)stream.ReceiveNext() };
            bool ready = (bool)stream.ReceiveNext();

            if (isOnline)
            {
                if (players[sender] == null)
                {
                    URLs[sender] = Serializacion.LoadJsonFromDisk<CharacterURLs>("Player " + (sender + 1));
                    if (URLs[sender] == default(CharacterURLs))
                    {
                        URLs[sender] = new CharacterURLs();
                        URLs[sender].bodyURL = bodyURL;
                        URLs[sender].weaponURL = weaponURL;
                        URLs[sender].complementaryURL = new string[2] { complementaryURL[0], complementaryURL[1] };
                        URLs[sender].defensiveURL = defensiveURL;
                    }

                    bodyIndexes[sender] = bodies.IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[sender].bodyURL));
                    weaponIndexes[sender] = weapons.IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[sender].weaponURL));
                    complementaryIndexes[sender, 0] = complementarySkills[0].IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[sender].complementaryURL[0]));
                    complementaryIndexes[sender, 1] = complementarySkills[1].IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[sender].complementaryURL[1]));
                    defensiveIndexes[sender] = defensiveSkills.IndexOf(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[sender].defensiveURL));

                    players[sender] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[sender].bodyURL), playerSpawnPoints[sender].transform.position, Quaternion.identity);
                    currentWeapons[sender] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[sender].weaponURL), players[sender].transform.position, Quaternion.identity);
                    currentComplementary[sender, 0] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[sender].complementaryURL[0]), players[sender].transform.position, Quaternion.identity);
                    currentComplementary[sender, 1] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary/" + URLs[sender].complementaryURL[1]), players[sender].transform.position, Quaternion.identity);
                    currentDefensive[sender] = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[sender].defensiveURL), players[sender].transform.position, Quaternion.identity);

                    //We need this to run on start, but then use ChangeBody/ChangePart.
                    CharacterAssembler.Assemble(players[sender], currentDefensive[sender], currentComplementary[sender, 0], currentComplementary[sender, 1], currentWeapons[sender]);
                    players[sender].GetComponentsInChildren<Renderer>().Where(x => x.material.GetTag("SkillStateColor", true, "Nothing") != "Nothing").First().material.SetColor("_PlayerColor", playerColors[sender]);

                    var finalBody = bodies[bodyIndexes[sender]].gameObject.name;
                    bodyTexts[sender].SetModuleName(finalBody);
                    var finalWeapon = weapons[weaponIndexes[sender]].gameObject.name;
                    weaponTexts[sender].SetModuleName(finalWeapon);
                    var finalDefensive = defensiveSkills[defensiveIndexes[sender]].gameObject.name;
                    defensiveTexts[sender].SetModuleName(finalDefensive);
                    var finalComplementary1 = complementarySkills[0][complementaryIndexes[sender, 0]].gameObject.name;
                    complementary1Texts[sender].SetModuleName(finalComplementary1);
                    var finalComplementary2 = complementarySkills[1][complementaryIndexes[sender, 1]].gameObject.name;
                    complementary2Texts[sender].SetModuleName(finalComplementary2);

                    blackScreens[sender].gameObject.SetActive(false);
                }
                else
                {
                    if(URLs[sender].bodyURL != bodyURL)
                    {
                        URLs[sender].bodyURL = bodyURL;

                        players[sender] = CharacterAssembler.ChangeBody(
                            Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs[sender].bodyURL), players[sender].transform.position, Quaternion.identity),
                            players[sender],
                            currentDefensive[sender],
                            currentComplementary[sender, 0],
                            currentComplementary[sender, 1],
                            currentWeapons[sender]);
                    }
                    if(URLs[sender].defensiveURL != defensiveURL)
                    {
                        URLs[sender].defensiveURL = defensiveURL;

                        Destroy(currentDefensive[sender].gameObject);
                        currentDefensive[sender] = CharacterAssembler.ChangePart(currentDefensive[sender], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs[sender].defensiveURL), players[sender].transform.position, Quaternion.identity));
                    }
                    if(URLs[sender].weaponURL != weaponURL)
                    {
                        URLs[sender].weaponURL = weaponURL;

                        Destroy(currentWeapons[sender].gameObject);
                        currentWeapons[sender] = CharacterAssembler.ChangePart(currentWeapons[sender], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs[sender].weaponURL), players[sender].transform.position, Quaternion.identity));
                    }
                    if(URLs[sender].complementaryURL[0] != complementaryURL[0])
                    {
                        URLs[sender].complementaryURL[0] = complementaryURL[0];

                        Destroy(currentComplementary[sender, 0].gameObject);
                        currentComplementary[sender, 0] = CharacterAssembler.ChangePart(currentComplementary[sender, 0], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary" + /*(compIndex + 1) +*/ "/" + URLs[sender].complementaryURL[0]), players[sender].transform.position, Quaternion.identity));
                    }
                    if (URLs[sender].complementaryURL[1] != complementaryURL[1])
                    {
                        URLs[sender].complementaryURL[1] = complementaryURL[1];

                        Destroy(currentComplementary[sender, 1].gameObject);
                        currentComplementary[sender, 1] = CharacterAssembler.ChangePart(currentComplementary[sender, 1], Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary" + /*(compIndex + 1) +*/ "/" + URLs[sender].complementaryURL[1]), players[sender].transform.position, Quaternion.identity));
                    }
                }

                this.ready[sender] = ready;
            }
            else CancelPlayer(sender);
        }
    }

    public void StartPlayer(int player)
    {
        if (!startWhenReadyText.gameObject.activeSelf) startWhenReadyText.gameObject.SetActive(true);
        if (PhotonNetwork.InRoom) URLs[player] = Serializacion.LoadJsonFromDisk<CharacterURLs>("Online Player");
        else URLs[player] = Serializacion.LoadJsonFromDisk<CharacterURLs>("Player " + (player + 1));
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

        if (PhotonNetwork.InRoom) GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

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
    float[] lastAxisValue;

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
        lastAxisValue = new float[4] { 0, 0, 0, 0 };

        weapons = new List<Weapon>();
        var filterList = Resources.LoadAll("Prefabs/Weapons").Select(x => (GameObject)x);
        weapons = filterList.Select(x => x.GetComponent<Weapon>()).ToList();

        complementarySkills = new List<ComplementarySkillBase>();
        var complementaryFilterList = Resources.LoadAll("Prefabs/Skills/Complementary").Select(x => (GameObject)x);
        complementarySkills = complementaryFilterList.Select(x => x.GetComponent<ComplementarySkillBase>()).ToList();

        defensiveSkills = new List<DefensiveSkillBase>();
        var defensiveFilterList = Resources.LoadAll("Prefabs/Skills/Defensive").Select(x => (GameObject)x);
        defensiveSkills = defensiveFilterList.Select(x => x.GetComponent<DefensiveSkillBase>()).ToList();
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (players[i] == null) CheckStart(i + 1);
            else
            {
                CheckSelect(i + 1);
                if (selectedModifier[i] == 0) SelectWeapon(i + 1);
                if (selectedModifier[i] == 1) SelectComplementary(i + 1, 0);
                if (selectedModifier[i] == 2) SelectComplementary(i + 1, 1);
                if (selectedModifier[i] == 3) SelectDefensive(i + 1);
            }
        }

        if (Input.GetKeyDown("joystick button 7"))
        {
            var regPlayers = players.Where(a => a != default(Player)).ToArray();

            if (regPlayers.Length >= 2)
            {
                var reg = new RegisteredPlayers()
                {
                    playerControllers = regPlayers.Select(a => System.Array.IndexOf(players, a) + 1).ToArray()
                };

                reg.SaveDataToDisk("Assets/Resources/Save Files/Registered Players.dat" /*ass*/);

                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i] != null)
                    {
                        URLs[i].SaveDataToDisk("Assets/Resources/Save Files/Player " + (i + 1) + ".dat");
                    }
                }
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            }
        }
    }

    void CheckStart(int player)
    {
        if (Input.GetKeyDown("joystick " + player + " button 0"))
        {
            string path = "Assets/Resources/Save Files/Player " + player + ".dat";

            URLs[player - 1] = Serializacion.LoadDataFromDisk<CharacterURLs>(path);
            if (URLs[player - 1] == default(CharacterURLs))
            {
                var weaponNameChars = weapons[weaponIndexes[player - 1]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string weaponName = new string(weaponNameChars);

                var compNameChars1 = complementarySkills[complementaryIndexes[player - 1, 0]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string compName1 = new string(compNameChars1);

                var compNameChars2 = complementarySkills[complementaryIndexes[player - 1, 1]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string compName2 = new string(compNameChars2);

                var defNameChars = defensiveSkills[defensiveIndexes[player - 1]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string defName = new string(defNameChars);

                URLs[player - 1] = new CharacterURLs
                {
                    bodyURL = "Prefabs/Bodies/Body " + player,
                    weaponURL = "Prefabs/Weapons/" + weaponName,
                    complementaryURL = new string[2] { "Prefabs/Skills/Complementary/" + compName1, "Prefabs/Skills/Complementary/" + compName2 },
                    defensiveURL = "Prefabs/Skills/Defensive/" + defName
                };
            }
            else
            {
                weaponIndexes[player - 1] = weapons.IndexOf(Resources.Load<GameObject>(URLs[player - 1].weaponURL).GetComponent<Weapon>());
                complementaryIndexes[player - 1, 0] = complementarySkills.IndexOf(Resources.Load<GameObject>(URLs[player - 1].complementaryURL[0]).GetComponent<ComplementarySkillBase>());
                complementaryIndexes[player - 1, 1] = complementarySkills.IndexOf(Resources.Load<GameObject>(URLs[player - 1].complementaryURL[1]).GetComponent<ComplementarySkillBase>());
                defensiveIndexes[player - 1] = defensiveSkills.IndexOf(Resources.Load<GameObject>(URLs[player - 1].defensiveURL).GetComponent<DefensiveSkillBase>());
            }

            players[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].bodyURL), playerSpawnPoints[player - 1].transform.position, Quaternion.identity).GetComponent<Player>();
            currentWeapons[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].weaponURL), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<Weapon>();
            currentComplementary[player - 1, 0] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].complementaryURL[0]), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<ComplementarySkillBase>();
            currentComplementary[player - 1, 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].complementaryURL[1]), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<ComplementarySkillBase>();
            currentDefensive[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].defensiveURL), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<DefensiveSkillBase>();

            blackScreens[player - 1].gameObject.SetActive(false);
        }
    }

    void CheckSelect(int player)
    {
        if (-0.3f < lastAxisValue[player - 1] && lastAxisValue[player - 1] < 0.3f)
        {
            //Axis 2 is Y on the left stick. Fuck me if you want to detect the D-pad, because NO GOD PLEASE NO. NOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            if (Input.GetAxisRaw("Joystick " + player + " Axis 2") >= 0.3f)
            {
                selectedModifier[player - 1] = selectedModifier[player - 1] + 1 > 3 ? 0 : selectedModifier[player - 1] + 1;
            }

            if (Input.GetAxisRaw("Joystick " + player + " Axis 2") <= -0.3f)
            {
                selectedModifier[player - 1] = selectedModifier[player - 1] - 1 < 0 ? 3 : selectedModifier[player - 1] - 1;
            }
        }

        lastAxisValue[player - 1] = Input.GetAxisRaw("Joystick " + player + " Axis 2");
    }

    void SelectWeapon(int player)
    {
        if (Input.GetKeyDown("joystick " + player + " button 4"))
        {
            weaponIndexes[player - 1]--;
            if (weaponIndexes[player - 1] < 0) weaponIndexes[player - 1] = weapons.Count - 1;

            var weaponName = weapons[weaponIndexes[player - 1]].gameObject.name;
            URLs[player - 1].weaponURL = "Prefabs/Weapons/" + weaponName;

            Destroy(currentWeapons[player - 1].gameObject);
            currentWeapons[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].weaponURL), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<Weapon>();
        }

        if (Input.GetKeyDown("joystick " + player + " button 5"))
        {
            weaponIndexes[player - 1]++;
            if (weaponIndexes[player - 1] >= weapons.Count) weaponIndexes[player - 1] = 0;

            var weaponName = weapons[weaponIndexes[player - 1]].gameObject.name;
            URLs[player - 1].weaponURL = "Prefabs/Weapons/" + weaponName;

            Destroy(currentWeapons[player - 1].gameObject);
            currentWeapons[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].weaponURL), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<Weapon>();
        }

        var finalWeapon = weapons[weaponIndexes[player - 1]].gameObject.name;
        selectionTexts[player - 1].text = "<   Weapon: " + finalWeapon + "   >";
    }

    void SelectComplementary(int player, int compIndex)
    {
        //DUPLICATE THIS FUCKING SHIT IN ORDER TO MAKE 2 SKILLS POSSIBLE. MODIFY THE SERIALIZATION FILES, THEN THIS, THEN THE LOADING PART ON THE GAME MANAGER.
        if (Input.GetKeyDown("joystick " + player + " button 4"))
        {
            do
            {
                complementaryIndexes[player - 1, compIndex]--;
                if (complementaryIndexes[player - 1, compIndex] < 0) complementaryIndexes[player - 1, compIndex] = complementarySkills.Count - 1;
            } while (complementaryIndexes[player - 1, 0] == complementaryIndexes[player - 1, 1]);

            var complementaryName = complementarySkills[complementaryIndexes[player - 1, compIndex]].gameObject.name;
            URLs[player - 1].complementaryURL[compIndex] = "Prefabs/Skills/Complementary/" + complementaryName;

            Destroy(currentComplementary[player - 1, compIndex].gameObject);
            currentComplementary[player - 1, compIndex] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].complementaryURL[compIndex]), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<ComplementarySkillBase>();
        }

        if (Input.GetKeyDown("joystick " + player + " button 5"))
        {
            do
            {
                complementaryIndexes[player - 1, compIndex]++;
                if (complementaryIndexes[player - 1, compIndex] >= complementarySkills.Count) complementaryIndexes[player - 1, compIndex] = 0;
            } while (complementaryIndexes[player - 1, 0] == complementaryIndexes[player - 1, 1]);

            var complementaryName = complementarySkills[complementaryIndexes[player - 1, compIndex]].gameObject.name;
            URLs[player - 1].complementaryURL[compIndex] = "Prefabs/Skills/Complementary/" + complementaryName;

            Destroy(currentComplementary[player - 1, compIndex].gameObject);
            currentComplementary[player - 1, compIndex] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].complementaryURL[compIndex]), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<ComplementarySkillBase>();
        }

        var finalComplementary = complementarySkills[complementaryIndexes[player - 1, compIndex]].gameObject.name;
        selectionTexts[player - 1].text = "<   Complementary " + (compIndex +1) + ": " + finalComplementary + "   >";
    }

    void SelectDefensive(int player)
    {
        if (Input.GetKeyDown("joystick " + player + " button 4"))
        {
            defensiveIndexes[player - 1]--;
            if (defensiveIndexes[player - 1] < 0) defensiveIndexes[player - 1] = defensiveSkills.Count - 1;

            var defName = defensiveSkills[defensiveIndexes[player - 1]].gameObject.name;
            URLs[player - 1].defensiveURL = "Prefabs/Skills/Defensive/" + defName;

            Destroy(currentDefensive[player - 1].gameObject);
            currentDefensive[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].defensiveURL), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<DefensiveSkillBase>();
        }

        if (Input.GetKeyDown("joystick " + player + " button 5"))
        {
            defensiveIndexes[player - 1]++;
            if (defensiveIndexes[player - 1] >= defensiveSkills.Count) defensiveIndexes[player - 1] = 0;

            var defName = defensiveSkills[defensiveIndexes[player - 1]].gameObject.name;
            URLs[player - 1].defensiveURL = "Prefabs/Skills/Defensive/" + defName;

            Destroy(currentDefensive[player - 1].gameObject);
            currentDefensive[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].defensiveURL), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<DefensiveSkillBase>();
        }

        var finalDefensive = defensiveSkills[defensiveIndexes[player - 1]].gameObject.name;
        selectionTexts[player - 1].text = "<   Defensive: " + finalDefensive + "   >";
    }
}

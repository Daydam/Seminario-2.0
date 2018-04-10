using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

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

    Player[] players;
    Weapon[] currentWeapons;
    CharacterURLs[] URLs;
    List<Weapon> weapons;
    int[] weaponIndexes;

    void Start()
    {
        players = new Player[4];
        URLs = new CharacterURLs[4];
        currentWeapons = new Weapon[4];
        weaponIndexes = new int[4] { 0, 0, 0, 0 };

        weapons = new List<Weapon>();
        var filterList = Resources.LoadAll("Prefabs/Weapons").Select(x => (GameObject)x);
        weapons = filterList.Select(x => x.GetComponent<Weapon>()).ToList();
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (players[i] == null) CheckStart(i + 1);
            else SelectWeapon(i + 1);
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
                    if(players[i] != null)
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
                var weaponNameChars = weapons[weaponIndexes[player-1]].gameObject.name.TakeWhile(a => a != '(').ToArray();
                string weaponName = new string(weaponNameChars);

                URLs[player - 1] = new CharacterURLs
                {
                    bodyURL = "Prefabs/Bodies/Body " + player,
                    weaponURL = "Prefabs/Weapons/" + weaponName
                };
            }
            else
            {
                weaponIndexes[player - 1] = weapons.IndexOf(Resources.Load<GameObject>(URLs[player - 1].weaponURL).GetComponent<Weapon>());
            }

            players[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].bodyURL), playerSpawnPoints[player - 1].transform.position, Quaternion.identity).GetComponent<Player>();
            currentWeapons[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].weaponURL), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<Weapon>();

            blackScreens[player - 1].gameObject.SetActive(false);
        }
    }

    void SelectWeapon(int player)
    {
        if (Input.GetKeyDown("joystick " + player + " button 4"))
        {
            weaponIndexes[player - 1]--;
            if (weaponIndexes[player - 1] <= 0) weaponIndexes[player - 1] = weapons.Count-1;

            var weaponName = weapons[weaponIndexes[player - 1]].gameObject.name;

            Destroy(currentWeapons[player - 1].gameObject);
            currentWeapons[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].weaponURL), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<Weapon>();

            URLs[player - 1].weaponURL = "Prefabs/Weapons/" + weaponName;
        }

        if (Input.GetKeyDown("joystick " + player + " button 5"))
        {
            weaponIndexes[player - 1]++;
            if (weaponIndexes[player - 1] >= weapons.Count) weaponIndexes[player - 1] = 0;

            var weaponName = weapons[weaponIndexes[player - 1]].gameObject.name;

            Destroy(currentWeapons[player - 1].gameObject);
            currentWeapons[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].weaponURL), players[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<Weapon>();
            
            URLs[player - 1].weaponURL = "Prefabs/Weapons/" + weaponName;
        }
    }
}

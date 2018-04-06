using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    CharacterURLs[] URLs;

    void Update()
    {
        CheckStart(1);
        CheckStart(2);
        CheckStart(3);
        CheckStart(4);
        if (Input.GetKeyDown("joystick 1 button 7"))
        {
            for (int i = 0; i < players.Length; i++)
            {
                URLs[i].SaveDataToDisk("Resources/Save Files/Player " + (i + 1) + ".dat");
            }
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }
    }

    void CheckStart(int player)
    {
        if (Input.GetKeyDown("joystick " + player + " button 0"))
        {
            if (players == null) players = new Player[4];
            if (URLs == null) URLs = new CharacterURLs[4];

            string path = "Resources/Save Files/Player " + player + ".dat";
            URLs[player - 1] = Serializacion.LoadDataFromDisk<CharacterURLs>(path);
            if (URLs[player - 1] == default(CharacterURLs))
            {
                URLs[player - 1] = new CharacterURLs
                {
                    bodyURL = "Prefabs/Bodies/Body " + player,
                    weaponURL = "Prefabs/Weapons/Assault Rifle"
                };
            }

            players[player - 1] = Instantiate(Resources.Load<GameObject>(URLs[player - 1].bodyURL), playerSpawnPoints[player - 1].transform.position, Quaternion.identity).GetComponent<Player>();
            Instantiate(Resources.Load<GameObject>(URLs[player - 1].weaponURL), playerSpawnPoints[player - 1].transform.position, Quaternion.identity, players[player - 1].transform).GetComponent<Player>();

            blackScreens[player - 1].gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager: MonoBehaviour
{
	private static CharacterSelectionManager instance;
	public static CharacterSelectionManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<CharacterSelectionManager>();
				if(instance == null)
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
                
            }
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }
    }

    void CheckStart(int player)
    {
        if(Input.GetKeyDown("joystick " + player + " button 0"))
        {
            if (players == null) players = new Player[4];
            players[player - 1] = Instantiate(Resources.Load<GameObject>("Prefabs/Player " + player), playerSpawnPoints[player - 1].transform.position, Quaternion.identity).GetComponent<Player>();
            blackScreens[player - 1].gameObject.SetActive(false);
        }
    }
}

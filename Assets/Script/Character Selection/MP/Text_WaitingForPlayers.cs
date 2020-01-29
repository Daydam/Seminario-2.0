using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Text_WaitingForPlayers : MonoBehaviour
{
    public TextMeshProUGUI[] texts;

    void Update()
    {
        if (!PhotonNetwork.InRoom)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].text = "Connecting...";
            }
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                gameObject.SetActive(false);
                Debug.Log(PhotonNetwork.NickName);
            }
            else
            {
                for (int i = 0; i < texts.Length; i++)
                {
                    texts[i].text = "Waiting for players...";
                    Debug.Log(PhotonNetwork.NickName);
                }
            }
        }
    }
}

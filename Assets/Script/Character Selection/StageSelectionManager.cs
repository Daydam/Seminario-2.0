﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class StageSelectionManager : MonoBehaviour
{
    public GameObject templateSlot;
    public Text titleText;

    public Canvas canvas;
    public LoadingScreen loadingScreenPrefab;
    LoadingScreen _loadingScreen;

    bool _loading;

    Text[] slotTexts;
    RegisteredPlayers playerInfo;
    SO_StageSelect[] stages;
    int selectedIndex = 0;

    int selector;

    Vector2 lastAnalogValue;
    GamePadState currentGamePad;
    GamePadState previousGamePad;

    void Start()
    {
        // TO DO RE-ADD RINGS


        _loadingScreen = GameObject.Instantiate(loadingScreenPrefab, new Vector3(8000, 8000, 8000), Quaternion.identity);
        _loadingScreen.gameObject.SetActive(false);

        stages = Resources.LoadAll("Scriptable Objects/Stages").Select(x => (SO_StageSelect)x).ToArray();
        slotTexts = new Text[stages.Length];

        var stageSpacing = Camera.main.pixelWidth / stages.Length;

        for (int i = 0; i < slotTexts.Length; i++)
        {
            slotTexts[i] = Instantiate(templateSlot, templateSlot.transform.parent).GetComponent<Text>();
            //Pos = -half + index+1
            //slotTexts[i].rectTransform.position = new Vector3(Camera.main.pixelWidth / 2 + stageSpacing / 2 + stageSpacing * (Mathf.Ceil(-stages.Length / 2f) + i), slotTexts[i].rectTransform.position.y, slotTexts[i].rectTransform.position.z);
            slotTexts[i].gameObject.SetActive(true);
            slotTexts[i].text = "<b>" + stages[i].name + "</b> \n\n" + stages[i].description;
            slotTexts[i].GetComponentInChildren<Image>().sprite = stages[i].stageImage;
        }

        playerInfo = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players");
        selector = playerInfo.playerControllers[Random.Range(0, playerInfo.playerControllers.Length - 1)];
        titleText.text = "Player <b>" + (selector + 1) + "</b>, please select a stage";

        lastAnalogValue = Vector2.zero;
        currentGamePad = new GamePadState();
        currentGamePad = new GamePadState();
    }

    void Update()
    {
        previousGamePad = currentGamePad;
        currentGamePad = GamePad.GetState((PlayerIndex)selector);

        if (-0.3f < lastAnalogValue.x && lastAnalogValue.x < 0.3f)
        {
            if (/*JoystickInput.LeftAnalog(currentGamePad).x >= 0.3f
            ||*/ JoystickInput.allKeys[JoystickKey.DPAD_RIGHT](previousGamePad, currentGamePad))
            {
                selectedIndex = selectedIndex + 1 >= stages.Length ? 0 : selectedIndex + 1;
            }

            if (/*JoystickInput.LeftAnalog(currentGamePad).x <= -0.3f
            ||*/ JoystickInput.allKeys[JoystickKey.DPAD_LEFT](previousGamePad, currentGamePad))
            {
                selectedIndex = selectedIndex - 1 < 0 ? stages.Length - 1 : selectedIndex - 1;
            }
            if (JoystickInput.allKeys[JoystickKey.A](previousGamePad, currentGamePad))
            {
                playerInfo.stage = stages[selectedIndex].stageScene.handle;
                print(playerInfo.stage);
                Serializacion.SaveJsonToDisk(playerInfo, "Registered Players");
                if (!_loading) StartCoroutine(StartGameCoroutine());
            }
        }
        #region KEYBOARD IMPLEMENTATION
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedIndex = selectedIndex + 1 >= stages.Length ? 0 : selectedIndex + 1;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedIndex = selectedIndex - 1 < 0 ? stages.Length : selectedIndex - 1;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!_loading) StartCoroutine(StartGameCoroutine());
        }
        #endregion


        for (int i = 0; i < stages.Length; i++)
        {
            if (i != selectedIndex)
                slotTexts[i].GetComponentInChildren<Transform>().localScale = new Vector3(1f, 1f, 1f);
            else
                slotTexts[i].GetComponentInChildren<Transform>().localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
    }

    IEnumerator StartGameCoroutine()
    {
        _loadingScreen.gameObject.SetActive(true);
        canvas.gameObject.SetActive(false);

        _loading = true;
        var asyncOp = SceneManager.LoadSceneAsync(stages[selectedIndex].stageScene.handle, LoadSceneMode.Single);
        asyncOp.allowSceneActivation = false;

        yield return new WaitForSeconds(2f);

        while (!asyncOp.isDone)
        {
            if (asyncOp.progress >= 0.9f)
            {
                _loadingScreen.OnLoadEnd();
                if (Input.anyKey || AnyButtonHandler.AnyButtonPressed()) asyncOp.allowSceneActivation = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}

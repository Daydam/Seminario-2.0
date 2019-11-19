using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    Canvas _canvas;
    GameObject _bodyContainer, _loadingTextContainer, _pressKeyContainer;
    TextMeshProUGUI _bodyText, _tipText;
    LoadingScreenTips _tips;

    void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>(true);

        _bodyContainer = transform.Find("Bodies").gameObject;

        _loadingTextContainer = _canvas.transform.Find("Loading").gameObject;
        _loadingTextContainer.gameObject.SetActive(true);

        _pressKeyContainer = _canvas.transform.Find("PressAnyKey").gameObject;
        _pressKeyContainer.gameObject.SetActive(false);

        _tips = GetComponent<LoadingScreenTips>();

        var textMeshesPro = GetComponentsInChildren<TextMeshProUGUI>();

        _bodyText = textMeshesPro.Where(x => x.name == "BodyText").First();
        _tipText = textMeshesPro.Where(x => x.name == "TipText").First();

        var children = _bodyContainer.GetComponentsInChildren<LoadingScreenBody>(true).ToArray();
        foreach (var item in children)
        {
            item.gameObject.SetActive(false);
        }
    }

    void OnEnable ()
	{
        _loadingTextContainer.gameObject.SetActive(true);
        _pressKeyContainer.gameObject.SetActive(false);

        var children = _bodyContainer.GetComponentsInChildren<LoadingScreenBody>(true).ToArray();
        foreach (var item in children)
        {
            item.gameObject.SetActive(false);
        }
        var rng = Random.Range(0, children.Length);
        children[rng].gameObject.SetActive(true);
        _bodyText.text = children[rng].gameObject.name;

        var selectedTip = _tips.tips[Random.Range(0, _tips.tips.Length)];
        _tipText.text = selectedTip;
    }

    public void OnLoadEnd()
    {
        _loadingTextContainer.gameObject.SetActive(false);
        _pressKeyContainer.gameObject.SetActive(true);
    }
}

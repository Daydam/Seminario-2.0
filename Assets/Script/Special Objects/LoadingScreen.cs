using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LoadingScreen : MonoBehaviour
{
    GameObject _bodyContainer, _loadingTextContainer, _pressKeyContainer;

    void Awake()
    {
        _bodyContainer = transform.Find("Bodies").gameObject;
        _loadingTextContainer = GetComponentInChildren<Canvas>(true).transform.Find("Loading").gameObject;
        _loadingTextContainer.gameObject.SetActive(true);
        _pressKeyContainer = GetComponentInChildren<Canvas>(true).transform.Find("PressAnyKey").gameObject;
        _pressKeyContainer.gameObject.SetActive(false);
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
	}

    public void OnLoadEnd()
    {
        _loadingTextContainer.gameObject.SetActive(false);
        _pressKeyContainer.gameObject.SetActive(true);
    }
}

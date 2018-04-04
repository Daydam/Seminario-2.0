using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
	private static UIManager instance;
	public UIManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<UIManager>();
				if(instance == null)
				{
					instance = new GameObject("new UIManager Object").AddComponent<UIManager>().GetComponent<UIManager>();
				}
			}
			return instance;
		}
	}

    List<Text> scoreTexts;
    public List<Text> ScoreTextFields { get { return scoreTexts; } }

    void Start()
    {
        scoreTexts = transform.GetComponentsInChildren<Text>().ToList();
    }
}

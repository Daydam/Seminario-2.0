using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerLightsModuleHandler : MonoBehaviour
{
    float _lifeValue;
    Color _playerColor;

    Renderer[] _rends;
    [SerializeField] string _droneBodyName;

    public string DroneBodyName
    {
        get
        {
            return _droneBodyName;
        }
    }

    void Awake()
    {
        _rends = transform.Find(_droneBodyName).GetComponentsInChildren<Renderer>();
    }

    public float GetLifeValue()
    {
        return _lifeValue;
    }

    public Color GetPlayerColor()
    {
        return _playerColor;
    }

    public void SetLifeValue(float val)
    {
        foreach (var item in _rends)
        {
            item.material.SetFloat("_Life", val);
        }
        _lifeValue = val;
    }

    public void SetPlayerColor(Color val)
    {
        foreach (var item in _rends)
        {
            item.material.SetColor("_PlayerColor", val);
        }
        _playerColor = val;
    }

    public void SetBestPlayer(bool isBest)
    {
        foreach (var item in _rends)
        {
            item.material.SetFloat("_isBest", isBest ? 1 : 0);
        }
    }
}

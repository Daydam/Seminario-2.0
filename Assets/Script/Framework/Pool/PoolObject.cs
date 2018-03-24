using UnityEngine;
using System.Collections;

public class PoolObject<T>
{
    private bool _isActive;
    private T _obj;
    public delegate void PoolCallback(T obj);
    private PoolCallback _initializationCallback;
    private PoolCallback _finalizationCallback;

    public PoolObject(T obj, PoolCallback initialization, PoolCallback finalization)
    {        
        _obj = obj;
        _initializationCallback = initialization;
        _finalizationCallback = finalization;
        isActive = false;
    }

    public T GetObj
    {
        get
        {
            return _obj;
        }
    }

    public bool isActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _isActive = value;
            if (_isActive)
            {
                if (_initializationCallback != null)
                    _initializationCallback(_obj);
            }
            else
            {
                if (_finalizationCallback != null)
                    _finalizationCallback(_obj);
            }
        }
    }
}
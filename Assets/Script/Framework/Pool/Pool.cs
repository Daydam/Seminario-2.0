using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool<T>
{
    private List<PoolObject<T>> _poolList;
    public List<PoolObject<T>> PoolList
    {
        get { return _poolList; }
    }
    public delegate T CallbackFactory();

    private int _count;
    private bool _isDinamic = false;
    private PoolObject<T>.PoolCallback _init;
    private PoolObject<T>.PoolCallback _finalize;
    private CallbackFactory _factoryMethod;

    public Pool(int initialStock, CallbackFactory factoryMethod, PoolObject<T>.PoolCallback initialize, PoolObject<T>.PoolCallback finalize, bool isDinamic)
    {
        _poolList = new List<PoolObject<T>>();
        _factoryMethod = factoryMethod;
        _isDinamic = isDinamic;
        _count = initialStock;
        _init = initialize;
        _finalize = finalize;
        
        for (int i = 0; i < _count; i++)
        {
            _poolList.Add(new PoolObject<T>(_factoryMethod(), _init, _finalize));
        }
    }

    public PoolObject<T> GetPoolObject()
    {
        for (int i = 0; i < _count; i++)
        {
            if (!_poolList[i].isActive)
            {
                _poolList[i].isActive = true;
                return _poolList[i];
            }
        }
        if (_isDinamic)
        {
            PoolObject<T> po = new PoolObject<T>(_factoryMethod(), _init, _finalize);
            po.isActive = true;
            _poolList.Add(po);
            _count++;
            return po;
        }
        return null;
    }

    public T GetObjectFromPool()
    {
        for (int i = 0; i < _count; i++)
        {
            if (!_poolList[i].isActive)
            {
                _poolList[i].isActive = true;
                return _poolList[i].GetObj;
            }
        }

        if (_isDinamic)
        {
            PoolObject<T> po = new PoolObject<T>(_factoryMethod(), _init, _finalize);
            po.isActive = true;
            _poolList.Add(po);
            _count++;
            return po.GetObj;
        }
        return default(T);
    }

    public void DisablePoolObject(T obj)
    {
        foreach (PoolObject<T> poolObj in _poolList)
        {
            if (poolObj.GetObj.Equals(obj))
            {
                poolObj.isActive = false;
                return;
            }
        }
    }
}

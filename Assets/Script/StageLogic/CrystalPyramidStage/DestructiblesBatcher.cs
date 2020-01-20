using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Firepower.Events;
using System;

public class DestructiblesBatcher : MonoBehaviour
{
    GameObject[] _destructibleProps;
    List<GameObject> _baseObjs;

    void Start()
    {
        Batch();
        var ans = GetComponentsInChildren<Animator>(true).Select(x => x.gameObject).ToFList();
        var flist = FList.Create<GameObject>();
        foreach (var item in ans)
        {
            var l = item.GetComponentsInChildren<Transform>(true).Select(x => x.gameObject);
            flist += l;
        }
        flist += ans;
        _destructibleProps = flist.ToArray();
        
        //EventManager.Instance.AddEventListener(CrystalPyramidEvents.DestructibleWallDestroyEnd, OnDestroyAnimationEnd);
    }

    void Batch()
    {
        _baseObjs = GetComponentsInChildren<Transform>().Where(x => x.name == "BaseObj").Select(x => x.gameObject).ToList();
        StaticBatchingUtility.Combine(_baseObjs.ToArray(), transform.parent.gameObject);
    }

    void OnDestroyAnimationEnd(object[] paramsContainer)
    {
        var anim = (Animator)paramsContainer[0];
        if (_destructibleProps.Contains(anim.gameObject))
        {
            _baseObjs.Add(anim.gameObject);
            StaticBatchingUtility.Combine(_baseObjs.ToArray(), transform.parent.gameObject);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PropsBatcher : MonoBehaviour
{
    void Start()
    {
        Batch();
    }

    void Batch()
    {
        var objs = GetComponentsInChildren<Transform>(true).Select(x => x.gameObject);

        StaticBatchingUtility.Combine(objs.ToArray(), transform.parent.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RomboidParent : MonoBehaviour
{
    public bool falling = false;

    void OnCollisionEnter(Collision collision)
    {
        if (!falling)
        {
            return;
        }

        var obj = collision.collider.transform;
        if (obj.gameObject.LayerMatchesWith(LayerMask.NameToLayer("Player1"), LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Player3"), LayerMask.NameToLayer("Player4")))
        {
            if (obj.parent != this.transform) obj.SetParent(this.transform);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (!falling)
        {
            return;
        }

        var obj = collision.collider.transform;
        if (obj.gameObject.LayerMatchesWith(LayerMask.NameToLayer("Player1"), LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Player3"), LayerMask.NameToLayer("Player4")))
        {
            if (obj.parent != this.transform) obj.SetParent(this.transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        var obj = collision.collider.transform;
        if (obj.gameObject.LayerMatchesWith(LayerMask.NameToLayer("Player1"), LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Player3"), LayerMask.NameToLayer("Player4")))
        {
            if (obj.parent == this.transform) obj.SetParent(null);
        }
    }
}

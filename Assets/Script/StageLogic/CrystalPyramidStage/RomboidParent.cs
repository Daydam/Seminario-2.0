using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RomboidParent : MonoBehaviour
{
    /*void OnCollisionEnter(Collision collision)
    {
        var obj = collision.collider.transform;
        if (obj.gameObject.LayerMatchesWith("Player1", "Player2", "Player3", "Player4"))
        {
            obj.SetParent(this.transform);
        }
    }*/

    /*void OnCollisionExit(Collision collision)
    {
        var obj = collision.collider.transform;
        if (collision.collider.gameObject.LayerMatchesWith("Player1", "Player2", "Player3", "Player4"))
        {
            if (obj.parent == this.transform) obj.SetParent(null);           
        }
    }*/
}

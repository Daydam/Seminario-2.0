using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RingStructure : MonoBehaviour,IDamageable
{
    public IDamageable GetThisEntity()
    {
        return this;
    }

    public void ResetHP()
    {
        
    }

    public void TakeDamage(float damage)
    {
       
    }

    public void TakeDamage(float damage, string killerTag)
    {
        
    }
}

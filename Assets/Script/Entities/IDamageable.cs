using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    void ResetHP();

    void TakeDamage(float damage);
    void TakeDamage(float damage, string killerTag);

    IDamageable GetThisEntity();
}

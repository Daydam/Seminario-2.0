using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructibleBase : MonoBehaviour, IDamageable
{
    public abstract void ResetHP();

    public abstract void ResetHP(params object[] info);

    public abstract void TakeDamage(float damage);

    public abstract void TakeDamage(float damage, string killerTag);

    protected abstract void Death();

    protected abstract void SubstractLife(float damage);
}

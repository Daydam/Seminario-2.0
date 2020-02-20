using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructibleBase : MonoBehaviour, IDamageable
{
    public ObstacleHeight obstacleHeight;
    public abstract void ResetHP();

    public abstract void ResetHP(params object[] info);

    public abstract void TakeDamage(float damage);

    public abstract void TakeDamage(float damage, string killerTag);

    public abstract void TakeDamage(float damage, Vector3 hitPosition);

    public abstract void TakeDamage(float damage, string killerTag, Vector3 hitPosition);

    protected abstract void Death();

    protected abstract void SubstractLife(float damage);
}

public enum ObstacleHeight
{
    HIGH,
    MEDIUM,
    LOW,
    Count
}

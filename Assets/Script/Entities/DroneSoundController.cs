using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class DroneSoundController : MonoBehaviour
{
    [SerializeField] AudioSource _engineSource, _stateSource;
    public AudioClip engineSound;
    public AudioClip stunSound;
    public AudioClip disableSound;
    public AudioClip slowedSound;

    Player _owner;
    float _maxSpeed;

    void Start ()
	{
        _owner = GetComponentInParent<Player>();
        _maxSpeed = _owner.movementSpeed;
	}

    public void SetEnginePitch(float currentSpeed)
    {
        _engineSource.pitch = 1 + 0.3f * (currentSpeed / _maxSpeed);
    }

    [System.Obsolete("Aún no tiene los sonidos, cuando estén borrame el return")]
    public void PlayStunSound()
    {
        return;
        _stateSource.PlayOneShot(stunSound);
    }

    [System.Obsolete("Aún no tiene los sonidos, cuando estén borrame el return")]
    public void PlayDisableSound()
    {
        return;

        _stateSource.PlayOneShot(disableSound);

    }

    [System.Obsolete("Aún no tiene los sonidos, cuando estén borrame el return")]
    public void PlaySlowedSound()
    {
        return;

        _stateSource.PlayOneShot(slowedSound);

    }
}

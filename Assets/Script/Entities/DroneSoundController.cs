using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class DroneSoundController : MonoBehaviour
{
    [SerializeField] AudioSource _stateSource;
    public AudioClip stunSound;
    public AudioClip disarmSound, deathSound;

    Player _owner;

    void Start ()
	{
        _owner = GetComponentInParent<Player>();
	}

    public void PlayStunSound()
    {
        _stateSource.PlayOneShot(stunSound);
    }

    public void PlayDisarmSound()
    {
        _stateSource.PlayOneShot(disarmSound);
    }

    public void PlayDeathSound()
    {
        GameManager.Instance.AudioSource.PlayOneShot(deathSound);
    }
}

using Firepower.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrowdMaster : MonoBehaviour
{
    [SerializeField] AudioSource crowdLoop;
    [SerializeField] AudioSource crowdCheering;
    [SerializeField] AudioSource crowdBooing;
    [SerializeField] AudioSource crowdSurprise;

    [SerializeField] float fadeInSpeed = 10;

    [SerializeField] [Range(0f, 1f)] float cheerChance = 0.75f;
    [SerializeField] [Range(0f, 1f)] float booChance = 0.75f;
    [SerializeField] [Range(0f, 1f)] float surpriseChance = 0.75f;

    [SerializeField] bool booInCheer;
    [SerializeField] [Range(0f, 1f)] float booInCheerChance = 0.25f;
    [SerializeField] [Range(0f, 1f)] float booInCheerVolume = 0.66f;
    [SerializeField] bool surpriseInCheer;
    [SerializeField] [Range(0f, 1f)] float surpriseInCheerChance = 0.5f;
    [SerializeField] [Range(0f, 1f)] float surpriseInCheerVolume = 0.85f;
    [SerializeField] bool cheerInBoo;
    [SerializeField] [Range(0f, 1f)] float cheerInBooChance = 0.66f;
    [SerializeField] [Range(0f, 1f)] float cheerInBooVolume = 0.66f;
    [SerializeField] bool surpriseInBoo;
    [SerializeField] [Range(0f, 1f)] float surpriseInBooChance = 0.5f;
    [SerializeField] [Range(0f, 1f)] float surpriseInBooVolume = 0.9f;
    [SerializeField] bool cheerInSurprise;
    [SerializeField] [Range(0f, 1f)] float cheerInSurpriseChance = 0.25f;
    [SerializeField] [Range(0f, 1f)] float cheerInSurpriseVolume = 0.5f;
    [SerializeField] bool booInSurprise;
    [SerializeField] [Range(0f, 1f)] float booInSurpriseChance = 0.1f;
    [SerializeField] [Range(0f, 1f)] float booInSurpriseVolume = 0.25f;
    [SerializeField] float criticalDamageThreshold = 50;
    [SerializeField] float criticalHPThreshold = 10;

    [SerializeField] AudioClip[] cheerTracks;
    [SerializeField] AudioClip[] booTracks;
    [SerializeField] AudioClip[] surpriseTracks;

    int cheerIndex;
    int CheerIndex { set { cheerIndex = cheerIndex + value >= cheerTracks.Length ? 0 : cheerIndex + value; } get { return cheerIndex; } }
    int booIndex;
    int BooIndex { set { booIndex = booIndex + value >= booTracks.Length ? 0 : booIndex + value; } get { return booIndex; } }
    int surpriseIndex;
    int SurpriseIndex { set { surpriseIndex = surpriseIndex + value >= surpriseTracks.Length ? 0 : surpriseIndex + value; } get { return surpriseIndex; } }

    bool failsafe;

    void Start ()
	{
        StartCoroutine(StartGameCoroutine());
        if (cheerTracks != null && booTracks != null && surpriseTracks != null) failsafe = false; else failsafe = true;
        EventManager.Instance.AddEventListener(PlayerEvents.Death, OnPlayerDeath);
        EventManager.Instance.AddEventListener(PlayerEvents.TakeDamage, OnPlayerDamaged);
    }
	
	public void Cheer ()
	{
		if(Random.Range(0f,1f) <= cheerChance)
        {
            //playoneshot of the corresponding cheer sound, in the mixer.
            //ALSO WE NEED TO ADD THE DUCKING WIT SIDECHAIN SYSTEMS!
            crowdCheering.PlayOneShot(cheerTracks[Random.Range(0,cheerTracks.Length)]);
            if(booInCheer)
            {
                if (Random.Range(0f, 1f) <= booInCheerChance) crowdBooing.PlayOneShot(booTracks[Random.Range(0, booTracks.Length)], booInCheerVolume);
            }
            if (surpriseInCheer)
            {
                if (Random.Range(0f, 1f) <= surpriseInCheerChance) crowdSurprise.PlayOneShot(surpriseTracks[Random.Range(0, surpriseTracks.Length)], surpriseInCheerVolume);
            }
            CheerIndex++;
        }
	}

    public void Boo()
    {
        if (Random.Range(0f, 1f) <= cheerChance)
        {
            crowdBooing.PlayOneShot(booTracks[Random.Range(0, booTracks.Length)]);
            if (cheerInBoo)
            {
                if (Random.Range(0f, 1f) <= cheerInBooChance) crowdCheering.PlayOneShot(cheerTracks[Random.Range(0, cheerTracks.Length)], cheerInBooVolume);
            }
            if (surpriseInBoo)
            {
                if (Random.Range(0f, 1f) <= surpriseInBooChance) crowdSurprise.PlayOneShot(surpriseTracks[Random.Range(0, surpriseTracks.Length)], surpriseInBooVolume);
            }
            BooIndex++;
        }
    }

    public void Surprise()
    {
        if (Random.Range(0f, 1f) <= cheerChance)
        {
            crowdSurprise.PlayOneShot(surpriseTracks[Random.Range(0, surpriseTracks.Length)]);
            if (cheerInSurprise)
            {
                if (Random.Range(0f, 1f) <= cheerInSurpriseChance) crowdCheering.PlayOneShot(cheerTracks[Random.Range(0, cheerTracks.Length)], cheerInSurpriseVolume);
            }
            if (booInSurprise)
            {
                if (Random.Range(0f, 1f) <= booInSurpriseChance) crowdBooing.PlayOneShot(booTracks[Random.Range(0, booTracks.Length)], booInSurpriseVolume);
            }
            SurpriseIndex++;
        }
    }

    IEnumerator StartGameCoroutine()
    {
        crowdLoop.volume = 0;
        while (crowdLoop.volume < 1)
        {
            crowdLoop.volume += Time.deltaTime * fadeInSpeed;
            yield return new WaitForEndOfFrame();
        }
        crowdLoop.volume = 1;
    }

    void OnPlayerDeath(object[] parameterContainer)
    {
        var dyingPlayer = (Player)parameterContainer[0];
        var deathType = (DeathType)parameterContainer[1];
        var wasPushed = (bool)parameterContainer[2];
        var playerKiller = GameManager.Instance.Players.Where(x => x.tag == (string)parameterContainer[3]).First();

        if (deathType == DeathType.LaserGrid)
        {
            Boo();
        }

        if(deathType == DeathType.Player && dyingPlayer == playerKiller)
        {
            Boo();
        }

        else if (deathType == DeathType.Player && dyingPlayer != playerKiller)
        {
            Cheer();
        }
    }

    void OnPlayerDamaged(object[] parameterContainer)
    {
        var damage = (float)parameterContainer[1];
        var hp = (float)parameterContainer[2];

        if (damage >= criticalDamageThreshold || hp < criticalHPThreshold)
        {
            Surprise();
        }
    }
}

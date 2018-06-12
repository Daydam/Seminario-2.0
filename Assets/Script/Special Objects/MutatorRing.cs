using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MutatorRing : MonoBehaviour
{
    public Transform[] antennas;

    public Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        antennas = transform.GetComponentsInChildren<Transform>().Where(x => x.gameObject.name == "Antenna").ToArray();
    }

    public void DestroyRing()
    {
        for (int i = 0; i < antennas.Length; i++)
        {
            antennas[i].gameObject.SetActive(false);
        }

        StartCoroutine(DelayedDisable(.4f));
    }

    IEnumerator DelayedDisable(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.SetActive(false);
    }

    public void ResetRound()
    {
        transform.position = startPos;
    }

}

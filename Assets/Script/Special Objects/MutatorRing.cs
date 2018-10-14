using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MutatorRing : MonoBehaviour
{
    public Transform[] antennas;
    RingPiece[] _pieces;
    RingStructure[] _structures;
    RingWall[] _walls;

    public Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        antennas = transform.GetComponentsInChildren<Transform>().Where(x => x.gameObject.name == "Antenna").ToArray();
        _pieces = GetComponentsInChildren<RingPiece>().ToArray();
        _structures = GetComponentsInChildren<RingStructure>().ToArray();
        _walls = GetComponentsInChildren<RingWall>().ToArray();
    }

    public void DestroyRing()
    {
        for (int i = 0; i < antennas.Length; i++)
        {
            antennas[i].gameObject.SetActive(false);
        }

        foreach (var item in _structures)
        {
            item.gameObject.SetActive(false);
        }

        foreach (var item in _walls)
        {
            item.gameObject.SetActive(false);

        }

        StartCoroutine(DelayedDisable(1f));
    }

    IEnumerator DelayedDisable(float t)
    {
        var tick = t / _pieces.Length;

        var list = _pieces.Select(x => x.gameObject).ToList();

        Utility.KnuthShuffle(list);

        while (list.Any())
        {
            var rnd = UnityEngine.Random.Range(0, list.Count);
            list[rnd].SetActive(false);
            list.Remove(list[rnd]);
            yield return new WaitForSeconds(tick);
        }
    }

    public void Reactivate()
    {
        for (int i = 0; i < _pieces.Length; i++)
        {
            _pieces[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < _structures.Length; i++)
        {
            _structures[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < _walls.Length; i++)
        {
            _walls[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < antennas.Length; i++)
        {
            antennas[i].gameObject.SetActive(true);
        }
    }

    public void ResetRound()
    {
        Reactivate();
        transform.position = startPos;
        gameObject.SetActive(true);
    }

}

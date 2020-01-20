using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainRevealController : MonoBehaviour
{
    public Material m_Reveal;

    private void Update()
    {
        m_Reveal.SetVector("_CurrentPos_Drone", transform.position);
    }
}

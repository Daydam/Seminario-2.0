using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class invert : MonoBehaviour
{
    public bool laconchadteumadre;
	
	void Update ()
	{
        if (laconchadteumadre)
        {
            Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
            mesh.triangles = mesh.triangles.Reverse().ToArray();
            laconchadteumadre = false;
            //DestroyImmediate(this);
        }
	}
}

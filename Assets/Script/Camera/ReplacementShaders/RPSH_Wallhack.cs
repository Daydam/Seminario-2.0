using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PhoenixDevelopment;

public class RPSH_Wallhack : MonoBehaviour
{
    public Shader enemyWallhack;
    public Shader environmentWallhack;
    Camera _cam;

    bool _replaced;

    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    void Update()
    {

    }

    public void SetWallhackState()
    {
        /*
        if (!_replaced)
        {
            _cam.SetReplacementShader(environmentWallhack, "");
            _cam.SetReplacementShader(enemyWallhack, "Wallhack");
        }
        else _cam.ResetReplacementShader();

        //Shader.SetGlobalFloat("_WallhackState", _replaced ? 1 : 0);
        _replaced.InvertBoolean();*/
    }

    public void SetWallhackState(bool on)
    {
        /*if (on)
        {
            _cam.SetReplacementShader(environmentWallhack, "");
            _cam.SetReplacementShader(enemyWallhack, "Wallhack");
        }
        else _cam.ResetReplacementShader();

        //Shader.SetGlobalFloat("_WallhackState", on ? 1 : 0);
        */
    }
}

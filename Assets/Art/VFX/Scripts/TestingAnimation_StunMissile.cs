using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingAnimation_StunMissile : MonoBehaviour
{
    public float waitSeconds;
    public float speedLerp;
    public ParticleSystem ps;
    public ParticleSystem ps_Proyectile;
    public GameObject objectAnimated;
    public Material mat;

    private bool _activateCD;
    private float _wait;
    private Color _colorSpell;
    private float interpolator;

    public GameObject psTestProyectile;
    public float speed;

    private void Start()
    {
        _wait = waitSeconds;
        _colorSpell = mat.GetColor("_SkillStateColor");
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space) && !_activateCD)
        {
            ps.Stop();
            ps_Proyectile.Play();
            _activateCD = true;
            waitSeconds = _wait;
            objectAnimated.GetComponent<Animation>().Play();
            mat.SetFloat("_CooldownLerp", 0.0f);

            psTestProyectile.transform.position = new Vector3(-0.2f, 0.281f, 0.278f);
            psTestProyectile.GetComponentInChildren<ParticleSystem>().Play();

        }

        if(_activateCD)
        {
            waitSeconds -= Time.deltaTime;

            if(waitSeconds < 0.0f)
            {
                if (mat.GetFloat("_CooldownLerp") < 1.0f)
                {
                    interpolator += speedLerp * Time.deltaTime;
                    mat.SetFloat("_CooldownLerp", Mathf.Lerp(0.0f, 1.0f, interpolator));
                }
                else
                {
                    interpolator = 0.0f;
                    ps.Play();
                    _activateCD = false;
                    waitSeconds = 0.0f;
                }
            }
        }

        psTestProyectile.transform.position += transform.forward * speed * Time.deltaTime; 
    }
}

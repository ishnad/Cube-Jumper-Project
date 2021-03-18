using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : MonoBehaviour
{
    Renderer rend;
    public Material glitchMaterial;
    public GameObject player;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void GlitchShader()
    {
        if (player.GetComponent<PlayerStats>().PointCount >= 10)
        {
            rend.material = glitchMaterial;
        }
    }
}

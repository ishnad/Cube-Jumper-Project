using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private Text pointCount;

    private Transform hearts;
    private int health; // life counter

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    private void Start()
    {
        hearts = GameObject.FindGameObjectWithTag("HeartTransform").GetComponent<Transform>();
        pointCount = GameObject.FindGameObjectWithTag("Point").GetComponent<Text>();

        health = hearts.transform.childCount;
    }

    void Update()
    {
        pointCount.text = GetComponent<PlayerMovement>().PointCount.ToString();
    }
}

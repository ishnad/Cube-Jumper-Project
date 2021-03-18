using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private Transform hearts;
    private int health; // life counter

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int pointCount = 0;

    public int PointCount
    {
        get { return pointCount; }
        set { pointCount = value; }
    }

    private void Start()
    {
        hearts = GameObject.FindGameObjectWithTag("HeartTransform").GetComponent<Transform>();
        health = hearts.transform.childCount;
    }
}

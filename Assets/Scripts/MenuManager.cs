using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private Text pointCount;
    private PlayerStats playerStats;
    private bool restart = false;
    private bool doOnce = false;

    [SerializeField]
    private Transform deathScreen;

    [SerializeField]
    private Text totalPoints;

    void Start()
    {
        pointCount = GameObject.FindGameObjectWithTag("Point").GetComponent<Text>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (playerStats.Health <= 0) // if player dies
        {
            restart = true;
            deathScreen.gameObject.SetActive(true);

            if (!doOnce)
            {
                doOnce = true;
                totalPoints.text += playerStats.PointCount.ToString();
            }
        }
        else
        {
            pointCount.text = playerStats.PointCount.ToString();

            if (restart)
            {
                restart = false;
                doOnce = false;
                deathScreen.gameObject.SetActive(false);
            }
        }
    }
}

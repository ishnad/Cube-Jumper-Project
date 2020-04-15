using UnityEngine;

public class SpikeController : MonoBehaviour
{
    private Transform walls;

    private PlayerMovement player;
    private PlayerStats playerStats;

    private int spikeNumber = -1;
    private int spikeNumber2 = -1;
    private int spikeNumber3 = -1;
    private int secondDiffThreshold = 4;
    private int thirdDiffThreshold = 8;

    private float timeBetweenSpawn = 2f;
    private float startTimeBetweenSpawn;
    private float decreaseTime = 0.1f;
    private float minTime = 1.1f;
    private float warningTime;
    private float warningTimeInterval = 0.5f;

    private bool doOnce = false;
    private bool doOnceStart = false;
    private bool firstSpikeCheck = false;
    private bool secondDiffBoolean = false;
    private bool thirdDiffBoolean = false;

    void Start()
    {
        walls = GameObject.FindGameObjectWithTag("WallTransform").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        warningTime = timeBetweenSpawn - warningTimeInterval;
        startTimeBetweenSpawn = timeBetweenSpawn;
    }

    void Update()
    {
        if (playerStats.Health > 0)
            SpikeSpawner();
    }

    private void SpikeSpawner()
    {
        if (timeBetweenSpawn <= 0) // when it is time to spawn
        {
            doOnce = false;

            timeBetweenSpawn = startTimeBetweenSpawn; // set next time
            warningTime = startTimeBetweenSpawn - warningTimeInterval; // set next warning time

            if (secondDiffBoolean) // start of start time reduce
            {
                if (startTimeBetweenSpawn > minTime) // if start time is more than minimum set time
                    startTimeBetweenSpawn -= decreaseTime; // decrease the next start time
            }

            walls.GetChild(spikeNumber).GetComponent<SpriteRenderer>().color = Color.white; // make no warning on wall
            transform.GetChild(spikeNumber).gameObject.SetActive(true); // set spike to be true

            if (secondDiffBoolean && spikeNumber2 != -1)
            {
                walls.GetChild(spikeNumber2).GetComponent<SpriteRenderer>().color = Color.white; // make no warning on wall
                transform.GetChild(spikeNumber2).gameObject.SetActive(true); // set spike to be true
            }
            if (thirdDiffBoolean && spikeNumber3 != -1)
            {
                walls.GetChild(spikeNumber3).GetComponent<SpriteRenderer>().color = Color.white; // make no warning on wall
                transform.GetChild(spikeNumber3).gameObject.SetActive(true); // set spike to be true
            }

            firstSpikeCheck = true;
        }
        else if (timeBetweenSpawn <= warningTime) // when it is time to give warning
        {
            timeBetweenSpawn -= Time.deltaTime; // timer reduce
            player.CanGainPoint = false;

            if (!doOnce) // only do this once
            {
                doOnce = true;

                if (firstSpikeCheck)
                {
                    transform.GetChild(spikeNumber).gameObject.SetActive(false); // make spike not there
                    if (spikeNumber2 != -1)
                        transform.GetChild(spikeNumber2).gameObject.SetActive(false); // make spike not there
                    if (spikeNumber3 != -1)
                        transform.GetChild(spikeNumber3).gameObject.SetActive(false); // make spike not there

                    player.CanGainPoint = true; // player can gain point during this once

                    spikeNumber = Random.Range(0, 4); // random which spike to spawn
                    walls.GetChild(spikeNumber).GetComponent<SpriteRenderer>().color = Color.red; // set warning colour

                    if (secondDiffBoolean)
                    {
                        do
                        {
                            spikeNumber2 = Random.Range(0, 4);
                        } while (spikeNumber2 == spikeNumber);
                        walls.GetChild(spikeNumber2).GetComponent<SpriteRenderer>().color = Color.red; // set warning colour
                    }

                    if (thirdDiffBoolean)
                    {
                        do
                        {
                            spikeNumber3 = Random.Range(0, 4);
                        } while (spikeNumber3 == spikeNumber2 || spikeNumber3 == spikeNumber);
                        walls.GetChild(spikeNumber3).GetComponent<SpriteRenderer>().color = Color.red; // set warning colour
                    }
                }
                if (!doOnceStart)
                {
                    doOnceStart = true;
                    spikeNumber = Random.Range(0, 4); // random which spike to spawn
                    walls.GetChild(spikeNumber).GetComponent<SpriteRenderer>().color = Color.red; // set warning colour
                }
            }
        }
        else
        {
            if (playerStats.PointCount >= secondDiffThreshold)
                secondDiffBoolean = true;
            if (playerStats.PointCount >= thirdDiffThreshold)
                thirdDiffBoolean = true;

            timeBetweenSpawn -= Time.deltaTime; // timer reduce
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	Scene scene;

    public GameObject particlePrefab = null;

	private Transform walls;
	private Transform hearts;

	private int heartCount; // count how many hearts in the scene
	
	private Quaternion zeroQuaternion = Quaternion.Euler(0f, 0f, 0f);

	private bool tookDamage = false; // check if player takes damage


	public bool TookDamage
	{
		set { tookDamage = value; }
	}

	private bool canGainPoint = false;
	
	public bool CanGainPoint
	{
		set { canGainPoint = value; }
	}

	private bool diffCheckOnce = false;

	private PlayerStats playerStats;

	private float playerSpeed = 12.0f; // set player speed
	private float playerSpeedStraightMultiplier = 2.0f; // set player speed
	private float playerSpeedReset; // to reset player speed
	private float offset = 0.35f;
	private float diffCheck = 0f; // to check if player moves diagonally or straight

	// Factor of the screen width that we consider a swipe
	// 0.17 works well for portrait mode 16:9 phone
	public const float MIN_SWIPE_DISTANCE = 0.17f;

	public static bool swipedRight = false;
	public static bool swipedLeft = false;
	public static bool swipedUp = false;
	public static bool swipedDown = false;
	public bool debugWithArrowKeys = true;

	Vector2 startPos;

	void Start()
	{
		scene = SceneManager.GetActiveScene();

		hearts = GameObject.FindGameObjectWithTag("HeartTransform").GetComponent<Transform>();
		walls = GameObject.FindGameObjectWithTag("WallTransform").GetComponent<Transform>();
		playerStats = GetComponent<PlayerStats>();

		transform.position = walls.GetChild(1).GetChild(0).position;
		
		playerSpeedReset = playerSpeed;
		
		Reset();
	}

	public void Update()
	{
		if (playerStats.Health > 0) // player alive
		{
			if (!swipedUp && !swipedDown && !swipedLeft && !swipedRight) // this ensures we dont get double input
				SwipeChecker();

			PlayerMove();

			if (!tookDamage && canGainPoint)
				playerStats.PointCount++;
		}
	}

	private void PlayerMove()
	{
		float step = playerSpeed * Time.deltaTime; // how fast player moves with deltaTime

		/* wall number
		 * top = 0
		 * bottom = 1
		 * left = 2
		 * right = 3
		 * second child is always 0
		 */
		if (swipedUp) // up
		{
			diffCheck = transform.position.y - walls.GetChild(0).GetChild(0).position.y; // checks distance between player and destination

			if (diffCheck <= -3.45f && !diffCheckOnce) // if player is moving straight
			{
				diffCheckOnce = true;
				playerSpeed *= playerSpeedStraightMultiplier;
			}
			else if (diffCheck == 0f) // if player reaches destination
			{
				diffCheckOnce = false;
				swipedUp = false;
			}
			else if (diffCheck >= -offset) // 0.3 offset
			{
				transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(0).GetChild(0).position, step);
				transform.rotation = zeroQuaternion;
			}
			else
			{
				// moves towards destination
				transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(0).GetChild(0).position, step);
				transform.Rotate(0f, 0f, 45f, Space.World);
			}
		}
		if (swipedDown) // down
		{
			diffCheck = transform.position.y - walls.GetChild(1).GetChild(0).position.y; // checks distance between player and destination

			if (diffCheck >= 3.45f && !diffCheckOnce) // if player is moving straight
			{
				diffCheckOnce = true;
				playerSpeed *= playerSpeedStraightMultiplier;
			}
			else if (diffCheck == 0f) // if player reaches destination
			{
				diffCheckOnce = false;
				swipedDown = false;
			}
			else if (diffCheck <= offset) // 0.3 offset
			{
				transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(1).GetChild(0).position, step);
				transform.rotation = zeroQuaternion;
			}
			else
			{
				// moves towards destination
				transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(1).GetChild(0).position, step);
				transform.Rotate(0f, 0f, 45f, Space.World);
			}
		}
		if (swipedLeft) // left
		{
			diffCheck = transform.position.x - walls.GetChild(2).GetChild(0).position.x; // checks distance between player and destination

			if (diffCheck >= 3.45f && !diffCheckOnce) // if player is moving straight
			{
				diffCheckOnce = true;
				playerSpeed *= playerSpeedStraightMultiplier;
			}
			else if (diffCheck == 0f) // if player reaches destination
			{
				diffCheckOnce = false;
				swipedLeft = false;
			}
			else if (diffCheck <= offset) // 0.3 offset
			{
				transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(2).GetChild(0).position, step);
				transform.rotation = zeroQuaternion;
			}
			else
			{
				// moves towards destination
				transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(2).GetChild(0).position, step);
				transform.Rotate(0f, 0f, 45f, Space.World);
			}
		}
		if (swipedRight) // right
		{
			diffCheck = transform.position.x - walls.GetChild(3).GetChild(0).position.x; // checks distance between player and destination

			if (diffCheck <= -3.45f && !diffCheckOnce) // if player is moving straight
			{
				diffCheckOnce = true;
				playerSpeed *= playerSpeedStraightMultiplier;
			}
			else if (diffCheck == 0f) // if player reaches destination
			{
				diffCheckOnce = false;
				swipedRight = false;
			}
			else if (diffCheck >= -offset) // 0.3 offset
			{
				transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(3).GetChild(0).position, step);
				transform.rotation = zeroQuaternion;
			}
			else
			{
				// moves towards destination
				transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(3).GetChild(0).position, step);
				transform.Rotate(0f, 0f, 45f, Space.World);
			}
		}
	}

	private void SwipeChecker()
	{
		if (debugWithArrowKeys)
		{
			swipedDown = swipedDown || Input.GetKeyDown(KeyCode.DownArrow);
			swipedUp = swipedUp || Input.GetKeyDown(KeyCode.UpArrow);
			swipedRight = swipedRight || Input.GetKeyDown(KeyCode.RightArrow);
			swipedLeft = swipedLeft || Input.GetKeyDown(KeyCode.LeftArrow);
		}

		if (Input.touches.Length > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				startPos = new Vector2(touch.position.x / Screen.width, touch.position.y / Screen.width);
			}
			if (touch.phase == TouchPhase.Ended)
			{
				Vector2 endPos = new Vector2(touch.position.x / Screen.width, touch.position.y / Screen.width);

				Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

				if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
					return;

				if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y)) // Horizontal swipe
				{
					if (swipe.x > 0)
					{
						swipedRight = true;
					}
					else
					{
						swipedLeft = true;
					}
				}
				else // Vertical swipe
				{
					if (swipe.y > 0)
					{
						swipedUp = true;
					}
					else
					{
						swipedDown = true;
					}
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Spike") // if player hits spike
		{
			playerStats.Health--; // minus health
			tookDamage = true; // took damage
			heartCount = hearts.transform.childCount; // check how many hearts on the screen
			Destroy(hearts.transform.GetChild(heartCount - 1).gameObject); // destroy a heart
		}
		else if (collision.gameObject.tag == "Wall")
		{
            if (!tookDamage) // if player didnt take damage that move
            {
                var particle = Instantiate(particlePrefab, collision.transform); // particle;

				Destroy(particle.gameObject, 0.3f);
            }
			Reset();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Spike") // to reset tookDamage
			tookDamage = true; // took damage
		else
			tookDamage = false;

		//if (collision.gameObject.tag == "Wall")
		//	Reset();
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(scene.name);
	}

	public void BackToMainMenu()
	{
		SceneManager.LoadScene("MainmenuScene");
	}

	// reset booleans and speed
	private void Reset()
	{
		playerSpeed = playerSpeedReset;
		swipedRight = false;
		swipedLeft = false;
		swipedUp = false;
		swipedDown = false;
	}
}

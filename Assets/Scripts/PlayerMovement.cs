using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	Scene scene;

	private Transform walls;
	private Transform hearts;

	private int heartCount; // count how many hearts in the scene
	private bool tookDamage = false; // check if player takes damage
	private int pointCount = -1; // point counter set to -1 so when start, it goes to 0

	public int PointCount
	{
		get { return pointCount; }
	}

	private PlayerStats playerStats;

	private float playerSpeed = 10.0f; // set player speed
	private float playerSpeedReset; // to reset player speed

	private float diffCheck = 0f; // to check if player moves diagonally or straight

	// If the touch is longer than MAX_SWIPE_TIME, we dont consider it a swipe
	public const float MAX_SWIPE_TIME = 0.5f;

	// Factor of the screen width that we consider a swipe
	// 0.17 works well for portrait mode 16:9 phone
	public const float MIN_SWIPE_DISTANCE = 0.17f;

	public static bool swipedRight = false;
	public static bool swipedLeft = false;
	public static bool swipedUp = false;
	public static bool swipedDown = false;
	public bool debugWithArrowKeys = true;

	Vector2 startPos;
	float startTime;

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
		if (!swipedUp && !swipedDown && !swipedLeft && !swipedRight) // this ensures we dont get double input
			SwipeChecker();

		PlayerMove();

		if (playerStats.Health <= 0) // if player dies
		{
			SceneManager.LoadScene(scene.name);
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
		if (swipedUp)
		{
			diffCheck = transform.position.y - walls.GetChild(0).GetChild(0).position.y; // checks distance between player and destination

			if (diffCheck == 0f) // if player reaches destination
				swipedUp = false;
			else if (diffCheck <= -3.45f) // if player is moving straight
				playerSpeed *= 1.5f;

			// moves towards destination
			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(0).GetChild(0).position, step);
		}
		if (swipedDown)
		{
			diffCheck = transform.position.y - walls.GetChild(1).GetChild(0).position.y;

			if (diffCheck == 0f)
				swipedDown = false;
			else if (diffCheck >= 3.45f)
				playerSpeed *= 1.5f;

			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(1).GetChild(0).position, step);
		}
		if (swipedLeft)
		{
			diffCheck = transform.position.x - walls.GetChild(2).GetChild(0).position.x;

			if (diffCheck == 0f)
				swipedLeft = false;
			else if (diffCheck >= 3.45f)
				playerSpeed *= 1.5f;

			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(2).GetChild(0).position, step);
		}
		if (swipedRight)
		{
			diffCheck = transform.position.x - walls.GetChild(3).GetChild(0).position.x;

			if (diffCheck == 0f)
				swipedRight = false;
			else if (diffCheck <= -3.45f)
				playerSpeed *= 1.5f;

			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(3).GetChild(0).position, step);
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
				startTime = Time.time;
			}
			if (touch.phase == TouchPhase.Ended)
			{
				if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
					return;

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
			heartCount = hearts.transform.childCount; // check how many hearts on the screen
			Destroy(hearts.transform.GetChild(heartCount - 1).gameObject); // destroy a heart
			tookDamage = true; // took damage
		}
		else if (collision.gameObject.tag == "Wall")
		{
			if (!tookDamage) // adds points if player didnt take damagem that move
				pointCount++;
			Reset();
		}
	}

	// reset booleans and speed
	private void Reset()
	{
		playerSpeed = playerSpeedReset;
		swipedRight = false;
		swipedLeft = false;
		swipedUp = false;
		swipedDown = false;
		tookDamage = false;
	}
}

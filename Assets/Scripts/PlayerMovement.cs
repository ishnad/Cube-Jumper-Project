using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
	public Transform walls;
	private float playerSpeed = 10.0f;
	private Rigidbody2D playerRB;
    void Start()
    {
		playerRB = gameObject.GetComponent<Rigidbody2D>();
    }

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

	public void Update()
	{
		float step = playerSpeed * Time.deltaTime;

		if (!swipedUp && !swipedDown && !swipedLeft && !swipedRight)
			SwipeChecker();

		/* wall number
		 * top = 0
		 * bottom = 1
		 * left = 2
		 * right = 3
		 * second child is always 0
		 */
		if (swipedUp)
		{
			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(0).GetChild(0).position, 3 * Time.deltaTime);
			Debug.Log("Up Swipe");
		}
		if (swipedDown)
		{
			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(1).GetChild(0).position, 3 * Time.deltaTime);
			Debug.Log("Down Swipe");
		}
		if (swipedLeft)
		{
			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(2).GetChild(0).position, 3 * Time.deltaTime);
			Debug.Log("Left Swipe");
		}
		if (swipedRight)
		{
			transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), walls.GetChild(3).GetChild(0).position, 3 * Time.deltaTime);
			Debug.Log("Right Swipe");
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
		if (collision.gameObject.tag == "Wall")
		{
			Debug.Log("Entered collision player wall");
			playerRB.velocity = new Vector2(0, 0);
			swipedRight = false;
			swipedLeft = false;
			swipedUp = false;
			swipedDown = false;
		}
	}
}

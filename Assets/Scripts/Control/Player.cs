using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public Snake snake;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.RightArrow))
			snake.ChangeDirection(Vector2.right);
		else if (Input.GetKey(KeyCode.DownArrow))
			snake.ChangeDirection(Vector2.down);
		else if (Input.GetKey(KeyCode.LeftArrow))
			snake.ChangeDirection(Vector2.left);
		else if (Input.GetKey(KeyCode.UpArrow))
			snake.ChangeDirection(Vector2.up);
	}
}

using UnityEngine;

public class Player : MonoBehaviour {
    Snake snake;

    void Start () {
        snake = GetComponent<Snake>();
    }

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

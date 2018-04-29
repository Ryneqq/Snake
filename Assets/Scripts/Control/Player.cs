using UnityEngine;

public class Player : MonoBehaviour {
    Snake snake;
    bool gotInput = false;

    void Start () {
        snake = GetComponent<Snake>();
        InvokeRepeating("ResetInput", 0.3f, 0.3f);
    }

    void Update () {
        GetInput();
    }

    private void ResetInput() {
        gotInput = false;
    }

    private void GetInput() {
        if (Input.GetKey(KeyCode.RightArrow))
            ChangeDirection(Map.Side.right);
        else if (Input.GetKey(KeyCode.DownArrow))
            ChangeDirection(Map.Side.down);
        else if (Input.GetKey(KeyCode.LeftArrow))
            ChangeDirection(Map.Side.left);
        else if (Input.GetKey(KeyCode.UpArrow))
            ChangeDirection(Map.Side.up);
    }

    private void ChangeDirection(Map.Side side){
        if(gotInput == true)
            return;

        if(snake.Direction() == Map.Side.right || snake.Direction() == Map.Side.left) {
            if (side == Map.Side.up || side == Map.Side.down) {
                snake.ChangeDirection(side);
                gotInput = true;
            }
        } else if(snake.Direction() == Map.Side.up || snake.Direction() == Map.Side.down) {
            if (side == Map.Side.right || side == Map.Side.left) {
                snake.ChangeDirection(side);
                gotInput = true;
            }
        }
    }
}

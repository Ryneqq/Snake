using UnityEngine;

public class Player : MonoBehaviour {
    Snake snake;
    bool gotInput             = false;
    AvaibleSides upAndDown    = new AvaibleSides(Map.Side.up,    Map.Side.down);
    AvaibleSides rightAndLeft = new AvaibleSides(Map.Side.right, Map.Side.left);

    void Start ()
    {
        this.snake = GetComponent<Snake>();
        InvokeRepeating("MoveSnake", 0.3f, 0.3f);
    }

    void Update ()
    {
        this.GetInput();
    }

    private void MoveSnake()
    {
        // this.snake.Move(); // Uncomment when script moved to its own snake (now it is on NN snake)
        this.ResetInput();
    }

    private void SetInput()
    {
        this.gotInput = true;
    }

    private void ResetInput()
    {
        this.gotInput = false;
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            ChangeDirection(Map.Side.right);
        else if (Input.GetKey(KeyCode.DownArrow))
            ChangeDirection(Map.Side.down);
        else if (Input.GetKey(KeyCode.LeftArrow))
            ChangeDirection(Map.Side.left);
        else if (Input.GetKey(KeyCode.UpArrow))
            ChangeDirection(Map.Side.up);
    }

    private void ChangeDirection(Map.Side side)
    {
        if(gotInput)
            return;

        if(ActualSide(this.rightAndLeft))
            this.SetDirection(side, this.upAndDown);
        else if(ActualSide(this.upAndDown))
            this.SetDirection(side, this.rightAndLeft);
    }

    private bool ActualSide(AvaibleSides avaibleSides)
    {
        return avaibleSides.CheckAvaiblity(this.snake.Direction());
    }

    private void SetDirection(Map.Side side, AvaibleSides avaibleSides)
    {
        if (avaibleSides.CheckAvaiblity(side)) {
            this.snake.ChangeDirection(side);
            this.SetInput();
        }
    }

    private class AvaibleSides
    {
        private Map.Side one;
        private Map.Side another;

        public AvaibleSides(Map.Side one, Map.Side another)
        {
            this.one = one;
            this.another = another;
        }

        public bool CheckAvaiblity(Map.Side side)
        {
            return side == this.one || side == this.another;
        }
    }
}

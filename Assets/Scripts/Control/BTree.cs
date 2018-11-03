using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTree: MonoBehaviour {
    Snake snake;
    BehavioralTree BT;
   void Start ()
    {
        this.snake = GetComponent<Snake>();
        this.BT = new BehavioralTree();
    }

    public void MoveSnake()
    {
        var dir = BT.Run(this.snake);
        this.snake.ChangeDirection(dir);
        this.snake.Move();
    }
}

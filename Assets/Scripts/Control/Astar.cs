using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour {
    Snake snake;
    Pathfinder pathfinder;

    void Start ()
    {
        this.snake = GetComponent<Snake>();
        this.pathfinder = new Pathfinder();
    }

    public void MoveSnake()
    {
        var start = this.snake.Head();
        var nextStep = this.pathfinder.FindPath(this.snake.Head(), this.snake.Food());


        if (nextStep != start)
        {
            var direction = Map.Direction(nextStep, this.snake.Head());
            this.snake.Move(nextStep);
        }
        else
        {
            this.snake.Move();
        }
    }
}

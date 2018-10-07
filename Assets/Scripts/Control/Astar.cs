using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour {
    Snake snake;
    CustomPathfinder pathfinder;

    void Start ()
    {
        this.snake = GetComponent<Snake>();
        this.pathfinder = new CustomPathfinder();
        InvokeRepeating("MoveSnake", 0.1f, 0.1f);
    }

    void Update ()
    {

    }

    private void MoveSnake()
    {
        this.TakeStep();
    }

    private void TakeStep()
    {
        // var start = Map.Neighbour(this.snake.Head(), this.snake.Direction());
        var start = this.snake.Head();
        // var nextStep = new CustomPathfinder(start, this.snake.Food()).FindPath();
        var nextStep = this.pathfinder.FindPath(this.snake.Head(), this.snake.Food());


        if (nextStep != start)
        {
            // Debug.Log("I found this fucking a path");
            var direction = Map.Direction(nextStep, this.snake.Head());
            // this.snake.Turn(direction);
            // this.snake.Slither(nextStep);
            this.snake.Move(nextStep);
        }
        else
        {
            // Debug.Log("Didnt find a path");
            this.snake.Move();
        }
    }
}

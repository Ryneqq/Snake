using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class NN : MonoBehaviour {
    public bool create = false; // shall we create new nn

    Snake snake;
    NeuralNetwork nn;
    Matrix P; // examples
    Matrix T; // correct resposes
    bool examples = false; // examples were loaded from file

    void Start() {
        snake = GetComponent<Snake>();

        if(create)
            CreateNeuralNetwork();
        else
            LoadNeuralNetwork();

        // InvokeRepeating("Steer", 0.5f, 0.5f);
    }

    private void CreateNeuralNetwork() {
        int[] layers = {10, 16, 8, 4, 2};
        nn = new NeuralNetwork(layers);

        Learn();
    }

    private void LoadNeuralNetwork() {
        nn = NeuralNetwork.LoadNeuralNetwork();
    }

    public void Learn() {
        if(!examples) {
            LoadExamples();
        }
        nn.Learn(P, T, 10000);
        nn.SaveNeuralNetwork();
    }

    private void LoadExamples(){
        string p = string.Empty, t = string.Empty;
        string[] question = new string[10], anwser = new string[2];
        string newLine = "\r\n";

        string loaded = Load.FromFile("examples/examples.txt");
        string[] examples = loaded.Split(';');

        for (int i = 0; i < examples.GetLength(0); i++){
            var arr = examples[i].Split(',');

            if(i >= examples.GetLength(0) - 1)
                newLine = String.Empty;

            Array.Copy(arr, 0, question, 0, 10);
            p += String.Join(" ", question) + newLine;
            Array.Copy(arr, 10, anwser, 0, 2);
            t += String.Join(" ", anwser) + newLine;
        }

        P = Matrix.Transpose(Matrix.Parse(p));
        T = Matrix.Transpose(Matrix.Parse(t));

        this.examples = true;
    }

    private Vector2 DistanceToFood() {
        var food = snake.food.Position();
        return new Vector2(snake.Head().x - food.x, snake.Head().y - food.y);
    }

    private double[] GetView() { // fixed but still no idea how it works
        // var pos = snake.Head();
        // var view = new Matrix(3,3);

        // for(int i = -1; i <= 1; i++)
        //     for(int j = -1; j <= 1; j++)
        //         view[i+1, j+1] = (int)Map.map[pos.x + j, pos.y + i].field;
        // view = Matrix.SwitchRows(view,0,2);

        // return view;

        var view = new double[6];
        var pos  = this.snake.Head();
        var dir  = this.snake.Direction();
        var next  = Map.Neighbour(pos, dir);

        view[0] = (double)  Map.Neighbour(next, Map.Left(dir)).field;
        view[1] = (double)  next.field;
        view[2] = (double)  Map.Neighbour(next, Map.Right(dir)).field;
        view[3] = (double)  Map.Neighbour(pos, Map.Left(dir)).field;
        view[4] = (double)  pos.field;
        view[5] = (double)  Map.Neighbour(pos, Map.Right(dir)).field;

        return view;
    }

    private Matrix TurnView(Matrix view) {
        var dir = snake.Direction();

        switch (dir) {
            case Map.Side.right:
                view = Matrix.SwitchCols(view, 0, 2);
                view = Matrix.Transpose(view);
                break;
            case Map.Side.down:
                view = Matrix.SwitchCols(view, 0, 2);
                view = Matrix.SwitchRows(view, 0, 2);
                break;
            case Map.Side.left:
                view = Matrix.SwitchRows(view, 0, 2);
                view = Matrix.Transpose(view);
                break;
            default: break;
        }

        return view;
    }

    private void ChangePerceptionOfTheFood(Matrix perception) {
        for(int i = 0; i < perception.rows; i++)
            for(int j = 0; j < perception.cols; j++)
                if(perception[i,j] == 3)
                    perception[i,j] = -1;
    }

    private void ChangeDirection(Matrix perception) {
        var dir = nn.Run(perception);
        var threshold = .5f;

        if(dir[0,0] > threshold && dir[1,0] < threshold) {
            snake.Turn(Map.Side.right);
        } else if(dir[0,0] < threshold && dir[1,0] > threshold) {
            snake.Turn(Map.Side.left);
        }

        // Debug.Log(dir.ToString());
    }

    private Matrix CreatePerception() {
        // var view = TurnView(GetView());
        var view = GetView();
        var perception = new Matrix(10, 1);
        var k = 0;

        // for (int i = 0; i < 2; i++)
        //     for (int j = 0; j <= 2; j++)
        //         perception[k++, 0] = view[i,j];
        for (int i = 0; i < 6; i++)
            perception[i,0] = view[i];

        var dir = Map.Direction(snake.Direction());
        perception[6, 0] = dir.x;
        perception[7, 0] = dir.y;

        var dist = DistanceToFood();
        perception[8, 0] = dist.x;
        perception[9, 0] = dist.y;

        return perception;
    }

    public void MoveSnake(){
        var perception = CreatePerception();
        ChangePerceptionOfTheFood(perception);
        ChangeDirection(perception);
        Debug.Log(perception.ToString());
        this.snake.Move();
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class NN : MonoBehaviour {
    public bool create = false; // shall we create new nn
    public bool learn = false; // shall we create new nn

    Snake snake;
    Perception perception;
    NeuralNetwork nn;
    Matrix P; // examples
    Matrix T; // correct resposes
    bool examples = false; // examples were loaded from file

    void Start() {
        this.snake = GetComponent<Snake>();
        perception = new Perception(this.snake);

        if(create)
            CreateNeuralNetwork();
        else
            LoadNeuralNetwork();

        // InvokeRepeating("Steer", 0.5f, 0.5f);
    }

    private void CreateNeuralNetwork() {
        int[] layers = {10, 8, 4, 2};
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

    private Matrix GetDirection(Matrix perception)
    {
        return this.nn.Run(perception);
    }

    private void ChangeDirection(Matrix dir)
    {
        var threshold = .5f;

        if(dir[0,0] > threshold && dir[1,0] < threshold) {
            snake.Turn(Map.Side.right);
        } else if(dir[0,0] < threshold && dir[1,0] > threshold) {
            snake.Turn(Map.Side.left);
        }
    }

    private void TryLearn(Matrix perception)
    {
        if (this.learn)
        {
            // var anwser = this.GetRightAnwser();

            // for (int i = 0; i < 100; i++)
            // {
            //     if(this.nn.Learn(perception, anwser))
            //     {
            //         break;
            //     }
            // }
        }
    }

    public void MoveSnake(){
        var perception = this.perception.CreatePerception();
        this.TryLearn(perception);

        var dir = this.GetDirection(perception);
        this.ChangeDirection(dir);
        this.snake.Move();
    }
}

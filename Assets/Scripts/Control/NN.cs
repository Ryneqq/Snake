﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NN : MonoBehaviour {
    Snake snake;
    NeuralNetwork nn;
    Matrix P; // examples
    Matrix T; // correct resposes
    bool create = false; // shall we create new nn
    bool examples = false; // examples were loaded from file
    float distance = 10f;

    // Use this for initialization
    void Start() {
        snake = GetComponent<Snake>();
        if(create){
            CreateNeuralNetwork();
        } else {
            LoadNeuralNetwork();
        }
        InvokeRepeating("Steer", .3f, .3f);
    }

    private void CreateNeuralNetwork() {
        int[] layers = new int[3];
        layers[0] = 8;
        layers[1] = 4;
        layers[2] = 2;
        nn = new NeuralNetwork(6, layers);
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
        string loaded = Load.FromFile("examples");
        string[] frames = loaded.Split(';');
        string[] example;
        string p = string.Empty, t = string.Empty;
        for(int i = 0; i < frames.Length; i++){
            example = frames[i].Split(',');
            p += Matrix.Transpose(Matrix.Parse(example[0])).ToString();
            t += Matrix.Transpose(Matrix.Parse(example[1])).ToString();
        }

        P = Matrix.Transpose(Matrix.Parse(p));
        T = Matrix.Transpose(Matrix.Parse(t));
        Debug.Log(P.rows);
        Debug.Log(T.rows);

        examples = true;
    }

    private int ApproachToFood () {
        var actual = Vector2.Distance(snake.Head().pos, snake.food.Position());

        if(distance >= actual){
            distance = actual;
            return -1;
        } else {
            distance = actual;
            return 1;
        }
    }

    private void ChangePerceptionOfTheFood(Matrix perception) {
        for(int i = 0; i < perception.rows; i++){
            if(perception[i,0] == 3){
                perception[i,0] = -1;
            }
        }
    }

    private void ChangeDirection(Matrix perception) {
        Matrix dir = nn.Run(perception);
        if(dir[0,0] > .5f && dir[1,0] < .5f){
            snake.Turn("right");
        } else if(dir[0,0] < .5f && dir[1,0] > .5f){
            snake.Turn("left");
        }

        Debug.Log(dir.ToString());
    }

    private Matrix CreatePerception() {
        var perception = new Matrix(6, 1);
        int x, y;
        snake.Turn("right");
        x = (int)snake.Dir().x; y = (int)snake.Dir().y;
        perception[0,0] = (int)Map.map[snake.Head().x + x, snake.Head().y + y].field;
        snake.Turn("left");
        x = (int)snake.Dir().x; y = (int)snake.Dir().y;
        perception[2,0] = (int)Map.map[snake.Head().x + x, snake.Head().y + y].field;
        snake.Turn("left");
        x = (int)snake.Dir().x; y = (int)snake.Dir().y;
        perception[4,0] = (int)Map.map[snake.Head().x + x, snake.Head().y + y].field;
        snake.Turn("right");
        x = snake.Head().x + (int)snake.Dir().x; 
        y = snake.Head().y + (int)snake.Dir().y;
        snake.Turn("right");
        x += (int)snake.Dir().x; y += (int)snake.Dir().y;
        perception[1,0] = (int)Map.map[x,y].field; 
        x -= (int)snake.Dir().x; y -= (int)snake.Dir().y;
        snake.Turn("left");
        snake.Turn("left");
        x += (int)snake.Dir().x; y += (int)snake.Dir().y;
        perception[3,0] = (int)Map.map[x,y].field;
        snake.Turn("right");
        perception[5,0] = ApproachToFood();

        return perception;
    }

    private void Steer(){
        Matrix perception = CreatePerception();
        ChangePerceptionOfTheFood(perception);
        ChangeDirection(perception);
    }
}

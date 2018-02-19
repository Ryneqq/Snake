using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// klasa do testowania działania klasy NeuralNetwork
public class XOR : MonoBehaviour {
    Matrix P = Matrix.Parse("0 0 1 1\r\n0 1 0 1");
    Matrix T = Matrix.Parse("0 1 1 0");

    NeuralNetwork nn;

    void Start () {
        int[] layers = new int[3];
        layers[0] = 4;
        layers[1] = 2;
        layers[2] = 1;
        nn = new NeuralNetwork(2, layers);

        Debug.Log("Before learning");
        Debug.Log("Correct: 0, Network response: " + nn.Run(Matrix.Parse("0\r\n0")).ToString());
        Debug.Log("Correct: 1, Network response: " + nn.Run(Matrix.Parse("1\r\n0")).ToString());
        Debug.Log("Correct: 1, Network response: " + nn.Run(Matrix.Parse("0\r\n1")).ToString());
        Debug.Log("Correct: 0, Network response: " + nn.Run(Matrix.Parse("1\r\n1")).ToString());
        Learn();
    }

    public void Learn(){
        nn.Learn(P,T,40000);
        Debug.Log("After learning");
        Debug.Log("Correct: 0, Network response: " + nn.Run(Matrix.Parse("0\r\n0")).ToString());
        Debug.Log("Correct: 1, Network response: " + nn.Run(Matrix.Parse("1\r\n0")).ToString());
        Debug.Log("Correct: 1, Network response: " + nn.Run(Matrix.Parse("0\r\n1")).ToString());		
        Debug.Log("Correct: 0, Network response: " + nn.Run(Matrix.Parse("1\r\n1")).ToString());
    }
}
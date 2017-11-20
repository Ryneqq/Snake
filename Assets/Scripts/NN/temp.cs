﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// klasa do testowania działania klasy NeuralNetwork
public class temp : MonoBehaviour {

	Matrix P = Matrix.Parse("0 0 1 1\r\n0 1 0 1");
	Matrix T = Matrix.Parse("0 1 1 0");
	Matrix Pr = Matrix.Parse("1\r\n0");
	NeuralNetwork nn;
	


	// Use this for initialization
	void Start () {

		int[] layers = new int[2];
		layers[0] = 2;
		layers[1] = 1;
		// layers[2] = 1;		
		nn = new NeuralNetwork(2, layers);		
				

		Debug.Log("Before learning");
		Debug.Log("Correct: 0, Network response: " + nn.Run(Matrix.Parse("0\r\n0")).ToString());
		Debug.Log("Correct: 1, Network response: " + nn.Run(Matrix.Parse("1\r\n0")).ToString());
		Debug.Log("Correct: 1, Network response: " + nn.Run(Matrix.Parse("0\r\n1")).ToString());		
		Debug.Log("Correct: 0, Network response: " + nn.Run(Matrix.Parse("1\r\n1")).ToString());		
		
		// int wartosc = 0;
		// Matrix tab = new Matrix(9,9);
		// for(int k = 0; k < 3; k++){
		// 	for(int i = k*3, l = 0; l < 3; i++, l++){
		// 		for(int j = k*3; j < k*3+3; j++){
		// 			tab[i,j] = wartosc++;
		// 			Debug.Log("i: " + i + " j: " + j);
		// 		}
		// 	}
		// }
		// Debug.Log(tab.ToString());
	}

	public void Learn(){
		nn.Learn(P,T,10000);
		Debug.Log("After learning");
		Debug.Log("Correct: 0, Network response: " + nn.Run(Matrix.Parse("0\r\n0")).ToString());
		Debug.Log("Correct: 1, Network response: " + nn.Run(Matrix.Parse("1\r\n0")).ToString());
		Debug.Log("Correct: 1, Network response: " + nn.Run(Matrix.Parse("0\r\n1")).ToString());		
		Debug.Log("Correct: 0, Network response: " + nn.Run(Matrix.Parse("1\r\n1")).ToString());
	}
}

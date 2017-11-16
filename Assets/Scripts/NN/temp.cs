using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// klasa do testowania działania klasy NeuralNetwork
public class temp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Matrix P = Matrix.Parse("0 0 1 1\r\n0 1 0 1");
		Matrix T = Matrix.Parse("0 1 1 0\r\n0 1 0 1");
		Matrix Pr = Matrix.Parse("0\r\n1");

		int[] layers = new int[2];
		// layers[0] = 20;
		layers[0] = 2;
		layers[1] = 2;		
        var nn = new NeuralNetwork(2, layers);

		Debug.Log(nn.Run(Pr).ToString());
		nn.Learn(P,T,1);
		Debug.Log(nn.Run(Pr).ToString());

	}
}

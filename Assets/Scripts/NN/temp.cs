using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// klasa do testowania działania klasy NeuralNetwork
public class temp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int[] hidden = new int[1];
		hidden[0] = 3;
		// hidden[1] = 1;

		Matrix P = Matrix.Parse("4 2 -1\r\n0.01 -1 3.5\r\n0.01 2 0.01\r\n-1 2.5 -2\r\n-1.5 2 1.5");
		// Debug.Log(P.ToString());
		Matrix T = Matrix.IdentityMatrix(3,3);
		// Matrix T = Matrix.Parse("1 0 0 1\r\n0 1 0 0\r\n0 0 1 0");
		Debug.Log(T.ToString());
		
        var nn = new NeuralNetwork(5,hidden);
		Debug.Log(nn.Run(P).ToString());
		nn.Learn(P,T,100);
		Debug.Log(nn.Run(P).ToString());

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

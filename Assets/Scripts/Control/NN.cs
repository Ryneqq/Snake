using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// klasa do testowania działania klasy NeuralNetwork
public class NN : MonoBehaviour {
	public Snake snake;
	NeuralNetwork nn;
	Matrix P; // examples
	Matrix T; // correct resposes
	bool create = false; // shall we create new nn
	bool examples = false; // examples were loaded from file
	


	// Use this for initialization
	void Start() {
		// Debug.Log()
		if(create){
			int[] layers = new int[3];
			layers[0] = 20;
			layers[1] = 20;
			layers[2] = 2;		
			nn = new NeuralNetwork(100, layers);		
		} else {
			nn = new NeuralNetwork();
			nn.LoadNeuralNetwork();
			Debug.Log("NN loaded");			
		}
		InvokeRepeating("Steer", .3f, .3f);
		
	}

	public void Learn() {
		if(!examples) {
			LoadExamples();
		}
		nn.Learn(P, T, 50000);
		nn.SaveNeuralNetwork();
	}

	private void LoadExamples(){
        string loaded = Load.FromFile("5");
        string[] frames = loaded.Split(';');
		string[] example;
		string p = string.Empty, t = string.Empty;
		for(int i = 0; i < frames.Length - 1; i++){
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

	private float Ceiling(double f){
		if(f > .5f)
			return 1f;
		else if(f < .5f && f > -.5f)
			return 0f;
		else
			return -1f;
	}

	private void Steer(){
		Matrix map = new Matrix(Map.map.GetLength(0) * Map.map.GetLength(1), 1);
		int k = 0;
		for(int i = 0; i < Map.map.GetLength(0); i++){
			for(int j = 0; j < Map.map.GetLength(1); j++){
				map[k,0] = (int)Map.map[i,j].field;
				k++;
			}
		}
		Matrix dir = nn.Run(map);
		if(dir[0,0] > .5f && dir[1,0] < .5f){
			snake.Turn("right");
		} else if(dir[0,0] < .5f && dir[1,0] > .5f){
			snake.Turn("left");
		}
	
		Debug.Log(dir.ToString());
	}

	private void LoadHumanReadableMapsAndSaveToFile(){
		string loaded = Load.FromFile("4na");
        string[] frames = loaded.Split(';');
		string[] example;
		Matrix mat = new Matrix(100,1);

		string p = string.Empty, t = string.Empty, content = string.Empty;
		for(int i = 0; i < frames.Length - 1; i++){
			example = frames[i].Split(',');
			Matrix pr = Matrix.Parse(example[0]);
			int k = 0;
			for(int m = 0; m < pr.rows; m++){
				for(int n = 0; n < pr.cols; n++){
					mat[k,0] = pr[m,n];
					k++;
				}
			}
			content += mat.ToString() + "," + example[1] + ";";
		}

		Save.ToFile("5", content);
	}
}

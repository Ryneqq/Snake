using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NN : MonoBehaviour {
	Snake snake;
	NeuralNetwork nn;
	Matrix P; // examples
	Matrix T; // correct resposes
	bool create = true; // shall we create new nn
	bool examples = false; // examples were loaded from file
	float distance;
	


	// Use this for initialization
	void Start() {
		snake = GetComponent<Snake>();
		distance = 10f;
		if(create){
			int[] layers = new int[3];
			layers[0] = 8;
			layers[1] = 4;
			layers[2] = 2;		
			nn = new NeuralNetwork(6, layers);	
			Learn();	
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
		nn.SaveNeuralNetwork(); // to tylko odpalić dla najlepszego snake'a
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

	private void Steer(){
		Matrix perceptiont = new Matrix(6, 1);
		int x,y;
		snake.Turn("right");
		x = (int)snake.Dir().x; y = (int)snake.Dir().y;
		perceptiont[0,0] = (int)Map.map[snake.Head().x + x, snake.Head().y + y].field;
		snake.Turn("left");
		x = (int)snake.Dir().x; y = (int)snake.Dir().y;		
		perceptiont[2,0] = (int)Map.map[snake.Head().x + x, snake.Head().y + y].field;
		snake.Turn("left");
		x = (int)snake.Dir().x; y = (int)snake.Dir().y;		
		perceptiont[4,0] = (int)Map.map[snake.Head().x + x, snake.Head().y + y].field;
		snake.Turn("right");
		x = snake.Head().x + (int)snake.Dir().x; 
		y = snake.Head().y + (int)snake.Dir().y;
		snake.Turn("right");
		x += (int)snake.Dir().x; y += (int)snake.Dir().y;
		perceptiont[1,0] = (int)Map.map[x,y].field; 
		x -= (int)snake.Dir().x; y -= (int)snake.Dir().y;
		snake.Turn("left");
		snake.Turn("left");
		x += (int)snake.Dir().x; y += (int)snake.Dir().y;		
		perceptiont[3,0] = 	(int)Map.map[x,y].field;
		snake.Turn("right");

		if(distance > Vector2.Distance(snake.Head().pos, snake.food.Position())){
			perceptiont[5,0] = -1;
		} else {
			perceptiont[5,0] = 1;
		}
		distance =  Vector2.Distance(snake.Head().pos, snake.food.Position());
		// network doesn't really get it how to go through 3 and not through 2 and 1
		for(int i = 0; i < perceptiont.rows; i++){
			if(perceptiont[i,0] == 3){
				perceptiont[i,0] = -1;
				Debug.Log("zamienilem");
			}
		}

		Matrix dir = nn.Run(perceptiont);
		if(dir[0,0] > .5f && dir[1,0] < .5f){
			snake.Turn("right");
		} else if(dir[0,0] < .5f && dir[1,0] > .5f){
			snake.Turn("left");
		}
	
		Debug.Log(dir.ToString());
	}
}

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

	private int CheckIfApproachingToFood (float current, float previous) {
		if(previous > current){
			return -1;
		} else {
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

	private void CreatePerception(Matrix perception) {
		int x,y;

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
		perception[3,0] = 	(int)Map.map[x,y].field;
		snake.Turn("right");

		var next = Vector2.Distance(snake.Head().pos, snake.food.Position());
		perception[5,0] = CheckIfApproachingToFood(distance, next);
		distance = next;
	}

	private void Steer(){
		Matrix perception = new Matrix(6, 1);
		CreatePerception(perception);
		ChangePerceptionOfTheFood(perception);
		ChangeDirection(perception);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {
	public Transform tailPrefab;
	public Food food;
	List<Field> tail;
	List<Transform> tailTransforms;
	List<string> frames;
	Vector2 dir;

	void Start(){
		tail = new List<Field>();
		tailTransforms = new List<Transform>();
		frames = new List<string>();		
		dir = Vector2.right;
		Create(Map.map[4,4], 4);
		dir = Vector2.left;
	}

	public void Create (Field head, int length) {
		int x = (int)dir.x, y = (int)dir.y;		
		tail.Add(head);
		for(int i = 1; i < length; i++){
			tail.Add(Map.map[tail[i-1].x + x, tail[i-1].y + y]);
		} 
		for(int  i = 0 ; i < tail.Count; i++){
			Map.map[tail[i].x,tail[i].y].ChangeField(Map.Fields.tail);
			Spawn(tail[i].pos);			
		}

		InvokeRepeating("Move", 0.3f, 0.3f); 		
	}

	public void ChangeDirection(Vector2 _dir) {
		if(dir.x == _dir.x && dir.y == -_dir.y)
			return;
		if(dir.x == -_dir.x && dir.y == _dir.y)
			return;
		if(_dir.x == 0 && _dir.y == 0)
			return;
		if(_dir.x == 0 || _dir.y == 0)
			dir = _dir;
	}

	public void Turn(string side){

		if(side == "right"){
			if(dir.x == 1){
				dir.x = 0;
				dir.y = -1;
			} else if (dir.x == -1) {
				dir.x = 0;
				dir.y = 1;
			} else if (dir.y == 1){
				dir.x = 1;
				dir.y = 0;
			} else {
				dir.x = -1;
				dir.y = 0;
			}
		} else {
			if(dir.x == 1){
				dir.x = 0;
				dir.y = 1;
			} else if (dir.x == -1) {
				dir.x = 0;
				dir.y = -1;
			} else if (dir.y == 1){
				dir.x = -1;
				dir.y = 0;
			} else {
				dir.x = 1;
				dir.y = 0;
			}
		}
	}

	private void Move() {
		int x = (int)dir.x, y = (int)dir.y;
		int last = tail.Count-1;
		Field next = Map.map[tail[0].x + x, tail[0].y + y];
		if(!next.IsWalkable()){
			//dead
			Debug.Log("dead");
			return;
		}

		if(next.field != Map.Fields.food) {
			Map.map[tail[last].x, tail[last].y].ChangeField(Map.Fields.empty);
		} else {
			food.Eat();
			Spawn(tail[last].pos);
			x = tail[last].x; y = tail[last].y;
		}	

		for(int i = last; i > 0; i--){
			tail[i] = tail[i-1];
		}
		next.ChangeField(Map.Fields.tail);
		tail[0] = next;

		if (tailTransforms.Count != tail.Count){
			tail.Add(Map.map[x,y]);
		}

		for(int  i = 0 ; i < tail.Count; i++){
			tailTransforms[i].position = tail[i].pos;
		}
		// Register();
	}

	private void Spawn(Vector2 pos){
		Transform temp = Instantiate(tailPrefab, pos, Quaternion.identity);
        temp.transform.SetParent(GetComponent<Transform>(), false);
		tailTransforms.Add(temp);
	}

	private void Register(){
		Matrix _map = new Matrix(Map.map.GetLength(0) * Map.map.GetLength(1), 1);
		// Matrix _map = new Matrix(Map.map.GetLength(0), Map.map.GetLength(1));

		Matrix _dir = new Matrix(2,1);
		int k = 0;
		for(int i = 0; i < Map.map.GetLength(0); i++){
			for(int j = 0; j < Map.map.GetLength(1); j++){
				_map[k,0] = (int)Map.map[i,j].field;
				k++;
			}
		}
		_dir[0,0] = dir.x;
		_dir[1,0] = dir.y;

		string frame = _map.ToString() + "," + _dir.ToString() + ";";
		frames.Add(frame);
	}
	public void SaveGame(){
		string content = string.Empty;
		foreach (var frame in frames) {
			content += frame;
		}
		Save.ToFile("game", content);
	}
}

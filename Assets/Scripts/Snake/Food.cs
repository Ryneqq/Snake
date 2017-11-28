using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
	public Transform foodPrefab;
	private Transform food;

	void Start(){
		Spawn();
	}

	private void Spawn () {
		int x = 0, y = 0;
		while(!Map.map[x,y].IsWalkable()){
			x = Random.Range(1, Map.map.GetLength(0));
			y = Random.Range(1, Map.map.GetLength(1));
		}
		Map.map[x,y].ChangeField(Map.Fields.food);
				
		food = Instantiate(foodPrefab, Map.map[x,y].pos, Quaternion.identity);
        food.transform.SetParent(GetComponent<Transform>(), false);
	}

	public void Eat(){
		Destroy(food.gameObject);
		Spawn();
	}

	public Vector2 Position(){
		return food.position;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
    public Transform foodPrefab;
    public int x = 0, y = 0;
    private Transform food;

    void Start(){
        Spawn();
    }

    private Vector2 FindEmptyField() {
        while(!Map.map[x,y].IsEmpty()){
            x = Random.Range(1, Map.map.GetLength(0));
            y = Random.Range(1, Map.map.GetLength(1));
        }
        Map.map[x,y].ChangeField(Map.Fields.food);

        return Map.map[x,y].pos;
    }

    private void Spawn () {
        food = Instantiate(foodPrefab, FindEmptyField(), Quaternion.identity);
        food.transform.SetParent(GetComponent<Transform>(), false);
        food.GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void Eat(){
        Destroy(food.gameObject);
        Spawn();
    }

    public Vector2 Position(){
        return food.position;
    }
}

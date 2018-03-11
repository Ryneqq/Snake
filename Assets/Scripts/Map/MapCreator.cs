using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {
    public Transform plane;
    public Transform wall;
    private Vector2 mapSize;
    private float fieldSize;
    private int x, y;

	void Awake () {
        Setup();
        CreateEmpty();
        Fill();
        Spawn();
	}

    private void Setup() {
        x = 40;
        y = 40;
        fieldSize = 0.8f;
        mapSize = new Vector2(32,32);
    }

    private void CreateEmpty() {
        Vector2 start = new Vector2((mapSize.x - fieldSize) / (-2), (mapSize.y - fieldSize) / (-2)); // top-left corrner
        Vector2 actual = new Vector2(start.x, start.y);

        // creating map
        Map.map = new Field[x, y];
        for (int i = 0; i < x; i++) {
            for (int j = 0; j < y; j++) {
                Map.map[i, j] = new Field(i, j, actual);
                actual.y += fieldSize;
            }
            actual.x += fieldSize;
            actual.y = start.y;
        }
    }

    private void Fill() {
        // walls around the map
        for (int i = 0; i < x; i++) {
            Map.map[i, 0].ChangeField(Map.Fields.wall);
            Map.map[i, y-1].ChangeField(Map.Fields.wall);
        }
        for (int j = 0; j < y; j++) {
            Map.map[0, j].ChangeField(Map.Fields.wall);
            Map.map[x-1, j].ChangeField(Map.Fields.wall);
        }
    }

    private void Spawn() {
        Transform temp;

        foreach (var m in Map.map) {
            if(m.field == Map.Fields.wall) {
                temp = Instantiate(wall, m.pos, Quaternion.identity);
                temp.SetParent(plane);
            }
        }
    }
}
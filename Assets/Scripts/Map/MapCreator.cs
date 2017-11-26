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
        x = 10;
        y = 10;
        fieldSize = 0.8f;
        mapSize = new Vector2(8,8);
    }

    private void CreateEmpty() {
        // position
        Vector2 start = new Vector2((mapSize.x - fieldSize) / (-2), (mapSize.y - fieldSize) / (-2)); // top-left corrner
        Vector2 actual = new Vector2(start.x, start.y); // copy

        // creating map
        Map.map = new Field[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Map.map[i, j] = new Field();
                Map.map[i, j].pos = actual;
                Map.map[i, j].x = i; Map.map[i, j].y = j;   
                actual.y += fieldSize;
            }
            actual.x += fieldSize;
            actual.y = start.y;
        }
    }

    private void Fill()
    {
        // walls around the map
        for (int i = 0; i < x; i++)
        {
            Map.map[i, 0].field = Map.Fields.wall;
            Map.map[i, y-1].field = Map.Fields.wall;
        }
        for (int j = 0; j < y; j++)
        {
            Map.map[0, j].field = Map.Fields.wall;
            Map.map[x-1, j].field = Map.Fields.wall;
        }
    }

    private void Spawn()
    {
        Transform tempField;
        foreach (var m in Map.map)
        {
            if(m.field == Map.Fields.wall)
            {
                m.walkable = false;
                tempField = Instantiate(wall, new Vector2(m.pos.x, m.pos.y), Quaternion.identity);
                tempField.SetParent(plane);
            }
        }
    }
}
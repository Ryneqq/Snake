using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {
    public Vector2 mapSize;
    public float fieldSize;
    public Transform plane;
    public Transform wall;
    private int x, y;

	void Awake () {
        Setup();
        CreateEmpty();
        Fill();
        Spawn();
	}

    private void Setup()
    {
        // count fields on both axis
        x = Mathf.RoundToInt(mapSize.x / fieldSize);
        y = Mathf.RoundToInt(mapSize.y / fieldSize);
        // scale plane up to the mapSize
        plane.localScale = new Vector3(mapSize.x / 10, 1, mapSize.y / 10);
    }

    private void CreateEmpty()
    {
        // position
        Vector2 start = new Vector2((mapSize.x - fieldSize) / (-2), (mapSize.y - fieldSize) / (-2)); // top-left corrner
        Vector2 actual = new Vector2(start.x, start.y); // copy

        // creating map
        GameMode.map = new Field[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameMode.map[i, j] = new Field();
                GameMode.map[i, j].pos = actual;
                GameMode.map[i, j].x = i; GameMode.map[i, j].y = j;   
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
            GameMode.map[i, 0].field = GameMode.Fields.wall;
            GameMode.map[i, y-1].field = GameMode.Fields.wall;
        }
        for (int j = 0; j < y; j++)
        {
            GameMode.map[0, j].field = GameMode.Fields.wall;
            GameMode.map[x-1, j].field = GameMode.Fields.wall;
        }
    }

    private void Spawn()
    {
        Transform tempField;
        foreach (var m in GameMode.map)
        {
            if(m.field == GameMode.Fields.wall)
            {
                tempField = Instantiate(wall, new Vector3(m.pos.x, fieldSize/2, m.pos.y), Quaternion.identity);
                tempField.SetParent(plane);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class tracks informations about one separte field.
/// </summary>
public class Field { 
    public GameMode.Fields field;
    public Vector2 pos; // position in the world
    public int x, y; // position in the map
	public int h, g; // movement costs
    public int f {
		get { return h+g; }
	}
	public Field parent;
	public bool walkable; 


    public Field()
    {
        field = GameMode.Fields.empty;
        walkable = true;
    }

    public bool IsEmpty()
    {
        if (field == GameMode.Fields.empty)
        {
            return true;
        }
        return false;
    }
}

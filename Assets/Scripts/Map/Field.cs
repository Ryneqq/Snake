using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class tracks informations about one separte field.
/// </summary>
public class Field {
    public Map.Fields field;
    public Vector2 pos; // position in the world
    public int x, y; // position in the map
    public int h, g; // movement costs
    public int f {
        get { return h+g; }
    }
    public Field parent;
    public bool walkable;


    public Field() {
        field = Map.Fields.empty;
        walkable = true;
    }

    public Field(int _x, int _y, Vector2 _pos) {
        field = Map.Fields.empty;
        walkable = true;
        x = _x;
        y = _y;
        pos = _pos;
    }
    public void ChangeField(Map.Fields _field) {
        field = _field;

        if(IsEmpty() || IsFood())
            walkable = true;
        else
            walkable = false;
    }
    public bool IsWalkable() {
        if (IsEmpty() || IsFood())
            return true;
        return false;
    }

    public bool IsEmpty() {
        if (field == Map.Fields.empty)
            return true;
        return false;
    }

    public bool IsFood() {
        if (field == Map.Fields.food)
            return true;
        return false;
    }
}

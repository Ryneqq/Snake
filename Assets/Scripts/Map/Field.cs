using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class tracks informations about one separte field.
/// </summary>
public class Field {
    public Map.Fields field;
    public Vector2 pos; // position in the world
    public int x, y;    // position in the map
    public int h, g;    // movement costs
    public int f {
        get { return h+g; }
    }
    public Field parent;
    public bool walkable;


    public Field() {
        this.field = Map.Fields.empty;
        this.walkable = true;
    }

    public Field(int x, int y, Vector2 pos)
    {
        this.field = Map.Fields.empty;
        this.walkable = true;
        this.x = x;
        this.y = y;
        this.pos = pos;
        this.parent = this;
    }

    public void ChangeField(Map.Fields field)
    {
        this.field = field;
        this.walkable = this.IsWalkable();
    }

    public bool IsWalkable()
    {
        return this.IsEmpty() || this.IsFood();
    }

    public bool IsEmpty()
    {
        return this.field == Map.Fields.empty;
    }

    public bool IsFood()
    {
        return field == Map.Fields.food;
    }
}

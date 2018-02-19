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


    public Field()
    {
        field = Map.Fields.empty;
        walkable = true;
    }
    public void ChangeField(Map.Fields _field){
        field = _field;
        if(field == Map.Fields.empty || field == Map.Fields.food){
            walkable = true;
        }  else {
            walkable = false;
        }
    }
    public bool IsWalkable()
    {
        if (field == Map.Fields.empty || field == Map.Fields.food)
        {
            return true;
        }
        return false;
    }
}

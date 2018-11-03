using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class designed to keep map.
/// </summary>
public static class Map {
    /// <summary>
    /// Type of Fields used to build map.
    /// </summary>
    public enum Fields { empty, wall, tail, food };
    /// <summary>
    /// Sides of map.
    /// </summary>
    public enum Side {up, right, down, left};
    public static Field[,] map;

    public static Field Neighbour(Field field, Side side){
        var dir = Direction(side);
        int x = (int)dir.x, y = (int)dir.y;

        return map[field.x + x, field.y + y];
    }

    public static List<Field> GetNeighbours(Field field)
    {
        var fields = new List<Field>();
        var side = Side.up;

        for(int i = 0; i < 4; i++)
        {
            var neighbour = Neighbour(field, side);
            fields.Add(neighbour);
            side = Right(side);
        }

        return fields;
    }

    public static Vector2 Direction(Side side) {
        switch (side) {
            case Side.right: 
                return Vector2.right;
            case Side.down: 
                return Vector2.down;
            case Side.left:
                return Vector2.left;
            case Side.up:
                return Vector2.up;
            default:
                return Vector2.zero;
        }
    }

    public static Side Direction(Vector2 dir) {
        if(dir == Vector2.right)
            return Side.right;
        else if(dir == Vector2.left)
            return Side.left;
        else if(dir == Vector2.up)
            return Side.up;
        else
            return Side.down;
    }

    public static Side Direction(Field from, Field to)
    {
        var dir = new Vector2(to.x - from.x, to.y - from.y);
        dir.Normalize();

        return Direction(dir);
    }

    public static Side Right(Side side) {
        int x = (int)side;

        if(++x > 3)
            x = 0;

        return (Side)x;
    }

    public static Side Left(Side side) {
        int x = (int)side;

        if(--x < 0)
            x = 3;

        return (Side)x;
    }
}

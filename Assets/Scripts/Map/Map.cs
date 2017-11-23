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
    public static Field[,] map;

    public static Field WorldPointToField(Vector3 pos){
        for(int i = 0 ; i < map.GetLength(0); i++){
            for(int j = 0; j < map.GetLength(1); j++){
                if(Vector3.Distance(pos, map[i,j].pos) < 0.05f)
                    return map[i,j];
            }
        }
        return new Field();
    }
}

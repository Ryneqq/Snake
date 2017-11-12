using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class designed to keep the most important variables such as map.
/// </summary>
public static class GameMode {
    /// <summary>
    /// Type of Fields used to build map.
    /// </summary>
    public enum Fields { empty, wall, tail, food };
    public static Field[,] map;
}

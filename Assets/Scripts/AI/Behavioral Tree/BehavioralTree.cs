using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehavioralTree {
    public Map.Side Run(Snake snake)
    {
        var forward     = Map.Neighbour(snake.Head(), snake.Direction());
        var right       = Map.Neighbour(snake.Head(), Map.Right(snake.Direction()));
        var left        = Map.Neighbour(snake.Head(), Map.Left(snake.Direction()));
        var neighbours  = new Field[] {forward, right, left};
        var dist        = new Perception(snake).DistanceToFood();

        var desired = this.GetDesiredDirection(dist);
        if(CanDirectionBeFollowed(snake.Head(), neighbours, desired))
        {
            return desired;
        }

        var alternative = this.GetAlternativeDirection(dist);
        if(CanDirectionBeFollowed(snake.Head(), neighbours, alternative))
        {
            return alternative;
        }
        else
        {
            foreach (var neighbour in neighbours)
            {
                if(neighbour.IsWalkable())
                {
                    return Map.Direction(snake.Head(), neighbour);
                }
            }
        }


        return snake.Direction();
    }

    // private void ShiftNeighbours(Field[] neighbours)
    // {

    // }

    private bool CanDirectionBeFollowed(Field start, Field[] neighbours, Map.Side dir)
    {
        foreach (var neighbour in neighbours)
        {
            if(neighbour.IsWalkable() && dir == Map.Direction(start, neighbour))
            {
                return true;
            }
        }

        return false;
    }

    private Map.Side GetDesiredDirection(Vector2 dist)
    {
        Vector2 dir;

        if(Mathf.Abs(dist.x) > Mathf.Abs(dist.y))
        {
            dir = new Vector2(-dist.x / Mathf.Abs(dist.x), 0);
        }
        else
        {
            dir = new Vector2(0, -dist.y / Mathf.Abs(dist.y));
        }

        return Map.Direction(dir);
    }

    private Map.Side GetAlternativeDirection(Vector2 dist)
    {
        Vector2 dir;

        if(Mathf.Abs(dist.x) < Mathf.Abs(dist.y))
        {
            dir = new Vector2(-dist.x / Mathf.Abs(dist.x), 0);
        }
        else
        {
            dir = new Vector2(0, -dist.y / Mathf.Abs(dist.y));
        }

        return Map.Direction(dir);
    }
}

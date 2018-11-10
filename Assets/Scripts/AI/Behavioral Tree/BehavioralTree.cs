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
        var desired     = this.GetPossibleDirection(snake.Direction(), dist);

        if(forward.IsWalkable() && right.IsWalkable() && left.IsWalkable())
        {
            return desired;
        }
        else
        {
            if(IsDirectionFollowable(snake.Head(), neighbours, desired))
            {
                return desired;
            }

            var alternative = Map.Direction(this.GetAlternativeDirection(dist));

            if(alternative != desired && IsDirectionFollowable(snake.Head(), neighbours, alternative))
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
        }

        return snake.Direction();
    }

    // private void ShiftNeighbours(Field[] neighbours)
    // {

    // }

    private bool IsDirectionFollowable(Field start, Field[] neighbours, Map.Side dir)
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

    private Map.Side GetPossibleDirection(Map.Side dir, Vector2 dist)
    {
        var desired = Map.Direction(GetDesiredDirection(dist));

        if(dir == desired || Map.Right(dir) == desired || Map.Left(dir) == desired)
        {
            return desired;
        }
        else
        {
            return Map.Direction(GetAlternativeDirection(dist));
        }
    }

    private Vector2 GetDesiredDirection(Vector2 dist)
    {
            if(Mathf.Abs(dist.x) > Mathf.Abs(dist.y))
            {
                return new Vector2(-dist.x / Mathf.Abs(dist.x), 0);
            }
            else
            {
                return new Vector2(0, -dist.y / Mathf.Abs(dist.y));
            }
    }

    private Vector2 GetAlternativeDirection(Vector2 dist)
    {
            if(Mathf.Abs(dist.x) < Mathf.Abs(dist.y))
            {
                return new Vector2(-dist.x / Mathf.Abs(dist.x), 0);
            }
            else
            {
                return new Vector2(0, -dist.y / Mathf.Abs(dist.y));
            }
    }
}

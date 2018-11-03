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
        var desired     = this.GetPossibleDirection(Map.Direction(snake.Direction()), dist);

        foreach (var neighbour in neighbours)
        {
            if(neighbour.IsFood())
            {
                Debug.Log("Found food");
                return Map.Direction(snake.Head(), neighbour);
            }
        }

        if(forward.IsWalkable() && right.IsWalkable() && left.IsWalkable())
        {
            Debug.Log("following desire and i can see everithing");
            return desired;
        }
        else
        {
            foreach (var neighbour in neighbours)
            {
                if(neighbour.IsWalkable() && desired == Map.Direction(snake.Head(), neighbour))
                {
                    Debug.Log("follwing desire but i cannot see some of fields");
                    return desired;
                }
            }

            // var alternative = Map.Direction(this.GetAlternativeDirection(Map.Direction(snake.Direction()), this.GetDesiredDirection(dist)));

            // foreach (var neighbour in neighbours)
            // {
            //     if(neighbour.IsWalkable() && alternative == Map.Direction(snake.Head(), neighbour))
            //     {
            //         return alternative;
            //     }
            // }

            foreach (var neighbour in neighbours)
            {
                if(neighbour.IsWalkable())
                {
                    Debug.Log("Going blindly");
                    return Map.Direction(snake.Head(), neighbour);
                }
            }
        }

        return snake.Direction();
    }

    private Map.Side GetPossibleDirection(Vector2 dir, Vector2 dist)
    {
        var desiredSide = Map.Direction(GetDesiredDirection(dist));
        var dirSide     = Map.Direction(dir);

        if(Map.Right(dirSide) == desiredSide)
        {
            return desiredSide;
        }
        else if(Map.Left(dirSide) == desiredSide)
        {
            return desiredSide;
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

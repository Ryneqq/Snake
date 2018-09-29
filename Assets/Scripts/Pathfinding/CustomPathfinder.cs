using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPathfinder {
    Field start, target;

    public CustomPathfinder(Field start, Field target)
    {
        this.start  = start;
        this.target = target;
    }

    public Field FindPath()
    {
        if (this.PathExists())
        {
            return RetracePath();
        }
        else
        {
            return this.start;
        }
    }

    private bool PathExists()
    {
        List<Field>    open   = new List<Field>();
        HashSet<Field> closed = new HashSet<Field>();

        this.UpdateCosts(this.start, this.start, 0);
        open.Add(this.start);

        while (open.Count > 0)
        {
            Field current = FindCheapest(open);

            open.Remove(current);
            closed.Add(current);

            if (current == this.target)
            {
                return true;
            }

            this.AddPossibleSteps(current, open, closed);
        }

        return false;
    }

    private Field FindCheapest(List<Field> fields)
    {
        Field current = fields[0];

        foreach (var field in fields)
        {
            if (IsCheaper(current, field))
            {
                current = field;
            }
        }

        return current;
    }

    private bool IsCheaper(Field current, Field next)
    {
        return next.f <= current.f;
    }

    private void AddPossibleSteps(Field current, List<Field> open, HashSet<Field> closed)
    {
        foreach (Field neighbour in Map.GetNeighbours(current))
        {
            if (closed.Contains(neighbour))
            {
                var movementCost = current.g + GetDistance(current, neighbour);

                if (!neighbour.IsWalkable() || movementCost > neighbour.g)
                {
                    continue;
                }
            } 

            AddStep(current, neighbour, open);
        }
    }

    private void AddStep(Field current, Field neighbour,  List<Field> open)
    {
        var movementCost = current.g + GetDistance(current, neighbour);

        if (!open.Contains(neighbour) || movementCost < neighbour.g)
        {

            this.UpdateCosts(current, neighbour, movementCost);

            if (!open.Contains(neighbour) && neighbour.IsWalkable())
            {
                open.Add(neighbour);
            }
        }

    }

    private void UpdateCosts(Field current, Field neighbour, int movementCost)
    {
        neighbour.g = movementCost;
        neighbour.h = GetDistance(neighbour, this.target);
        neighbour.parent = current;
    }

    private Field RetracePath()
    {
        Field current = this.target;

        while(current.parent != this.start)
        {
            current = current.parent;
        }

        return current;
    }

    private int GetDistance(Field from, Field to)
    {
        int distanceX = Math.Abs(from.x - to.x);
        int distanceY = Math.Abs(from.y - to.y);

        return (distanceX + distanceY) * 10;
    }
}
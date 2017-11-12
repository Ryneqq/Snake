using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	Class handle the pathfinding.
/// </summary>
public class Pathfinder : MonoBehaviour {
	/// <summary>
	/// Method would return the path as Array of Vector3 between two fileds of map.
	/// </summary>
	// IN: Begin of a path and the target.
	// OUT: Array which contains the path.
	public Vector3[] FindPath(Field start, Field end){
		List<Field> open = new List<Field>();
		List<Field> closed = new List<Field>();
		open.Add(start);

		Field current = start;

		while(open.Count > 0){

			current = open[0];
			foreach(var o in open)
				if(o.f < current.f)
					current = o;
			
			open.Remove(current);

			if(current == end)
				break;

			List<Field> successors = FindSuccessors(current);

			foreach(var s in successors){
				int cost = current.g + Distance(current, s);

				if(open.Contains(s)){
					if(s.g <= cost)
						continue;
				} 
				else if(!s.walkable || closed.Contains(s)){
					if(s.g <= cost)
						continue;
					closed.Remove(s);
					open.Add(s);
				} else {
					open.Add(s);
					s.h = Distance(s, end);
				}
				s.g = cost;
				s.parent = current;
			}
			closed.Add(current);	
		}

		if(current != end)
			return null;
		else 
			return RetracePath(end);

	}

	private List<Field> FindSuccessors(Field n){
		//TODO: wyszukaj sąsiadów w ścieżce
		return new List<Field>();
	}

	private int Distance(Field start, Field end){
		//TODO: policz dystans
		return 0;
	}
	private Vector3[] RetracePath(Field n){
		// TODO: odtwórz drogę z rodziców
		return null;
	}
}

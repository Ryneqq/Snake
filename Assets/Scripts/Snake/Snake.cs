using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour {
    public Transform tailPrefab;
    public Color color;
    public Food food;
    public int x,y;
    public int length;

    List<Field> tail;
    List<Transform> tailTransforms;
    Map.Side dir;

    void Start() {
        tail = new List<Field>();
        tailTransforms = new List<Transform>();

        ChangeDirection(Map.Side.right);
        Create(Map.map[x,y], length);
        ChangeDirection(Map.Side.left);
    }

    public Map.Side Direction(){
        return dir;
    }

    public void ChangeDirection(Map.Side side) {
        dir = side;
    }

    public void Turn(Map.Side side) {
        switch(side) {
            case Map.Side.right:
                dir = Map.Right(dir);
                break;
            case Map.Side.left:
                dir = Map.Left(dir);
                break;
            default:
                break;
        }
    }

    public void Move() {
        Move(Map.Neighbour(Head(), Direction()));
    }

    public void Move(Field next)
    {
        var last = tail.Last();

        if(!next.IsWalkable()){
            // Die();
            Debug.Log("dead");
            return;
        }

        if(next.IsFood()) {
            food.Eat();
            AddTail(last);
            Slither(next);
        } else {
            last.ChangeField(Map.Fields.empty);
            Slither(next);
        }
    }

    private void Die() {
        foreach (var field in tail) {
            field.ChangeField(Map.Fields.empty);
        }

        Destroy(this.gameObject);
    }

    public Field Head(){
        return this.tail[0];
    }

    public Field Food(){
        var position = this.food.Position();

        return Map.map[(int)position.x, (int)position.y];
    }

    private void AddTail(Field tail) { // To map and scene
        tail.ChangeField(Map.Fields.tail);
        this.tail.Add(tail);
        Spawn(tail.pos);
    }

    private void Create (Field head, int length) {
        AddTail(head);

        for(int i = 0; i < length - 1; i++)
            AddTail(Map.Neighbour(tail.Last(), Direction()));
    }

    private void Slither(Field next) {
        for(int i = tail.Count - 1; i > 0; i--)
            tail[i] = tail[i-1];

        next.ChangeField(Map.Fields.tail);
        tail[0] = next;

        for(int  i = 0; i < tail.Count; i++)
            tailTransforms[i].position = tail[i].pos;
    }

    private void ChangeColor(Transform t) {
        t.GetComponent<SpriteRenderer>().color = color;
        color = Color.Lerp(color, Color.white, .1f);
    }

    private void Spawn(Vector2 pos){
        Transform temp = Instantiate(tailPrefab, pos, Quaternion.identity);
        temp.transform.SetParent(GetComponent<Transform>(), false);
        ChangeColor(temp);
        tailTransforms.Add(temp);
    }
}

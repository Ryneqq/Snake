using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour {
    public Transform tailPrefab;
    public Food food;
    public int x,y;
    public int length;
    public Color color;
    List<Field> tail;
    List<Transform> tailTransforms;
    Vector2 dir;

    void Start(){
        tail = new List<Field>();
        tailTransforms = new List<Transform>();

        ChangeDirection(Map.Side.right);
        Create(Map.map[x,y], length);
        ChangeDirection(Map.Side.left);

        InvokeRepeating("Move", 0.3f, 0.3f);
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

    public Map.Side Direction(){
        return Map.Direction(dir);
    }

    public void ChangeDirection(Map.Side side) {
        dir = Map.Direction(side);
    }

    public void Turn(string side){

        if(side == "right"){
            if(dir.x == 1){
                dir.x = 0;
                dir.y = -1;
            } else if (dir.x == -1) {
                dir.x = 0;
                dir.y = 1;
            } else if (dir.y == 1){
                dir.x = 1;
                dir.y = 0;
            } else {
                dir.x = -1;
                dir.y = 0;
            }
        } else {
            if(dir.x == 1){
                dir.x = 0;
                dir.y = 1;
            } else if (dir.x == -1) {
                dir.x = 0;
                dir.y = -1;
            } else if (dir.y == 1){
                dir.x = -1;
                dir.y = 0;
            } else {
                dir.x = 1;
                dir.y = 0;
            }
        }
    }

    private void Slither(Field next) {
        for(int i = tail.Count - 1; i > 0; i--)
            tail[i] = tail[i-1];

        next.ChangeField(Map.Fields.tail);
        tail[0] = next;

        for(int  i = 0; i < tail.Count; i++)
            tailTransforms[i].position = tail[i].pos;
    }

    private void Move() {
        var next = Map.Neighbour(Head(), Direction());
        var last = tail.Last();

        if(!next.IsWalkable()){
            // Destroy(this.gameObject);
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

    private void ChangeColor(Transform t) {
        t.GetComponent<SpriteRenderer>().color = color;
        color = Color.Lerp(color, Color.white, .15f);
    }

    private void Spawn(Vector2 pos){
        Transform temp = Instantiate(tailPrefab, pos, Quaternion.identity);
        temp.transform.SetParent(GetComponent<Transform>(), false);
        ChangeColor(temp);
        tailTransforms.Add(temp);
    }

    public Vector2 Dir(){
        return dir;
    }
    public Field Head(){
        return tail[0];
    }
}

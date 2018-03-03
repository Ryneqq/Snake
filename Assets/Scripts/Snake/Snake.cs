using System.Collections;
using System.Collections.Generic;
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
        dir = Vector2.right;
        Create(Map.map[x,y], length);
        dir = Vector2.left;
    }

    private void Create (Field head, int length) {
        int x = (int)dir.x, y = (int)dir.y;
        tail.Add(head);
        for(int i = 1; i < length; i++){
            tail.Add(Map.map[tail[i-1].x + x, tail[i-1].y + y]);
        } 
        for(int  i = 0 ; i < tail.Count; i++){
            Map.map[tail[i].x,tail[i].y].ChangeField(Map.Fields.tail);
            Spawn(tail[i].pos);
        }

        InvokeRepeating("Move", 0.3f, 0.3f); 
    }

    public void ChangeDirection(Vector2 _dir) {
        if(dir.x == _dir.x && dir.y == -_dir.y)
            return;
        if(dir.x == -_dir.x && dir.y == _dir.y)
            return;
        if(_dir.x == 0 && _dir.y == 0)
            return;
        if(_dir.x == 0 || _dir.y == 0)
            dir = _dir;
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

    private void Move() {
        int x = (int)dir.x, y = (int)dir.y;
        int last = tail.Count-1;
        Field next = Map.map[tail[0].x + x, tail[0].y + y];
        if(!next.IsWalkable()){
            //dead
            // Destroy(this.gameObject);
            Debug.Log("dead");
            return;
        }

        if(next.field != Map.Fields.food) {
            Map.map[tail[last].x, tail[last].y].ChangeField(Map.Fields.empty);
        } else {
            food.Eat();
            Spawn(tail[last].pos);
            x = tail[last].x; y = tail[last].y;
        }

        for(int i = last; i > 0; i--){
            tail[i] = tail[i-1];
        }
        next.ChangeField(Map.Fields.tail);
        tail[0] = next;

        if (tailTransforms.Count != tail.Count){
            tail.Add(Map.map[x,y]);
        }

        for(int  i = 0 ; i < tail.Count; i++){
            tailTransforms[i].position = tail[i].pos;
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

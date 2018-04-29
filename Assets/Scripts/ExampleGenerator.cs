using UnityEngine;

public class ExampleGenerator: MonoBehaviour {
    void Start() {
        Debug.Log(CreateExample());
    }

    private Matrix ClearView() {
        var view = new Matrix(2,3,0);
        view[1,1] = 2;

        return view;
    }

    private Vector2 Direction(Map.Side side) {
        return Map.Direction(side);
    }

    private Vector2 Distance(int quarter) {
        var x = Random.Range(1, 15);
        var y = Random.Range(1, 15);

        switch (quarter) {
            case 1: 
                return new Vector2(x,y);
            case 2: 
                return new Vector2(-x,y);
            case 3: 
                return new Vector2(x,-y);
            case 4: 
                return new Vector2(-x,-y);
            default:
                return Vector2.zero;
        }
    }

    public string CreateExample() {
        var example = ClearView().ToString();
        var dir = Direction(Map.Side.right);
        var dist = Distance(1);

        example += dir.x + " " + dir.y + "\n";
        example += dist.x + " " + dist.y + ";\n";

        return example;
    }

    private Vector2 Anwser() {
        return Vector2.zero;
    }
}